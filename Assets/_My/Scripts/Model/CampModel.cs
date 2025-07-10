///=====================================================
/// - FileName:      CampModel.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/7/7 13:43:07
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using UnityEngine;
using System;
namespace Slap
{
    [Registration(typeof(Slap.Push))]
    public class CampModel : AbstractModel
    {
        public CampData leftCamp = new CampData() { health = 5 };
        public CampData rightCamp = new CampData() { health = 5 };

 
        public override void Init()
        {

        }
    }
    public class CampData
    {
        public bool hasDead { get; set; }
        public int health { get; set; }
        public int point { get; set; }
        public int winPoint { get; set; }
        public float momentum { get; set; }

        public void InitRound()
        {
            point = 0;
            momentum = 0;
        }

    }
}
