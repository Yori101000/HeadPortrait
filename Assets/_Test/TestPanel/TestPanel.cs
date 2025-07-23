///=====================================================
/// - FileName:      TestPanel.cs
/// - NameSpace:     Slap.UI
/// - Description:   框架自定BasePanel
/// - Creation Time: 2025/6/27 16:48:17
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork.UI;
using YukiFrameWork;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
namespace Slap.UI
{
	public partial class TestPanel : BasePanel
	{
		public void CampGameOver()
		{
			Btn_Tips.AddListenerPure(() => { });
			var tipsText = Tips.GetComponentInChildren<TextMeshProUGUI>();
			tipsText.text = "游戏结束";
			Tips.SetActive(true);
		}
		public void TipsShow(string tips)
		{
			var tipsText = Tips.GetComponentInChildren<TextMeshProUGUI>();
			tipsText.text = tips;
			Tips.SetActive(true);

			Btn_Tips.AddListenerPure(() => Tips.SetActive(false));
		}
	}
}
