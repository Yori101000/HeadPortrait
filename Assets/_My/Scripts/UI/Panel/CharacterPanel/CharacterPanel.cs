///=====================================================
/// - FileName:      CharacterPanel.cs
/// - NameSpace:     Slap.UI
/// - Description:   框架自定BasePanel
/// - Creation Time: 2025/6/26 14:44:15
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork.UI;
using UnityEngine;
using YukiFrameWork;
using UnityEngine.UI;
namespace Slap.UI
{
	public partial class CharacterPanel : BasePanel
	{
		public override void OnInit()
		{
			base.OnInit();
		}
		public override void OnEnter(params object[] param)
		{
			base.OnEnter(param);
		}
		public override void OnPause()
		{
			base.OnPause();
			LogKit.I("CharacterPanel OnPause");
		}
		public override void OnResume()
		{
			base.OnResume();
		}
		public override void OnExit()
		{
			base.OnExit();
		}

	}
}
