///=====================================================
/// - FileName:      ExplainPanel.cs
/// - NameSpace:     Slap.UI
/// - Description:   框架自定BasePanel
/// - Creation Time: 2025/6/26 17:44:23
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
	public partial class ExplainPanel : BasePanel
	{
		public void OnClickClose(UnityAction action) => Close.AddListenerPure(action);

	}
}
