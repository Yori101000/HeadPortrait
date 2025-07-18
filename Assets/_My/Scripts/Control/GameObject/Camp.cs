///=====================================================
/// - FileName:      Camp.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/7/14 15:14:34
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
namespace Slap
{
    public class Camp : MonoBehaviour
    {
        //常量
        //动画名
        private const string AnimationName_BeHurt = "BeHurt";
        //物体名
        private const string ObjName_LeftPropPoint = "LeftThrowPropPoint";
        private const string ObjName_RightPropPoint = "RightThrowPropPoint";


        [Header("放大设置")]
        private Vector3 originalScale;  //当前实际的基础大小


        [Header("移动设置")]
        [SerializeField] private float moveDuration = 1.5f;
        private const float radianHight = 1;

        [Header("呼吸效果设置")]
        [SerializeField] private float maxScaleMultiplier = 1.05f;
        [SerializeField] private float breathSpeed = 2f;


        [Header("状态")]
        public int health;
        public bool hasDead { get; set; } = false;
        public int point { get; set; } = 0;      //当前阵容积分
        public int winPoint { get; set; } = 0;  //当前阵容胜点
        public int maxWeapon { get; private set; } = 6;
        public PlayerData.CampType aimCamp = PlayerData.CampType.None;
        public List<GameObject> list_Weapon { get; set; }
        public PlayerData.CampType campType { get; private set; } = PlayerData.CampType.None;

    
        public Transform LeftPropPoint { get; set; }
        public Transform RightPropPoint { get; set; }

        private Animator _animator;

        private Coroutine _curMoveCoroutine;

        public void Init(PlayerData.CampType _campType)
        {
            //初始化武器列表
            list_Weapon = new List<GameObject>();
            for (int i = 0; i < maxWeapon; i++)
            {
                list_Weapon.Add(null);
            }

            campType = _campType;

            originalScale = transform.localScale;
        }
        void Awake()
        {
            _animator = GetComponent<Animator>();
            
            LeftPropPoint = transform.Find(ObjName_LeftPropPoint)?.transform;
            RightPropPoint = transform.Find(ObjName_RightPropPoint)?.transform;
        }
        void Update()
        {
            PlayerBreathEffect();
        }
        
        public void PlayerBreathEffect()
        {
            // t: 0 ~ 1
            float t = (Mathf.Sin(Time.time * breathSpeed) + 1f) / 2f;

            // 缩放倍数：1.0 ~ maxScaleMultiplier
            float scale = Mathf.Lerp(1f, maxScaleMultiplier, t);

            transform.localScale = originalScale * scale;
        }

        public void MoveTo(Transform aimTrans, float _radianHight = radianHight)
        {
            if (_curMoveCoroutine != null)
                StopCoroutine(_curMoveCoroutine);
            _curMoveCoroutine = StartCoroutine(MoveAlongCurve(aimTrans, _radianHight));
        }

        private IEnumerator MoveAlongCurve(Transform target, float _radianHight = radianHight)
        {
            //移动和动画会产生冲突，所以在移动过程中不播放动画
            _animator.enabled = false;

            Vector3 start = transform.position;

            // 控制点可以控制弧度的“高度”
            Vector3 control = (start + target.position) / 2 + new Vector3(0, _radianHight, 0);

            float timer = 0f;
            while (timer < moveDuration)
            {
                float t = timer / moveDuration;

                // 二次贝塞尔插值公式
                Vector3 pos = Mathf.Pow(1 - t, 2) * start +
                            2 * (1 - t) * t * control +
                            Mathf.Pow(t, 2) * target.position;

                transform.position = pos;

                timer += Time.deltaTime;
                yield return null;
            }

            transform.position = target.position;
            transform.SetParent(target);

            _animator.enabled = true;
            _curMoveCoroutine = null;
        }

        public void ReduceHealth(int damage)
        {
            if(_animator.enabled)
                _animator.Play(AnimationName_BeHurt);
            health -= damage;
            Debug.Log(health);
        }

    }
}
