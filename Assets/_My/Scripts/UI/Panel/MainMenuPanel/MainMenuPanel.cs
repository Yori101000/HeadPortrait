///=====================================================
/// - FileName:      MainMenuPanel.cs
/// - NameSpace:     Slap.UI
/// - Description:   框架自定BasePanel
/// - Creation Time: 2025/6/27 11:02:18
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork.UI;
using UnityEngine;
using YukiFrameWork;
using UnityEngine.UI;
using UnityEngine.Events;
namespace Slap.UI
{
	public partial class MainMenuPanel : BasePanel
	{
		public void OnClickBtn1(UnityAction action) => Start1.AddListenerPure(action);
		public void OnClickBtn2(UnityAction action) => Start2.AddListenerPure(action);
		public void OnClickBtn3(UnityAction action) => Start3.AddListenerPure(action);
	}
}
