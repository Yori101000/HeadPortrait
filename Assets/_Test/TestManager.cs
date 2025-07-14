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
namespace Slap.Test
{
   public class TestManager : MonoBehaviour,IController
    {
        public static TestManager Instance;
        #region  UI组件
        //用户输入
        public TMP_InputField ipf_user;

        //用户设置
        public TMP_InputField ipf_userCreate;
        public TMP_InputField ipf_userWinPoint;
        public TMP_InputField ipf_userSelect;
        #endregion

        GlobalDataSystem globalDataSystem;
        GiftSystem giftSystem;
        public PlayerData curPlayer;

        void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void Start()
        {
            ipf_user.onSubmit.AddListener(OnUserInput);
            ipf_userCreate.onSubmit.AddListener(OnUserCreate);
            ipf_userWinPoint.onSubmit.AddListener(OnUserSetWinPoint);
            ipf_userSelect.onSubmit.AddListener(OnUserSelect);
            globalDataSystem = this.GetSystem<GlobalDataSystem>();
            giftSystem = this.GetSystem<GiftSystem>();
        }


        //TODO 待更改
        private void OnUserInput(string value)
        {
            if (value == "" || curPlayer.userName == "")
            {
                Debug.LogWarning("用户输入不能为空，请先创建或选择玩家");
                return;
            }

            Debug.Log($"用户输入: {value}");

            //检测输入，并进行相应处理
            if (Regex.IsMatch(value, @"^1+$"))
            {
                // 进行分配
                if (globalDataSystem.AllotPlayerToCamp(curPlayer, PlayerData.CampType.camp1))
                    Debug.Log($"玩家 {curPlayer.userName} 分配成功，阵营为红色");
                else
                    Debug.LogWarning($"玩家 {curPlayer.userName} 创建失败");
            }
            if (Regex.IsMatch(value, @"^2+$"))
            {
                // 进行分配
                if (globalDataSystem.AllotPlayerToCamp(curPlayer, PlayerData.CampType.camp2))
                    Debug.Log($"玩家 {curPlayer.userName} 分配成功，阵营为蓝色");
                else
                    Debug.LogWarning($"玩家 {curPlayer.userName} 分配失败");
            }
            if (Regex.IsMatch(value, @"^6+$"))
            {
                if (curPlayer.userCamp == 0)
                    Debug.Log($"玩家 {curPlayer.userName} 阵营为空，请重新分配");
                giftSystem.HandleLike(curPlayer, new GiftScoreData { baseScore = 5, duration = 3f});
            }
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
            }
            else
                Debug.LogWarning($"玩家 {value} 不存在");
            
        }

        public IArchitecture GetArchitecture()
        {
            return Push.Global;
        }
    }

}
