///=====================================================
/// - FileName:      EndState.cs
/// - NameSpace:     Slap
/// - Description:   YUKI 有限状态机构建状态类
/// - Creation Time: 2025/6/26 13:52:43
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork.Machine;
using UnityEngine;
using YukiFrameWork;
namespace Slap
{
	public class EndState : StateBehaviour
	{
		public override void OnEnter()
		{
			base.OnEnter();
			Debug.Log("进入结束状态"); 
        }

	}
}
