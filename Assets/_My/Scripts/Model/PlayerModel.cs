///=====================================================
/// - FileName:      PlayerModel.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/6/27 17:03:16
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Slap
{
    [Registration(typeof(Slap.Push))]
    public class PlayersModel : AbstractModel
    {
        #region 字段

        // 注意   
        // 因为 Dic_AllPlayerData 和 Dic_Left(Right)PlayerData 这两个字典中
        // 存储的是同一个 PlayerData 实例的引用
        // 在使用时对两者其中一个值进行更改，便可以实现两个值都发生更改
        private Dictionary<string, PlayerData> dic_GlobalPlayerData = new Dictionary<string, PlayerData>();
        private Dictionary<string, PlayerData> dic_AllPlayerData = new Dictionary<string, PlayerData>();
        private Dictionary<string, PlayerData> dic_LeftPlayerData = new Dictionary<string, PlayerData>();
        private Dictionary<string, PlayerData> dic_RightPlayerData = new Dictionary<string, PlayerData>();

        private Queue<PlayerData> que_LeftViewer = new Queue<PlayerData>();
        private Queue<PlayerData> que_RightViewer = new Queue<PlayerData>();

        #endregion

        #region 属性
     
        public Dictionary<string, PlayerData> Dic_GlobalPlayerData { get => dic_GlobalPlayerData; set => dic_GlobalPlayerData = value; }
        public Dictionary<string, PlayerData> Dic_AllPlayerData { get => dic_AllPlayerData; set => dic_AllPlayerData = value; }
        public Dictionary<string, PlayerData> Dic_LeftPlayerData { get => dic_LeftPlayerData; set => dic_LeftPlayerData = value; }
        public Dictionary<string, PlayerData> Dic_RightPlayerData { get => dic_RightPlayerData; set => dic_RightPlayerData = value; }
     
        public Queue<PlayerData> Que_LeftViewer { get => que_LeftViewer; set => que_LeftViewer = value; }
        public Queue<PlayerData> Que_RightViewer { get => que_RightViewer; set => que_RightViewer = value; }
     
        #endregion

        public override void Init()
        {
            //TODO 加载局外玩家数据（排行榜使用） 

        }

        public void InitRound()
        {
            // 清空所有玩家的目前分数
            foreach (var playerData in dic_AllPlayerData.Values)
                playerData.userScore = 0;
            que_LeftViewer.Clear();
            que_RightViewer.Clear();
        }

    }

    [Serializable]
    public class PlayerData
    {
        public string userName;
        public Sprite icon; //头像
        public int userScore;
        public CampType userCamp = CampType.None;
        public int userWinPoint;

        public enum CampType
        {
            camp1,
            camp2,
            camp3,
            camp4,
            None,
        }
    }

}
