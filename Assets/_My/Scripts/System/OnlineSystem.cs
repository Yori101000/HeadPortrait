///=====================================================
/// - FileName:      OnlineSystem.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/7/12 12:36:45
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Slap.UI;
using XFABManager;
using YukiFrameWork.UI;
namespace Slap
{
    [Registration(typeof(Slap.Push))]
    public class OnlineSystem : AbstractSystem
    {
        private CharacterPanel characterPanel;
        private GlobalDataSystem globalDataSystem;

        public override void Init()
        {

        }
        public void Start()
        {
            globalDataSystem = this.GetSystem<GlobalDataSystem>();
            characterPanel = UIKit.GetPanel<CharacterPanel>();
           
            GetCampNum();
        }
        public void End()
        {

        }



        public void GetCampNum()
        {
            var campNum = 2;
            //TODO 得到玩家数量

            globalDataSystem.campModel.campCount = campNum;
           
        }

       



    }
}
