///=====================================================
/// - FileName:      ConstConfig.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/6/26 13:58:25
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using UnityEngine;
using System;
namespace Slap
{
    public static class ConstModel
    {
        public const string ProjectName = "Push";  //项目名字

        public const string DefaultStateMachineCorePath = "Configs/Game";

        public const string GameSceneName = "Game"; //游戏场景名称
        public const string CampTag = "Camp";       //标签用于判断物体是否是Camp


        public const string DefaultStateMachineCoreName = "Game"; //默认状态机核心名称
        //状态机变量
        public const string StateValue_GameState = "GameState";

    }
}
