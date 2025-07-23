///=====================================================
/// - FileName:      OnlineSystem.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/7/12 12:36:45
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using Slap.UI;
using YukiFrameWork.UI;
namespace Slap
{
    public interface IProjectInitSystem : ISystem
    {
        void Start();
        void End();

    }

    public interface IOnlineSystem : IProjectInitSystem { }
    [Registration(typeof(Slap.Push),typeof(IOnlineSystem))]
    public class OnlineSystem : AbstractSystem,IOnlineSystem
    {
        private CharacterPanel characterPanel;
        private GlobalDataSystem globalDataSystem;

        public override void Init()
        {
            globalDataSystem = this.GetSystem<IGlobalDataSystem>() as GlobalDataSystem;
            characterPanel = UIKit.GetPanel<CharacterPanel>();
        }
        public void Start()
        {
           
            // GetCampNum();
        }
        public void End()
        {

        }



        public void GetCampNum(int campNum)
        {
            
            
            globalDataSystem.campModel.campCount = campNum;
           
        }

       



    }
}
