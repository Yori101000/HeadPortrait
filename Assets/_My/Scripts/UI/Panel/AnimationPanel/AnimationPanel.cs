///=====================================================
/// - FileName:      AnimationPanel.cs
/// - NameSpace:     Slap.UI
/// - Description:   框架自定BasePanel
/// - Creation Time: 2025/6/27 13:22:12
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using UnityEngine;
using YukiFrameWork.UI;
namespace Slap.UI
{
	public partial class AnimationPanel : BasePanel
	{
		
		public void PlayAnimation(PlayerData playerData, GiftAnimationData giftAnimationData)
		{
			
			if (giftAnimationData.type == GiftAnimationData.AnimationType.windowed)
			{
				if ((int)playerData.userCamp == 1)
				{
					LeftAnimation.gameObject.SetActive(true);
					LeftAnimation.Play(giftAnimationData.AnimationName);
				}
				else
				{
					RightAnimation.gameObject.SetActive(true);
					RightAnimation.Play(giftAnimationData.AnimationName);
				}
			}
			else
			{
				FullScreenAnimation.Play(giftAnimationData.AnimationName);
			}
		}

	}
}
