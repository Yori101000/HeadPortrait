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
// ...existing using...

namespace Slap
{
    public class GlobalState : StateBehaviour
    {
        GlobalDataSystem globalDataSystem;
        GiftSystem giftSystem;

        public override async void OnEnter()
        {
            await SceneTool.LoadSceneAsync(ConstModel.GameSceneName);

            // 打开UI面板
            UIKit.OpenPanel<BackGroundPanel>();
            UIKit.OpenPanel<PropPanel>();
            UIKit.OpenPanel<CharacterPanel>();
            UIKit.OpenPanel<GameUIPanel>();
            var explainPanel = UIKit.OpenPanel<ExplainPanel>();
            UIKit.OpenPanel<AnimationPanel>();
            UIKit.OpenPanel<PopPanel>();
            UIKit.OpenPanel<TestPanel>();

            explainPanel.OnClickClose(() => UIKit.ClosePanel<ExplainPanel>());

            // 系统初始化
            globalDataSystem = this.GetSystem<GlobalDataSystem>();
            giftSystem = this.GetSystem<GiftSystem>();

            globalDataSystem.Start();
            giftSystem.Start();

            // 事件绑定
            globalDataSystem.OnLeftRoundWin += () => OnRoundWin(1);
            globalDataSystem.OnRightRoundWin += () => OnRoundWin(2);
            globalDataSystem.OnLeftWin += () => OnGameWin(1);
            globalDataSystem.OnRightWin += () => OnGameWin(2);

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
        }

        /// <summary>
        /// 回合胜利结算
        /// </summary>
        private void OnRoundWin(int camp)
        {
            MonoHelper.Instance.StopAllCoroutines();
            giftSystem.Clear(); // 集中销毁所有道具

            // 播放动画(在动画过程中减少血量)

            //减少血量
            if (camp == 1)
                globalDataSystem.ReduceHealth(2);
            else
                globalDataSystem.ReduceHealth(1);
                

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

            if (camp == 1)
                globalDataSystem.ReduceHealth(2);
            else
                globalDataSystem.ReduceHealth(1);

            SetInt("GameState", (int)GameState.End);
        }
    }
}