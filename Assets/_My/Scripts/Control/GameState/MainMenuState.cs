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
			//TODO 临时
			// 打开MainMenu面板
			var panel = UIKit.ShowPanel<MainMenuPanel>();
			panel.OnClickBtn1(async () =>
			{
				this.GetSystem<GlobalDataSystem>().campModel.campCount = 2;
				// this.GetSystem<OnlineSystem>().GetCampNum(2);
				UIKit.OpenPanel<LoadingPanel>();
				//设置加载的最短时间
				await Task.Delay(1000);
				SetInt(ConstModel.StateValue_GameState, (int)GameState.WaitStart);
			});

			panel.OnClickBtn2(async () =>
			{

				this.GetSystem<GlobalDataSystem>().campModel.campCount = 3;
				// this.GetSystem<OnlineSystem>().GetCampNum(3);

				UIKit.OpenPanel<LoadingPanel>();
				//设置加载的最短时间
				await Task.Delay(1000);
				SetInt(ConstModel.StateValue_GameState, (int)GameState.WaitStart);

			});
			panel.OnClickBtn3(async () =>
		   	{
				
				this.GetSystem<GlobalDataSystem>().campModel.campCount = 4;
				// this.GetSystem<OnlineSystem>().GetCampNum(4);

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
