///=====================================================
/// - FileName:      LoadingPanel.cs
/// - NameSpace:     Slap.UI
/// - Description:   框架自定BasePanel
/// - Creation Time: 2025/6/24 13:27:26
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork.UI;
using UnityEngine;
namespace Slap.UI
{
	public partial class LoadingPanel : BasePanel, IUIAnimation
	{
		private Animator animator => GetComponent<Animator>();
		public BasePanel Panel { get => this; set => value = this; }
		public override void OnEnter(params object[] param)
		{
			base.OnEnter(param);

			animator.Play("FadeIn");

		}
		public override void OnExit()
		{
			base.OnExit();

			animator.Play("FadeOut");
		}

		public bool OnEnterAnimation()
		{

			return IsPlaying("FadeIn");

		}

		public bool OnExitAnimation()
		{
			return IsPlaying("FadeOut");
		}
		private bool IsPlaying(string animationName, int layer = 0)
			=> animator.GetCurrentAnimatorStateInfo(layer).IsName(animationName);
			
    }
}
