
using UnityEngine;
using YukiFrameWork;
namespace Slap
{
    public class AnimationEvent : MonoBehaviour, IController
    {
        private GiftSystem giftSystem;
        void Start()
        {
            giftSystem = this.GetSystem<GiftSystem>();
        }
        private void StopLeftPopWindow()
        {
            giftSystem.StopPopWindow(1);
            this.gameObject.SetActive(false);
        }
        private void StopRightPopWindow()
        {
            giftSystem.StopPopWindow(2);
            this.gameObject.SetActive(false);
        }
        private void StopLeftGiftAnimation()
        {
            giftSystem.StopGiftAnimation(1);
            this.gameObject.SetActive(false);

        }
        private void StopRightGiftAnimation()
        {
            giftSystem.StopGiftAnimation(2);
            this.gameObject.SetActive(false);
        }
        private void StopFullScreenGiftAnimation()
        {
            giftSystem.StopFullScreenGiftAnimation();
            this.gameObject.SetActive(false);
        }

        public IArchitecture GetArchitecture()
        {
            return Push.Global;
        }
    }
}
