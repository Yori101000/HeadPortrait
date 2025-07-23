///=====================================================
/// - FileName:      GiftModel.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/7/23 11:38:29
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using UnityEngine;
using System;
namespace Slap
{
    [Registration(typeof(Slap.Push))]
    public class GiftModel : AbstractModel
    {
        public GiftsConfig giftsConfig;
        public override void Init()
        {
            giftsConfig = Resources.Load<GiftsConfig>(ConstModel.GiftsConfigPath);
        }


    }
}
