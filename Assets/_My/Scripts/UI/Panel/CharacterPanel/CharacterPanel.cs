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
using XFABManager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Slap.UI
{
	public partial class CharacterPanel : BasePanel
	{
		public List<Transform> list_WeaponParent { get; set; } = new List<Transform>();
		public override void OnEnter(params object[] param)
		{
			base.OnEnter(param);
		}

		public Transform GetWeaponParent(int camp, int index)
		{
			return list_WeaponParent[camp].GetChild(index);
		}
	
	}
}
 