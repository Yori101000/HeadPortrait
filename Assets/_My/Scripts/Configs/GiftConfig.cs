///=====================================================
/// - FileName:      GiftConfig.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/7/1 17:01:40
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using UnityEngine;
using System;
using UnityEngine.Video;
using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using Slap.UI;
namespace Slap
{
    [CreateAssetMenu(fileName = "GiftConfig", menuName = "Config/GiftConfig"), SerializeField]
    public class GiftsConfig : ScriptableObject
    {
        //文本
        public string giftName;
        public GiftPopData[] popDatas = new GiftPopData[7];
        public GiftPropData[] propDatas = new GiftPropData[7];
        public GiftAnimationData[] animtionDatas = new GiftAnimationData[7];
        public GiftScoreData[] scoreDatas = new GiftScoreData[7];

    }

    [System.Serializable]
    public class GiftPopData
    {
        //弹窗选择
        public PopType popType;
        //对应图片
        public Sprite giftIcon;
        //数量
        public int number;

    }

    [Serializable]
    public class GiftPropData
    {
        //位置在道具动画中处理
        //道具预制体
        public GameObject propPre;
        
        public int propCount;
        public PropType type;

        public enum PropType
        {
            Weapon,
            ThrowableProp
        }
    }

    //填充动画数据
    [Serializable]
    public class GiftAnimationData
    {
        public string AnimationName;
        public AnimationType type;

        public enum AnimationType
        {
            windowed,
            Full
        }
    }
    
    [System.Serializable]
    public class GiftScoreData
    {
        public int baseScore;
        public float duration;
    }

    
}
