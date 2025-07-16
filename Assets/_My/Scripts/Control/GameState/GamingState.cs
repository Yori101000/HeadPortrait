///=====================================================
/// - FileName:      GamingState.cs
/// - NameSpace:     Slap
/// - Description:   YUKI 有限状态机构建状态类
/// - Creation Time: 2025/7/15 15:10:00
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork.Machine;
using UnityEngine;
using YukiFrameWork;
using Slap.UI;
using YukiFrameWork.UI;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
namespace Slap
{
	public class GamingState : StateBehaviour
	{
		private GlobalDataSystem globalDataSystem;
		private CharacterPanel characterPanel;

		public override void OnEnter()
		{
			globalDataSystem = this.GetSystem<GlobalDataSystem>();
			characterPanel = UIKit.GetPanel<CharacterPanel>();

			UIKit.GetPanel<TestPanel>().OnClickTest(() => { ChangePKMode(globalDataSystem.gameModel.pkMode); });
		}

		public override void OnUpdate()
		{

			base.OnUpdate();
			CheckAndFixOrder(globalDataSystem.campModel.list_realCamp);
		}

		private void CheckAndFixOrder(List<Camp> list)
		{
			// 获取正确的降序排序列表（新的顺序）
			var sortedList = list.OrderByDescending(c => c.health).ToList();

			// 如果顺序没变就退出（可选优化）
			if (Enumerable.SequenceEqual(list, sortedList))
				return;

			// 获取目标 UI 位置信息
			var modeTrans = characterPanel.transform.Find(globalDataSystem.gameModel.pkMode.ToString());
			Transform[] campParents = new Transform[modeTrans.childCount];
			for (int i = 0; i < modeTrans.childCount; i++)
				campParents[i] = modeTrans.GetChild(i);

			var campMoveRadian = 2;

			// 依次移动 camp 到新位置
			for (int i = 0; i < sortedList.Count; i++)
			{
				// 原本这个位置的 camp
				var originalCamp = i < list.Count ? list[i] : null;
				var sortedCamp = sortedList[i];

				// 如果位置不同，才执行移动
				if (originalCamp != sortedCamp)
				{
					sortedCamp.MoveTo(campParents[i], campMoveRadian);
					campMoveRadian = -campMoveRadian;
				}
			}

			// 更新原列表（注意这里是引用赋值）
			globalDataSystem.campModel.list_realCamp = sortedList;

			Debug.Log("已完成排序并重新排列所有阵营位置");
		}




		//TODO 更换PK模式
		private void ChangePKMode(GameModel.PKMode curPKMode)
		{
			var aimPKMode = curPKMode - 1;
			var aimModeTrans = characterPanel.transform.Find(aimPKMode.ToString());


			Transform[] campParents = new Transform[aimModeTrans.childCount];
			for (int i = 0; i < aimModeTrans.childCount; i++)
				campParents[i] = aimModeTrans.GetChild(i);

			for (int i = 0; i < campParents.Length; i++)
			{
				// globalDataSystem.campModel.list_camp[i].MoveTo(campParents[i]);
				globalDataSystem.campModel.dic_camp[((PlayerData.CampType)i).ToString()].MoveTo(campParents[i]);
			}

			//Test
			globalDataSystem.gameModel.pkMode = curPKMode - 1;
		}


	}
}
