///=====================================================
/// - FileName:      GameModel.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/7/11 10:36:38
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using UnityEngine;
using System;
namespace Slap
{
    [Registration(typeof(Slap.Push))]
    public class GameModel : AbstractModel
    {
        public string PKModel{ get; set; }  //游戏模式，确定是几个人在玩
        public override void Init()
        {

        }


    }
}
