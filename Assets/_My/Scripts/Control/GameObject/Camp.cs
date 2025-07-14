///=====================================================
/// - FileName:      Camp.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/7/14 15:14:34
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using UnityEngine;
using System;
using System.Collections.Generic;
namespace Slap
{
    public class Camp : MonoBehaviour
    {
        public bool hasDead { get; set; } = false;
        public int health { get; set; }
        public int point { get; set; } = 0;      //当前阵容积分
        public int winPoint { get; set; } = 0;  //当前阵容胜点
        public int maxWeapon { get; private set; } = 6;
        public PlayerData.CampType aimCamp = PlayerData.CampType.None;
        public List<GameObject> list_Weapon { get; set; }
        public PlayerData.CampType campType { get; private set; } = PlayerData.CampType.None;

        public void Init(PlayerData.CampType _campType)
        {
            //初始化武器列表
            list_Weapon = new List<GameObject>();
            for (int i = 0; i < maxWeapon; i++)
            {
                list_Weapon.Add(null);
            }

            campType = _campType;

        }

        public void ReduceHealth(int damage)
        {
            health -= damage;
            Debug.Log(health);
        }

    }
}
