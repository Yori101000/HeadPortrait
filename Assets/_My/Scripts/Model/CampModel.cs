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
using System.Collections.Generic;
using System.Linq;
namespace Slap
{
    [Registration(typeof(Slap.Push))]
    public class CampModel : AbstractModel
    {
        public int campCount;
        public Dictionary<string, Camp> dic_camp = new Dictionary<string, Camp>();

        public List<Camp> list_realCamp = new List<Camp>();

        public override void Init()
        {

        }
        public int GetAllPoint()
        {
            var allPoint = 0;
            foreach (var campData in dic_camp)
            {
                allPoint += campData.Value.point;
            }
            return allPoint;
        }
    }

}
