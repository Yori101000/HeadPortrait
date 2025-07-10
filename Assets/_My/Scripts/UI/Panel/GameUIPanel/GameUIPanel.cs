///=====================================================
/// - FileName:      GameUIPanel.cs
/// - NameSpace:     Slap.UI
/// - Description:   框架自定BasePanel
/// - Creation Time: 2025/6/26 17:01:43
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork.UI;
using UnityEngine;
using YukiFrameWork;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
namespace Slap.UI
{
	public partial class GameUIPanel : BasePanel
	{

		#region  UI组件

		//顶尖观众UI
		private Image[] leftTopViewerIcon;
		private Image[] rightTopViewerIcon;
		private TextMeshProUGUI[] txt_LeftTopViewer;
		private TextMeshProUGUI[] txt_RightTopViewer;

		#endregion



		public override void OnEnter(params object[] param)
		{
			leftTopViewerIcon = Pnt_LeftTapRank.GetComponentsInChildren<Image>();
			txt_LeftTopViewer = Pnt_LeftTapRank.GetComponentsInChildren<TextMeshProUGUI>();

			rightTopViewerIcon = Pnt_RightTapRank.GetComponentsInChildren<Image>();
			txt_RightTopViewer = Pnt_RightTapRank.GetComponentsInChildren<TextMeshProUGUI>();
		}


		//更新左方阵营UI
		public void UpdateLeftViewerUI(List<PlayerData> list_PlayerData)
		{
			//更新顶部观众
			int length = list_PlayerData.Count > leftTopViewerIcon.Length ? leftTopViewerIcon.Length : list_PlayerData.Count;
			for (int i = 0; i < length; i++)
			{
				leftTopViewerIcon[i].sprite = list_PlayerData[i].icon;
				txt_LeftTopViewer[i].text = list_PlayerData[i].userName;
			}


			//TODO 更新观众详情


		}

		//更新点赞观众
		public void UpdateLeftLikingViewer()
		{

		}




		public void UpdateRightViewerUI(List<PlayerData> list_PlayerData)
		{
			//更新顶部观众
			int length = list_PlayerData.Count > rightTopViewerIcon.Length ? rightTopViewerIcon.Length : list_PlayerData.Count;
			for (int i = 0; i < length; i++)
			{
				rightTopViewerIcon[i].sprite = list_PlayerData[i].icon;
				txt_RightTopViewer[i].text = list_PlayerData[i].userName;
			}

		}

		public void UpdateLeftMomentumUI(int totalMomentum) => Txt_LeftMomentum.text = totalMomentum.ToString();
		public void UpdateRightMomentumUI(int totalMomentum) => Txt_RightMomentum.text = totalMomentum.ToString();

		//更新积分池
		public void UpdatePointPoolUI(int point) => Txt_PointPool.text = point.ToString();
		public void UpdateLeftPointBarUI(float value) => Sdr_LeftPointBar.value = value;
		public void UpdateRightPointBarUI(float value) => Sdr_RightPointBar.value = value;
		public void UpdateWinPointUI(int leftWinPool, int rightWinPool)
		{
			Txt_LeftWinPoint.text = leftWinPool.ToString();
			Txt_RightWinPoint.text = rightWinPool.ToString();

		}
		public void UpdateChargeUI(float pushSpeed)
		{
			Sdr_ChargeBar.value += pushSpeed * Time.deltaTime;

			//更新相关文本
			Txt_LeftChargeProgress.text = $"{Sdr_ChargeBar.value * 100:F1}%";
			Txt_RightChargeProgress.text = $"{(1 - Sdr_ChargeBar.value) * 100:F1}%";
		}
		public void UpdateLeftHealthUI(int health) => Txt_LeftHealth.text = health.ToString();
		public void UpdateRightHealthUI(int health) => Txt_RightHealth.text = health.ToString();


		public float GetChargeProcess() => Sdr_ChargeBar.value;
		public float GetLeftPointProcess() => Sdr_LeftPointBar.value;
		public float GetRightPointProcess() => Sdr_RightPointBar.value;

		public void ResetCharge() => Sdr_ChargeBar.value = 0.5f;

	}

}
