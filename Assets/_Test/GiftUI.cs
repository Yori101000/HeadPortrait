using Slap.UI;
using UnityEngine;
using UnityEngine.UI;
using YukiFrameWork;

namespace Slap.Test
{
    public class GiftUI : MonoBehaviour, IController
    {
        //面板显示
        public Button gift => GetComponent<Button>();
        public GameObject giftItemPrefab;

        //基础信息
        public GiftsConfig config;

        //系统
        private GiftSystem giftSystem;
        private GlobalDataSystem globalDataSystem;


        void Start()
        {
            giftSystem = this.GetSystem<GiftSystem>();
            globalDataSystem = this.GetSystem<GlobalDataSystem>();

            gift.onClick.AddListener(() =>
            {
                giftItemPrefab.SetActive(true);
                Button[] buttons = giftItemPrefab.GetComponentsInChildren<Button>();
                buttons[0].AddListenerPure(OnClickBtn0);
                buttons[1].AddListenerPure(OnClickBtn1);
                buttons[2].AddListenerPure(OnClickBtn2);
                buttons[3].AddListenerPure(OnClickBtn3);
                buttons[4].AddListenerPure(OnClickBtn4);
                buttons[5].AddListenerPure(OnClickBtn5);
                buttons[6].AddListenerPure(OnClickBtn6);
            });

        }

        #region 按钮事件

        private void OnClickBtn0()
        {
            if (!CheckPlayer()) return;

            Debug.Log($"{gift.name} + 按钮1被点击了");
            giftItemPrefab.SetActive(false);

            giftSystem.HandleGift(TestManager.Instance.curPlayer, config, 0);

        }

        private void OnClickBtn1()
        {
            if (!CheckPlayer()) return;

            Debug.Log($"{gift.name} + 按钮2被点击了");
        giftItemPrefab.SetActive(false);

            giftSystem.HandleGift(TestManager.Instance.curPlayer, config, 1);

        }
        private void OnClickBtn2()
        {
            if (!CheckPlayer()) return;

            Debug.Log($"{gift.name} + 按钮3被点击了");
            giftItemPrefab.SetActive(false);

            giftSystem.HandleGift(TestManager.Instance.curPlayer, config, 2);

        }
        private void OnClickBtn3()
        {
            if (!CheckPlayer()) return;

            Debug.Log($"{gift.name} + 按钮4被点击了");
            giftItemPrefab.SetActive(false);

            giftSystem.HandleGift(TestManager.Instance.curPlayer, config, 3);

        }
        private void OnClickBtn4()
        {
             if (!CheckPlayer()) return;

            Debug.Log($"{gift.name} + 按钮5被点击了");
            giftItemPrefab.SetActive(false);

            giftSystem.HandleGift(TestManager.Instance.curPlayer, config, 4);

        }
        private void OnClickBtn5()
        {
            if (!CheckPlayer()) return;

            Debug.Log($"{gift.name} + 按钮6被点击了");
            giftItemPrefab.SetActive(false);

            giftSystem.HandleGift(TestManager.Instance.curPlayer, config, 5);
        }
        private void OnClickBtn6()
        {
            if (!CheckPlayer()) return;

            Debug.Log($"{gift.name} + 按钮7被点击了");
            giftItemPrefab.SetActive(false);

            giftSystem.HandleGift(TestManager.Instance.curPlayer, config, 6);
        }
        #endregion

        private bool CheckPlayer()
        {
            if (TestManager.Instance.curPlayer.userName == string.Empty)
            {
                Debug.LogWarning("当前玩家为空");
                return false;
            }
            if (TestManager.Instance.curPlayer.userCamp == PlayerData.CampType.None)
                //TODO 待更改
                globalDataSystem.AllotPlayerToCamp(TestManager.Instance.curPlayer, (PlayerData.CampType)Random.Range(0, 2));
            return true; 
        }


        public IArchitecture GetArchitecture()
        {
            return Push.Global;
        }

    }

}
