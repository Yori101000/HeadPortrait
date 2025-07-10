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
		public void OnClickStart(UnityAction action) => Start.AddListenerPure(action);
	}
}
