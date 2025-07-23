///=====================================================
/// - FileName:      MainMenuState.cs
/// - NameSpace:     Slap
/// - Description:   YUKI 有限状态机构建状态类
/// - Creation Time: 2025/6/24 12:02:30
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork.Machine;
using YukiFrameWork;
using YukiFrameWork.UI;
using Slap.UI;
namespace Slap
{
	public class MainMenuState : StateBehaviour
	{
		public override void OnEnter()
		{
			// 打开MainMenu面板
			var panel = UIKit.ShowPanel<MainMenuPanel>();
			panel.OnClickBtn(async () =>
			{
			
				(this.GetSystem<IGlobalDataSystem>() as GlobalDataSystem).campModel.campCount = 2;

				//TODO 使用网络获取当前的战斗阵营数量
				//这里是测试用的
				(this.GetSystem<IOnlineSystem>() as OnlineSystem).GetCampNum(3);

				UIKit.OpenPanel<LoadingPanel>();
				//设置加载的最短时间 这个是框架的等待一秒，性能更好一点
				await CoroutineTool.WaitForSeconds(1);

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
