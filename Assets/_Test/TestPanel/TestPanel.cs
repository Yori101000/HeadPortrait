///=====================================================
/// - FileName:      TestPanel.cs
/// - NameSpace:     Slap.UI
/// - Description:   框架自定BasePanel
/// - Creation Time: 2025/6/27 16:48:17
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork.UI;
using UnityEngine;
using YukiFrameWork;
using UnityEngine.UI;
using UnityEditor.Purchasing;
using UnityEngine.Events;
namespace Slap.UI
{
	public partial class TestPanel : BasePanel
	{
		public void OnClickTest(UnityAction action) => Test.GetComponent<Button>().AddListenerPure(action);

	}
}
