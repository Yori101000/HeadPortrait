///=====================================================
/// - FileName:      TestManager.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/6/27 18:37:48
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using YukiFrameWork.UI;
using Slap.UI;
namespace Slap.Test
{
    public class TestManager : MonoBehaviour, IController
    {
        public static TestManager Instance;
        #region  UI组件
        //用户输入
        public TMP_InputField ipf_user;

        //用户设置
        public TMP_InputField ipf_userCreate;
        public TMP_InputField ipf_userWinPoint;
        public TMP_InputField ipf_userSelect;
        public TMP_InputField ipf_giftCounter;
        public Button btn_userSend;
        public TMP_Dropdown DD_GiftType;
        public TMP_Dropdown DD_UserCamp;
        public TextMeshProUGUI txt_userName;
        #endregion

        GlobalDataSystem globalDataSystem;
        GiftSystem giftSystem;
        public PlayerData curPlayer;


        // 命令处理映射表
        private Dictionary<Regex, Action<string>> commandHandlers;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void Start()
        {
            ipf_user.onSubmit.AddListener(OnUserInput);
            btn_userSend.onClick.AddListener(() =>
            {
                //是否切换玩家
                if (ipf_userSelect.text != "")
                {
                    OnUserSelect(ipf_userSelect.text);
                    ipf_userSelect.text = "";
                    return;
                }


                //是否创建玩家
                if (ipf_userCreate.text != "")
                {
                    OnUserCreate(ipf_userCreate.text);
                    ipf_userCreate.text = "";
                }
                if(curPlayer == null && ipf_userCreate.text == "")
                {
                    UIKit.GetPanel<TestPanel>().TipsShow("请先创建或选择玩家");
                    return;
                }

                // 提交胜点设置
                if (ipf_userWinPoint.text == "")
                    curPlayer.userWinPoint = 0;
                else
                    OnUserSetWinPoint(ipf_userWinPoint.text);


                //提交阵营
                HandleJoinCamp("", (PlayerData.CampType)DD_UserCamp.value);


                //提交礼物
                //获取礼物数量
                int giftCounter = 0;
                if (ipf_giftCounter.text == "")
                    giftCounter = 1;
                else
                {
                    int.TryParse(ipf_giftCounter.text, out giftCounter);
                    if (giftCounter <= 0)
                    {
                        UIKit.GetPanel<TestPanel>().TipsShow("礼物数量必须大于0");
                        return;
                    }
                    //提交礼物
                    if (!CheckPlayer()) return;
                    giftSystem.HandleGift(curPlayer, DD_GiftType.value, giftCounter);
                }
                
                //当前玩家显示
                txt_userName.text = $"当前控制玩家为 {curPlayer.userName}";

            });

            globalDataSystem = this.GetSystem<IGlobalDataSystem>() as GlobalDataSystem;
            giftSystem = this.GetSystem<IGiftSystem>() as GiftSystem;

            InitCommandHandlers();
        }

        //初始化命令库
        void InitCommandHandlers()
        {
            commandHandlers = new Dictionary<Regex, Action<string>>()
                {
                    { new Regex(@"^6+$"), HandleLikeInput },
                    { new Regex(@"^加1$", RegexOptions.IgnoreCase), value => HandleJoinCamp(value, PlayerData.CampType.camp1) },
                    { new Regex(@"^加2$", RegexOptions.IgnoreCase), value => HandleJoinCamp(value, PlayerData.CampType.camp2) },
                    { new Regex(@"^加3$", RegexOptions.IgnoreCase), value => HandleJoinCamp(value, PlayerData.CampType.camp3) },
                    { new Regex(@"^加4$", RegexOptions.IgnoreCase), value => HandleJoinCamp(value, PlayerData.CampType.camp4) },

                    { new Regex(@"^攻击1$", RegexOptions.IgnoreCase), value => HandleAttack(value, PlayerData.CampType.camp1) },
                    { new Regex(@"^攻击2$", RegexOptions.IgnoreCase), value => HandleAttack(value, PlayerData.CampType.camp2) },
                    { new Regex(@"^攻击3$", RegexOptions.IgnoreCase), value => HandleAttack(value, PlayerData.CampType.camp3) },
                    { new Regex(@"^攻击4$", RegexOptions.IgnoreCase), value => HandleAttack(value, PlayerData.CampType.camp4) },
                };
        }


        private void OnUserInput(string value)
        {
            if (value == "" || curPlayer.userName == "")
            {
                Debug.LogWarning("用户输入不能为空，请先创建或选择玩家");
                return;
            }

            Debug.Log($"用户输入: {value}");


            //检测指令输入
            foreach (var kvp in commandHandlers)
            {
                var regex = kvp.Key;
                var action = kvp.Value;
                if (regex.IsMatch(value))
                {
                    action.Invoke(value);
                    return;
                }
            }
            
        }
        private void HandleLikeInput(string value)
        {
            if (curPlayer.userCamp == 0)
                Debug.Log($"玩家 {curPlayer.userName} 阵营为空，请重新分配");
            giftSystem.HandleLike(curPlayer, new GiftScoreData { baseScore = 5, duration = 3f });
        }
        private void HandleJoinCamp(string value, PlayerData.CampType toCamp)
        {
            // 进行分配
            if (globalDataSystem.AllotPlayerToCamp(curPlayer, toCamp))
                Debug.Log($"玩家 {curPlayer.userName} 分配成功，阵营为{toCamp}");
            else
                Debug.LogWarning($"玩家 {curPlayer.userName} 分配失败");
        }
        private void HandleAttack(string value, PlayerData.CampType targetCamp)
        {
            globalDataSystem.ChangeAttackCamp(curPlayer, targetCamp);
        }

        private void OnUserSetWinPoint(string value)
        {
            if (int.TryParse(value, out int winPoint))
            {
                curPlayer.userWinPoint = winPoint;
                Debug.Log($"用户 {curPlayer.userName} 目前有 {curPlayer.userWinPoint} 胜点");
            }
            else
            {
                Debug.LogWarning("请输入有效的整数作为胜点");
            }
        }

        private void OnUserCreate(string value)
        {
            if (value == "")
                return;

            PlayerData temp = new PlayerData { userName = value, userScore = 0 };

            if (globalDataSystem.CreatePlayerData(temp))
            {
                curPlayer = temp;
                Debug.Log($"玩家 {value} 创建成功");
            }
            else
                Debug.LogWarning($"玩家 {value} 创建失败, 已存在");
        }

        private void OnUserSelect(string value)
        {
            if (value == "")
                return;

            Debug.Log($"用户选择: {value}");

            PlayerData temp = globalDataSystem.GetPlayerData(value);

            if (temp != null)
            {
                Debug.Log($"玩家 {value} 选择成功");
                curPlayer = temp;
                UIKit.GetPanel<TestPanel>().TipsShow($"选择成功,当前控制玩家为 {curPlayer.userName}");
            }
            else
                UIKit.GetPanel<TestPanel>().TipsShow($"选择失败,玩家 {value} 不存在");

        }

        private bool CheckPlayer()
        {
            if (curPlayer.userName == string.Empty)
            {
                Debug.LogWarning("当前玩家为空");
                return false;
            }
            if (curPlayer.userCamp == PlayerData.CampType.None)
                //TODO 待更改
                globalDataSystem.AllotPlayerToCamp(curPlayer, (PlayerData.CampType)UnityEngine.Random.Range(0, 4));
            return true;
        }


        public IArchitecture GetArchitecture()
        {
            return Push.Global;
        }
    }

}
