///=====================================================
/// - FileName:      GlobalState.cs
/// - NameSpace:     Slap
/// - Description:   YUKI 有限状态机构建状态类
/// - Creation Time: 2025/6/24 12:02:12
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork.Machine;
using YukiFrameWork.UI;
using Slap.UI;
using YukiFrameWork;
using UnityEngine;
using XFABManager;
using System.Threading.Tasks;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Linq;
// ...existing using...

namespace Slap
{
    public class GlobalState : StateBehaviour
    {
        //面板
        private CharacterPanel characterPanel;

        private GlobalDataSystem globalDataSystem;
        private GiftSystem giftSystem;
        private OnlineSystem onlineSystem;

        public override async void OnEnter()
        {
            await SceneTool.LoadSceneAsync(ConstModel.GameSceneName);

            // 打开UI面板
            UIKit.OpenPanel<BackGroundPanel>();
            characterPanel = UIKit.OpenPanel<CharacterPanel>();
            // UIKit.OpenPanel<GameUIPanel>();
            UIKit.OpenPanel<AnimationPanel>();
            UIKit.OpenPanel<PopPanel>();
            UIKit.OpenPanel<TestPanel>();

            // 解释面板的打开
            // var explainPanel = UIKit.OpenPanel<ExplainPanel>();
            // explainPanel.OnClickClose(() => UIKit.ClosePanel<ExplainPanel>());

            // 系统初始化
            // globalDataSystem = this.GetSystem<GlobalDataSystem>();
            // giftSystem = this.GetSystem<GiftSystem>();
            // onlineSystem = this.GetSystem<OnlineSystem>();

            this.GetSystems<IProjectInitSystem>()
            .ForEach(system =>
            {
                system.Start();
                //外部就在这

                if (system is GlobalDataSystem globalDataSystem)
                {
                    this.globalDataSystem = globalDataSystem;
                }
                else if (system is GiftSystem giftSystem)
                {
                    this.giftSystem = giftSystem;
                }
                else if (system is OnlineSystem onlineSystem)
                {
                    this.onlineSystem = onlineSystem;
                }
            });

            
            // 事件绑定,应该在系统内部做
            globalDataSystem.OnLeftRoundWin += () => OnRoundWin(1);
            globalDataSystem.OnRightRoundWin += () => OnRoundWin(2);
            globalDataSystem.OnLeftWin += () => OnGameWin(1);
            globalDataSystem.OnRightWin += () => OnGameWin(2);

            //初始化阵营
            //检查游戏具体人数，并对其进行具体阵营生成
            await InitCamp();

            UIKit.ClosePanel<LoadingPanel>();

        }

        public override void OnUpdate()
        {
            globalDataSystem?.Update();
        }

        public override void OnExit()
        {
            // 取消事件绑定
            globalDataSystem.OnLeftRoundWin -= () => OnRoundWin(1);
            globalDataSystem.OnRightRoundWin -= () => OnRoundWin(2);
            globalDataSystem.OnLeftWin -= () => OnGameWin(1);
            globalDataSystem.OnRightWin -= () => OnGameWin(2);

            globalDataSystem.End();
            giftSystem.End();
            onlineSystem.End();
        }

        /// <summary>
        /// 回合胜利结算
        /// </summary>
        private void OnRoundWin(int camp)
        {
            MonoHelper.Instance.StopAllCoroutines();
            giftSystem.Clear(); // 集中销毁所有道具

            // 播放动画(在动画过程中减少血量)



            // 重置阵容数据
            globalDataSystem.InitRoundData();
            UIKit.GetPanel<GameUIPanel>().ResetCharge();
        }

        /// <summary>
        /// 游戏胜利结算
        /// </summary>
        private void OnGameWin(int camp)
        {
            MonoHelper.Instance.StopAllCoroutines();
            giftSystem.Clear(); // 集中销毁所有道具

            // 播放OK动画

            SetInt(ConstModel.StateValue_GameState, (int)GameState.End);
        }


        //全局进行一次初始化
        public async Task InitCamp()
        {

            //获取Camp预制体
            var request = Resources.LoadAsync<GameObject>("Prefabs/Camp");
            while (!request.isDone)
                await Task.Yield(); // 在主线程等待

            GameObject campPre = request.asset as GameObject;

            switch (globalDataSystem.campModel.campCount)
            {
                case 2:
                    globalDataSystem.gameModel.pkMode = GameModel.PKMode.Twosome;
                    break;
                case 3:
                    globalDataSystem.gameModel.pkMode = GameModel.PKMode.Threesome;
                    break;
                case 4:
                    globalDataSystem.gameModel.pkMode = GameModel.PKMode.Foursome;
                    break;
            }

            var mode = characterPanel.transform.Find(globalDataSystem.gameModel.pkMode.ToString());

            Transform[] campParents = new Transform[mode.childCount];

            for (int i = 0; i < mode.childCount; i++)
                campParents[i] = mode.GetChild(i);


            //初始化阵营数据(CampModel)
            for (int i = 0; i < campParents.Length; i++)
            {
                GameObject campObj = GameObjectLoader.Load(campPre, campParents[i]);
                campObj.name = $"Camp_{i}";
                Camp camp = campObj.GetComponent<Camp>();
                camp.Init((PlayerData.CampType)i);


                globalDataSystem.campModel.Dic_Camp.Add(((PlayerData.CampType)i).ToString(), camp);
                characterPanel.list_WeaponParent.Add(camp.Find("WeaponParent").transform);
            }
            globalDataSystem.campModel.List_RealCamp = globalDataSystem.campModel.Dic_Camp.OrderByDescending(c => c.Value.health).Select(c => c.Value).ToList();

            //额外 无阵营
            GameObject noneCamp = new GameObject("None");
            noneCamp.SetParent(mode);
            globalDataSystem.campModel.Dic_Camp.Add(PlayerData.CampType.None.ToString(), noneCamp.AddComponent<Camp>());
        }
    }
}