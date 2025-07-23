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
using System.Collections;

namespace Slap
{
    public interface IGlobalDataSystem : IProjectInitSystem 
    { }
    [Registration(typeof(Slap.Push), typeof(IGlobalDataSystem))]
    public class GlobalDataSystem : AbstractSystem, IGlobalDataSystem
    {
        #region 数据 (因为不需要存档所以直接放到系统中也是可以的)

        public PlayersModel playersModel { get; private set; }
        public CampModel campModel { get; private set; }
        public GameModel gameModel { get; private set; }

        #endregion

        //事件

        public Action<PlayerData.CampType> OnCampChanged;
        public Action OnLeftRoundWin;//搞到接口上
        public Action OnRightRoundWin;
        public Action OnLeftWin;
        public Action OnRightWin;

        public Action<PlayerData.CampType> OnCampDeath;


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

            OnCampChanged += UpdateData;


            OnLeftWin += () => DispenseWinPoint(1);

            OnRightWin += () => DispenseWinPoint(2);



            //测试用，加载一些头像
            icons = Resources.LoadAll<Sprite>("Arts/UI/头像");
            badges = Resources.LoadAll<Sprite>("Arts/UI/称号");
            iconFrames = Resources.LoadAll<Sprite>("Arts/UI/头像框");

        }
        public void End()
        {

            OnCampChanged -= UpdateData;

            OnLeftWin -= () => DispenseWinPoint(1);

            OnLeftWin -= () => DispenseWinPoint(2);


        }
        public void Update()
        {


        }

        #region 更新数据

        private void UpdateData(PlayerData.CampType camp)
        {
            campModel.Dic_Camp[camp.ToString()].UpdatePlayerUI();

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

        public IEnumerator AddScoreCor(PlayerData playerData, GiftScoreData propData, int number = 1)
        {
            int timer = 0;
            int additions = 0;

            while (timer < propData.duration)
            {
                playerData.userScore += propData.baseScore * number;
                additions += propData.baseScore * number;

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

            SetRandomInitPlayer(playerData);

            playersModel.Dic_AllPlayerData.Add(playerData.userName, playerData);
            return true;
        }
        //分配阵营
        public bool AllotPlayerToCamp(PlayerData playerData, PlayerData.CampType toCamp)
        {
            if (!playersModel.Dic_AllRealCampPlayerData.ContainsKey(toCamp) || campModel.Dic_Camp[toCamp.ToString()].hasDead == true)
            {
                Debug.Log($"当前阵营 {toCamp} 不存在，跳过");
                return false;
            }
            if (playerData.userCamp != PlayerData.CampType.None)
            {
                Debug.LogWarning($"玩家 {playerData.userName} 已经在 {playerData.userCamp} 阵营，无法重新分配");
                return false;
            }
            else
            {

                playerData.userCamp = toCamp;
                playersModel.Dic_AllRealCampPlayerData[toCamp].Add(playerData.userName, playerData);
                AllotWinPoint(toCamp);


                OnCampChanged?.Invoke(playerData.userCamp);
                Debug.Log($"玩家 {playerData.userName} 被分配到 {toCamp} 阵营");

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
            if (!playersModel.Dic_AllRealCampPlayerData.ContainsKey(attackCamp) || campModel.Dic_Camp[attackCamp.ToString()].hasDead == true)
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
                campModel.Dic_Camp[curPlayerData.userCamp.ToString()].aimCamp = attackCamp;
                Debug.Log($"{curPlayerData.userCamp} {curPlayerData.userName} 将 {attackCamp} 设置为攻击目标");
                return true;
            }

        }

        public void HandleDeathCamp(PlayerData.CampType campType)
        {
            //清理PlayerSModel中数据
            playersModel.ClearDicPlayerData(campType, true);

            //清理CampModel中数据
            campModel.List_RealCamp.Remove(campModel.Dic_Camp[campType.ToString()]);
            campModel.Dic_Camp.Remove(campType.ToString());

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
        private Sprite[] badges;
        private Sprite[] iconFrames;

        public void SetRandomInitPlayer(PlayerData playerData)
        {
            if (icons != null && icons.Length > 0)
            {
                int index = UnityEngine.Random.Range(0, icons.Length);
                playerData.icon = icons[index];
            }
            if (badges != null && badges.Length > 0)
            {
                int index = UnityEngine.Random.Range(0, badges.Length);
                playerData.badge = badges[index];
            }
            if (iconFrames != null && iconFrames.Length > 0)
            {
                int index = UnityEngine.Random.Range(0, iconFrames.Length);
                playerData.iconFrame = iconFrames[index];
            }
        }
        

        #endregion

    }


}
