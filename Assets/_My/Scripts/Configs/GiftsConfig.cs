///=====================================================
/// - FileName:      GiftsConfig.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/7/23 11:44:31
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using UnityEngine;
using System;
namespace Slap
{
    [CreateAssetMenu(fileName ="GiftsConfig",menuName = "Config/GiftsConfig")]
    public class GiftsConfig : ScriptableObject
    {
        public GiftConfig[] giftConfigs;
    }
}
