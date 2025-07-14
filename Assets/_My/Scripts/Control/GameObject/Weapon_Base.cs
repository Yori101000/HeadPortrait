///=====================================================
/// - FileName:      Weapon_Base.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/7/11 17:51:32
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using UnityEngine;
using System;
using XFABManager;
using System.Collections;
using YukiFrameWork.UI;
using Slap.UI;
namespace Slap
{
    public class Weapon_Base : MonoBehaviour, IController
    {
        //自身配置
        [SerializeField] private GameObject bulletPre;
        [SerializeField] private Transform fireTrans;
        private Transform bulletParent;

        [Header("武器设置")]
        [SerializeField] private int damage;
        [SerializeField] private float attackInterval;
        public int bulletCount { set; get; }
        public int grade { set; get; }

        [Header("子弹设置")]
        [SerializeField] private Sprite bulletIcon;
        [SerializeField] private float speed;
        [SerializeField] private float size;
        [SerializeField] private GameObject hitEffect;

        [Header("瞄准缓动")]
        [SerializeField] private float aimSmoothSpeed = 4f;

        private float fireTimer = 0f;

        private GlobalDataSystem globalDataSystem;

        private PlayerData.CampType curCamp = PlayerData.CampType.None;
        // private PlayerData.CampType aimCamp => globalDataSystem.campModel.dic_CampData[curCamp.ToString()].aimCamp;
        private PlayerData.CampType aimCamp = PlayerData.CampType.camp2;
        private Transform aimCampTrans => globalDataSystem.campModel.dic_CampData[aimCamp.ToString()].transform;
        void Start()
        {
            globalDataSystem = this.GetSystem<GlobalDataSystem>();
            bulletParent = UIKit.GetPanel<CharacterPanel>().Find("BulletParent").transform;

        }

        private void Update()
        {
            Aim();
            if (aimCamp != PlayerData.CampType.None)
                HandleFire();
        }

        private void HandleFire()
        {
            // 累加计时器
            fireTimer += Time.deltaTime;

            // 达到攻击间隔
            if (fireTimer >= attackInterval)
            {
                Fire();
                fireTimer = 0f; // 重置定时器
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="campType">武器所属阵营</param>
        public void Init(PlayerData.CampType campType) => curCamp = campType;

        private void Fire()
        {
            if (bulletPre != null)
            {
                var bullet = GameObjectLoader.Load(bulletPre, bulletParent);
                bullet.transform.position = fireTrans.position;

                bullet.GetComponent<Bullet>()?.Init(bulletIcon, damage, aimCampTrans, speed, aimCamp, size, hitEffect);
            }
        }        

        private void Aim()
        {
            if (aimCampTrans == null) return;

            Vector3 dir = aimCampTrans.position - transform.position;
            dir.z = 0;

            if (dir != Vector3.zero)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

                // 平滑旋转：使用 Lerp 插值
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    targetRotation,
                    Time.deltaTime * aimSmoothSpeed
                );
            }
        }
        void OnDestroy()
        {
            GameObjectLoader.UnLoad(this.gameObject);
        }

        public IArchitecture GetArchitecture()
        {
            return Push.Global;
        }

    }
}
