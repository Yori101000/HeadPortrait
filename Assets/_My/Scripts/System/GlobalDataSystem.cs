///=====================================================
/// - FileName:      PlayerDataProcessingSystem.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/6/30 13:38:31
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
namespace Slap
{
    [Registration(typeof(Slap.Push))]
    public class GlobalDataSystem : AbstractSystem
    {
        #region 数据 (因为不需要存档所以直接放到系统中也是可以的)

        public PlayersModel playersModel { get; private set; }
        public CampModel campModel { get; private set; }
        public GameModel gameModel { get; private set; }

        #endregion

        //事件
        private Action OnLeftScoreChanged;
        private Action OnRightScoreChanged;
        private Action<PlayerData.CampType> _onCampScoreChanged;
        public Action OnLeftRoundWin;
        public Action OnRightRoundWin;
        public Action OnLeftWin;
        public Action OnRightWin;


        private List<PlayerData> list_leftPlayerData = new List<PlayerData>();
        private List<PlayerData> list_rightPlayerData = new List<PlayerData>();




        public override void Init()
        {
            playersModel = this.GetModel<PlayersModel>();
            campModel = this.GetModel<CampModel>();
            gameModel = this.GetModel<GameModel>();
        }

        //开始系统的逻辑更新
        public void Start()
        {
            playersModel.InitPK(campModel.campCount);

            OnLeftScoreChanged += () => UpdateData(1);


            OnLeftWin += () => DispenseWinPoint(1);

            OnRightScoreChanged += () => UpdateData(2);


            OnRightWin += () => DispenseWinPoint(2);




#if UNITY_EDITOR

            //测试用，加载一些头像
            icons = Resources.LoadAll<Sprite>("Arts/UI/头像");
#endif

        }
        public void End()
        {

            OnLeftScoreChanged -= () => UpdateData(1);

            OnLeftWin -= () => DispenseWinPoint(1);


            OnRightScoreChanged -= () => UpdateData(2);

            OnLeftWin -= () => DispenseWinPoint(2);


        }
        public void Update()
        {


        }



        #region 更新数据

        private void UpdateData(int camp)
        {
            // if (camp == 1)
            // {
            //     list_leftPlayerData = playersModel.Dic_LeftPlayerData.OrderByDescending(pair => pair.Value.userScore)
            //         .Select(pair => pair.Value).ToList();
            //     Debug.Log($"更新左侧玩家数据，当前数量: {list_leftPlayerData.Count}");
            // }
            // else if (camp == 2)
            // {
            //     list_rightPlayerData = playersModel.Dic_RightPlayerData.OrderByDescending(pair => pair.Value.userScore)
            //         .Select(pair => pair.Value).ToList();
            //     Debug.Log($"更新右侧玩家数据，当前数量: {list_rightPlayerData.Count}");
            // }

        }




        #endregion

        //增加指定玩家的分数
        public void AddPlayerScore(string userName, int score)
        {
            if (playersModel.Dic_AllPlayerData.TryGetValue(userName, out PlayerData playerData))
            {
                playerData.userScore += score;

                Debug.Log($"玩家 {userName} 的分数增加了 {score}，当前分数为 {playerData.userScore}");
            }
            else
            {
                Debug.LogWarning($"玩家 {userName} 不存在，无法增加分数");
            }
        }

        public IEnumerator AddScoreCor(PlayerData playerData, GiftScoreData propData)
        {
            int timer = 0;
            int additions = 0;

            while (timer < propData.duration)
            {
                playerData.userScore += propData.baseScore;
                additions += propData.baseScore;

                yield return new WaitForSeconds(1f);
                timer++;
            }

            playerData.userScore -= additions;
        }


        //数据处理
        public void InitRoundData()
        {

        }




        //创建玩家数据
        public bool CreatePlayerData(PlayerData playerData)
        {
            if (playersModel.Dic_AllPlayerData.ContainsKey(playerData.userName))
            {
                Debug.LogWarning($"玩家 {playerData.userName} 已经存在，无法创建重复的玩家数据。");
                return false;
            }
#if UNITY_EDITOR
            SetRandomIcon(playerData);
#endif
            playersModel.Dic_AllPlayerData.Add(playerData.userName, playerData);
            return true;
        }
        //分配阵营
        public bool AllotPlayerToCamp(PlayerData playerData, PlayerData.CampType toCamp)
        {
            if (!playersModel.Dic_AllRealCampPlayerData.ContainsKey(toCamp) || campModel.dic_Camp[toCamp.ToString()].hasDead == true)
            {
                Debug.Log($"当前阵营 {toCamp} 不存在，跳过");
                return false;
            }

            if (playersModel.Dic_AllRealCampPlayerData[toCamp].TryGetValue(playerData.userName, out PlayerData existingInCampPlayerData))
            {
                Debug.Log($"玩家 {existingInCampPlayerData.userName} 已在 {existingInCampPlayerData.userCamp} 阵营");
                return false;
            }
            else
            {

                playerData.userCamp = toCamp;
                playersModel.Dic_AllRealCampPlayerData[toCamp].Add(playerData.userName, playerData);
                AllotWinPoint(toCamp);
                _onCampScoreChanged?.Invoke(playerData.userCamp);

                return true;
            }

            //分配胜点方法
            void AllotWinPoint(PlayerData.CampType toCamp)
            {
                // var winPoint = playerData.userWinPoint;
                // if (toCamp == 1)
                //     campModel.leftCamp.winPoint += winPoint;
                // else
                //     campModel.rightCamp.winPoint += winPoint;

                // playerData.userWinPoint -= winPoint;
            }

        }

        public bool ChangeAttackCamp(PlayerData curPlayerData, PlayerData.CampType attackCamp)
        {
            if (!playersModel.Dic_AllRealCampPlayerData.ContainsKey(attackCamp) || campModel.dic_Camp[attackCamp.ToString()].hasDead == true)
            {
                Debug.Log($"当前阵营 {attackCamp} 不存在，跳过");
                return false;
            }

            if (curPlayerData.userCamp == attackCamp)
            {
                Debug.Log($"您不可以攻击自己");
                return false;
            }

            if (!playersModel.List_CampBoss.Contains(curPlayerData))
            {
                Debug.Log($"{curPlayerData.userName} 您无权控制阵营的攻击目标");
                return false;
            }
            else
            {
                campModel.dic_Camp[curPlayerData.userCamp.ToString()].aimCamp = attackCamp;
                Debug.Log($"{curPlayerData.userCamp} {curPlayerData.userName} 将 {attackCamp} 设置为攻击目标");
                return true;
            }

        }

        //胜利后分发胜点
        //TODO 待补充
        private void DispenseWinPoint(int camp)
        {
            if (camp == 1)
            {
                // List<PlayerData> playerDatas =
                //     playersModel.Dic_LeftPlayerData.OrderByDescending(data => data.Value.userScore).Take(3).Select(data => data.Value).ToList();

            }

        }

        public PlayerData GetPlayerData(string userName)
        {
            if (playersModel.Dic_AllPlayerData.TryGetValue(userName, out PlayerData playerData))
            {
                return playerData;
            }
            Debug.LogWarning($"玩家 {userName} 不存在");
            return null;
        }


        #region Test

        private Sprite[] icons;

        public void SetRandomIcon(PlayerData playerData)
        {
            if (icons != null && icons.Length > 0)
            {
                int index = UnityEngine.Random.Range(0, icons.Length);
                playerData.icon = icons[index];
            }
            else
            {
                Debug.LogWarning("未找到任何图标资源！");
            }
        }

        #endregion

    }


}
