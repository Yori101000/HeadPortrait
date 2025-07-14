///=====================================================
/// - FileName:      PopPanel.cs
/// - NameSpace:     Slap.UI
/// - Description:   框架自定BasePanel
/// - Creation Time: 2025/6/27 13:27:41
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork.UI;
using UnityEngine;
using YukiFrameWork;
using UnityEngine.UI;
using TMPro;
namespace Slap.UI
{
	public partial class PopPanel : BasePanel
	{
		//弹出弹幕
		public void PopWindow(PlayerData playerData, GiftPopData giftPopData)
		{
	
			Image playerIcon;
			Image giftIcon;
			TextMeshProUGUI counter;
			Animator animator;

			if ((int)playerData.userCamp == 1)
			{
				PopedLeftWindow.SetActive(true);
				playerIcon = PopedLeftWindow.gameObject.Find("玩家头像").GetComponent<Image>();
				giftIcon = PopedLeftWindow.gameObject.Find("礼物").GetComponent<Image>();
				counter = PopedLeftWindow.gameObject.Find("数量").GetComponent<TextMeshProUGUI>();
				animator = PopedLeftWindow.gameObject.GetComponent<Animator>();
				animator.Play("PopLeftWindow");
			}
			else
			{
				PopedRightWindow.SetActive(true);
				playerIcon = PopedRightWindow.gameObject.Find("玩家头像").GetComponent<Image>();
				giftIcon = PopedRightWindow.gameObject.Find("礼物").GetComponent<Image>();
				counter = PopedRightWindow.gameObject.Find("数量").GetComponent<TextMeshProUGUI>();
				animator = PopedRightWindow.gameObject.GetComponent<Animator>();
				animator.Play("PopRightWindow");
			}
			playerIcon.sprite = playerData.icon;
			giftIcon.sprite = giftPopData.giftIcon;
			counter.text = $"×{giftPopData.number}";

		}
	}
	public enum PopType
	{
		Default
	}
}
