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
using System.Linq;
using Slap.UI;
using YukiFrameWork.UI;
using System.Collections;
using Unity.VisualScripting;
namespace Slap
{
    [Registration(typeof(Slap.Push))]
    public class GlobalDataSystem : AbstractSystem
    {
        #region 数据 (因为不需要存档所以直接放到系统中也是可以的)
        private PlayersModel playersModel;
        private CampModel campModel;

        //积分相关
        private int pointPool => campModel.leftCamp.point + campModel.rightCamp.point; 
        private float pointBarMaxValue = 1000;  //TODO 等待指定修改


        //充能相关
        //最多每秒推进进度（因为使用slider左减右加，所以需要 / 2）
        private float maxPushPerSecond = 1f / 2;
		//最大气势差
		private int MaxMomentumDiff = 1000;

        #endregion

        //事件
        private Action OnLeftScoreChanged;
        private Action OnRightScoreChanged;
        public Action OnLeftRoundWin;
        public Action OnRightRoundWin;
        public Action OnLeftWin;
        public Action OnRightWin;


        private List<PlayerData> list_leftPlayerData = new List<PlayerData>();
        private List<PlayerData> list_rightPlayerData = new List<PlayerData>();
        private GameUIPanel gameUIPanel;




        public override void Init()
        {
            playersModel = this.GetModel<PlayersModel>();
            campModel = this.GetModel<CampModel>();

        }

        //开始系统的逻辑更新
        public void Start()
        {
            //得到对应的UI面板
            gameUIPanel = UIKit.GetPanel<GameUIPanel>();

            OnLeftScoreChanged += () => UpdateData(1);
            OnLeftScoreChanged += () => gameUIPanel.UpdateLeftViewerUI(list_leftPlayerData);
            OnLeftScoreChanged += () => UpdateLeftMomentum(list_leftPlayerData);
            OnLeftScoreChanged += () => UpdateLeftPointBar();
            OnLeftScoreChanged += () => CheckLeftPointBar();

            OnLeftWin += () => DispenseWinPoint(1);

            OnRightScoreChanged += () => UpdateData(2);
            OnRightScoreChanged += () => gameUIPanel.UpdateRightViewerUI(list_rightPlayerData);
            OnRightScoreChanged += () => UpdateRightMomentum(list_rightPlayerData);
            OnRightScoreChanged += () => UpdateRightPointBar();
            OnRightScoreChanged += () => CheckRightPointBar();

            OnRightWin += () => DispenseWinPoint(2);



            gameUIPanel.UpdateLeftHealthUI(campModel.leftCamp.health);
            gameUIPanel.UpdateRightHealthUI(campModel.rightCamp.health);
            
#if UNITY_EDITOR

            //测试用，加载一些头像
            icons = Resources.LoadAll<Sprite>("Arts/UI/头像");
#endif

        }
        public void End()
        {

            OnLeftScoreChanged -= () => UpdateData(1);
            OnLeftScoreChanged -= () => gameUIPanel.UpdateLeftViewerUI(list_leftPlayerData);
            OnLeftScoreChanged -= () => UpdateLeftMomentum(list_leftPlayerData);
            OnLeftScoreChanged -= () => UpdateLeftPointBar();
            OnLeftScoreChanged -= () => CheckLeftPointBar();

            OnLeftWin -= () => DispenseWinPoint(1);


            OnRightScoreChanged -= () => UpdateData(2);
            OnRightScoreChanged -= () => gameUIPanel.UpdateRightViewerUI(list_rightPlayerData);
            OnRightScoreChanged -= () => UpdateRightMomentum(list_rightPlayerData);
            OnRightScoreChanged -= () => UpdateRightPointBar();
            OnRightScoreChanged -= () => CheckRightPointBar();

            OnLeftWin -= () => DispenseWinPoint(2);
                

        }
        public void Update()
        {
            UpdateCharge();
            CheckCharge();
            gameUIPanel.UpdatePointPoolUI(pointPool);
            gameUIPanel.UpdateWinPointUI(campModel.leftCamp.winPoint, campModel.rightCamp.winPoint);
        }

        #region 更新数据

        private void UpdateData(int camp)
        {
            if (camp == 1)
            {
                list_leftPlayerData = playersModel.Dic_LeftPlayerData.OrderByDescending(pair => pair.Value.userScore)
                    .Select(pair => pair.Value).ToList();
                Debug.Log($"更新左侧玩家数据，当前数量: {list_leftPlayerData.Count}");
            }
            else if (camp == 2)
            {
                list_rightPlayerData = playersModel.Dic_RightPlayerData.OrderByDescending(pair => pair.Value.userScore)
                    .Select(pair => pair.Value).ToList();
                Debug.Log($"更新右侧玩家数据，当前数量: {list_rightPlayerData.Count}");
            }

        }
        private void UpdateLeftMomentum(List<PlayerData> list_PlayerData)
        {
            var totalMomentum = 0;
            foreach (var playerData in list_PlayerData)
                totalMomentum += playerData.userScore;
            campModel.leftCamp.momentum = totalMomentum;
            gameUIPanel.UpdateLeftMomentumUI(totalMomentum);
        }
        private void UpdateRightMomentum(List<PlayerData> list_PlayerData)
        {
            var totalMomentum = 0;
			foreach (var playerData in list_PlayerData)
				totalMomentum += playerData.userScore;
            campModel.rightCamp.momentum = totalMomentum;
            gameUIPanel.UpdateRightMomentumUI(totalMomentum);
        }
        private void UpdateCharge()
        {
            float momentumDiff = campModel.leftCamp.momentum - campModel.rightCamp.momentum;
            float diffRatio = Mathf.Clamp(momentumDiff / MaxMomentumDiff, -1f, 1f);
            float pushSpeed = diffRatio * maxPushPerSecond;
            gameUIPanel.UpdateChargeUI(pushSpeed);
        }
        private void UpdateLeftPointBar() => gameUIPanel.UpdateLeftPointBarUI(campModel.leftCamp.point / pointBarMaxValue);
        private void UpdateRightPointBar() => gameUIPanel.UpdateRightPointBarUI(campModel.rightCamp.point / pointBarMaxValue);
        private void UpdateScore(PlayerData playerData)
        {
            if (playerData.userCamp == 1)
            {
                OnLeftScoreChanged?.Invoke();
            }
            else if (playerData.userCamp == 2)
            {
                OnRightScoreChanged?.Invoke();
            }
        }

        //更新两侧点赞玩家的数据
        public void UpdateLeftViewerData(PlayerData playerData)
        {
            playersModel.Que_LeftViewer.Enqueue(playerData);
            if (playersModel.Que_LeftViewer.Count > 16)
                playersModel.Que_LeftViewer.Dequeue();
        }
        
        public void UpdateRightViewerData(PlayerData playerData)
        {
            playersModel.Que_RightViewer.Enqueue(playerData);
            if (playersModel.Que_RightViewer.Count > 16)
                playersModel.Que_LeftViewer.Dequeue();
        }
        

        #endregion

        #region 检测 
        private void CheckCharge()
        {
            var value = gameUIPanel.GetChargeProcess();
            //左方胜利
            if (value == 1)
            {

                if (campModel.rightCamp.health == 1)
                    OnLeftWin.Invoke();
                else if (campModel.rightCamp.health > 1)
                    OnLeftRoundWin.Invoke();
            }
            //右方胜利
            else if (value == 0)
            {

                if (campModel.leftCamp.health == 1)
                    OnRightWin.Invoke();
                else if (campModel.leftCamp.health > 1)
                    OnRightRoundWin.Invoke();
            }
        }
        private void CheckLeftPointBar()
        {
            float progress = campModel.leftCamp.point / pointBarMaxValue;
            //为积分条添加事件
            // 例如 
            /*
                if (progress == 1 && ！hasTriggredLeftFull)
                {
                    该bool在回合重新开始时重置
                    hasTriggredLeftFull = true;
                    OnLeftPointBarFull.Invoke();
                }
            */

         
        }
        private void CheckRightPointBar()
        {
            
        }
        #endregion


        //增加指定玩家的分数
        public void AddPlayerScore(string userName, int score)
        {
            if (playersModel.Dic_AllPlayerData.TryGetValue(userName, out PlayerData playerData))
            {
                playerData.userScore += score;
                UpdateScore(playerData);

                Debug.Log($"玩家 {userName} 的分数增加了 {score}，当前分数为 {playerData.userScore}");
            }
            else
            {
                Debug.LogWarning($"玩家 {userName} 不存在，无法增加分数");
            }
        }
       
        public IEnumerator AddScoreCor(PlayerData playerData, EffectData effectData)
        {
            int timer = 0;
            int additions = 0;

            //增加积分池
            //TODO 更改积分池规则
            if (playerData.userCamp == 1)
                campModel.leftCamp.point += effectData.point;
            else
                campModel.rightCamp.point += effectData.point;


            while (timer < effectData.duration)
            {
                playerData.userScore += effectData.baseScore;
                additions += effectData.baseScore;

                UpdateScore(playerData);

                yield return new WaitForSeconds(1f);
                timer++;
            }

            playerData.userScore -= additions;
            UpdateScore(playerData);
        }


        //数据处理
        public void ReduceHealth(int camp)
        {
            if (camp == 1)
            {
                campModel.leftCamp.health--;
                gameUIPanel.UpdateLeftHealthUI(campModel.leftCamp.health);
            }
            else
            {
                campModel.rightCamp.health--;
                gameUIPanel.UpdateRightHealthUI(campModel.rightCamp.health);
            }
        }
        public void InitRoundData()
        {
            campModel.leftCamp.InitRound();
            campModel.rightCamp.InitRound();
            playersModel.InitRound();
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
        public bool AllotPlayerToCamp(PlayerData playerData, int toCamp)
        {
            if (playersModel.Dic_AllPlayerData.TryGetValue(playerData.userName, out PlayerData existingPlayerData))
            {
                //如果玩家数据的阵营为0，则设置为当前分配的阵营
                if (playerData.userCamp == 0)
                {
                    playerData.userCamp = toCamp;

                    //根据阵营更新到对应的字典中
                    if (playerData.userCamp == 1)
                    {
                        playersModel.Dic_LeftPlayerData.Add(playerData.userName, playerData);

                        //分配后进行胜点分配
                        AllotWinPoint(toCamp);

                        OnLeftScoreChanged?.Invoke();
                    }
                    else if (playerData.userCamp == 2)
                    {
                        playersModel.Dic_RightPlayerData.Add(playerData.userName, playerData);
                        AllotWinPoint(toCamp);
                        OnRightScoreChanged?.Invoke();
                    }
                    Debug.Log($"玩家 {playerData.userName} 第一次被分配");
                    return true;
                }
                else
                {
                    Debug.Log($"玩家 {playerData.userName} 已在 {playerData.userCamp} 阵营");

                    return false;
                }
            }
            return false;

            //分配胜点方法
            void AllotWinPoint(int toCamp)
            {
                var winPoint = playerData.userWinPoint;
                if (toCamp == 1)
                    campModel.leftCamp.winPoint += winPoint;
                else
                    campModel.rightCamp.winPoint += winPoint;

                playerData.userWinPoint -= winPoint;
            }
            
        }

        //胜利后分发胜点
        //TODO 待补充
        private void DispenseWinPoint(int camp)
        {
            if (camp == 1)
            {
                List<PlayerData> playerDatas =
                    playersModel.Dic_LeftPlayerData.OrderByDescending(data => data.Value.userScore).Take(3).Select(data => data.Value).ToList();

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
