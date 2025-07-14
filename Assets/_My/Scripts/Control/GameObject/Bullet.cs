///=====================================================
/// - FileName:      Bullet.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/7/12 18:33:15
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using XFABManager;
using System.Net.NetworkInformation;
namespace Slap
{
    public class Bullet : MonoBehaviour
    {
        private Sprite sprite;
        private int damage;
        private Transform aimTrans;
        private float speed;
        private Image icon;
        private PlayerData.CampType aimCamp = PlayerData.CampType.None;
        private GameObject hitEffectPre;
        private Transform hitEffectParent;

        private BoxCollider2D boxCollider2D;
        private RectTransform rectTransform;
        public void Init(Sprite _sprite, int _damage, Transform _aimTrans,
                         float _speed, PlayerData.CampType _aimCamp, float _size,
                         GameObject _hitEffectPre)
        {
            sprite = _sprite;
            damage = _damage;
            aimTrans = _aimTrans;
            speed = _speed;
            aimCamp = _aimCamp;
            hitEffectPre = _hitEffectPre;

            var iconRect = icon.GetComponent<RectTransform>();
            iconRect.transform.localScale = new Vector2(_size, _size);


            icon.sprite = _sprite;
            icon.preserveAspect = true;

            // //初始化碰撞器大小
            // boxCollider2D.size = iconRect.rect.size;
            // boxCollider2D.offset = iconRect.rect.center;
        }
        void Awake()
        {
            icon = GetComponent<Image>();
            boxCollider2D = GetComponent<BoxCollider2D>();
            rectTransform = GetComponent<RectTransform>();
            hitEffectParent = GameObject.Find("HitEffectParent").transform;
        }
        void Update()
        {
            TraceAim();
        }
        //追踪目标
        private void TraceAim()
        {
            if (aimTrans != null && aimCamp != PlayerData.CampType.None)
            {
                // 方向向量
                Vector3 direction = (aimTrans.position - transform.position).normalized;

                // 移动
                transform.position += direction * speed * Time.deltaTime;

                // 朝向（只旋转 Z 轴）
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }

        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == ConstModel.CampTag)
            {
                if (collision.GetComponent<Camp>().campType == aimCamp)
                {
                    collision.GetComponent<Camp>().ReduceHealth(damage);
                    var hitEffect = GameObjectLoader.Load(hitEffectPre, hitEffectParent);
                    hitEffect.transform.position = new Vector3(transform.position.x, transform.position.y, 0);

                    GameObjectLoader.UnLoad(this.gameObject);
                }
            }
        }
        
    }
}
