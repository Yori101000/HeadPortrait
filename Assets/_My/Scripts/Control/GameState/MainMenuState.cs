///=====================================================
/// - FileName:      MainMenuState.cs
/// - NameSpace:     Slap
/// - Description:   YUKI 有限状态机构建状态类
/// - Creation Time: 2025/6/24 12:02:30
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork.Machine;
using UnityEngine;
using YukiFrameWork;
using YukiFrameWork.UI;
using Slap.UI;
using System.Threading.Tasks;
namespace Slap
{
	public class MainMenuState : StateBehaviour
	{
        public override void OnEnter()
        {
            // 打开MainMenu面板
            var panel = UIKit.ShowPanel<MainMenuPanel>();
            panel.OnClickStart(async () =>
			{
				UIKit.OpenPanel<LoadingPanel>();
				//设置加载的最短时间
				await Task.Delay(1000);
				SetInt(ConstModel.StateValue_GameState, (int)GameState.WaitStart);
			
            });
        }
        public override void OnUpdate()
		{
		}
		public override void OnExit()
		{
			
			UIKit.HidePanel<MainMenuPanel>();
		}

	}
}
