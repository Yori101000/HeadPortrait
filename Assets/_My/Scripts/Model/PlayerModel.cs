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
using System.Linq;

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
        private Dictionary<string, PlayerData> dic_AllPlayerData = new Dictionary<string, PlayerData>();        //全部直播间的玩家

        // private Dictionary<string, PlayerData> dic_LeftPlayerData = new Dictionary<string, PlayerData>();
        // private Dictionary<string, PlayerData> dic_RightPlayerData = new Dictionary<string, PlayerData>();

        //全部在阵营中的玩家
        private Dictionary<PlayerData.CampType, Dictionary<string, PlayerData>> _dic_AllRealCampPlayerData
            = new Dictionary<PlayerData.CampType, Dictionary<string, PlayerData>>();

    
        #endregion

        #region 属性

        public Dictionary<string, PlayerData> Dic_GlobalPlayerData { get => dic_GlobalPlayerData; set => dic_GlobalPlayerData = value; }
        public Dictionary<string, PlayerData> Dic_AllPlayerData { get => dic_AllPlayerData; set => dic_AllPlayerData = value; }
        // public Dictionary<string, PlayerData> Dic_LeftPlayerData { get => dic_LeftPlayerData; set => dic_LeftPlayerData = value; }
        // public Dictionary<string, PlayerData> Dic_RightPlayerData { get => dic_RightPlayerData; set => dic_RightPlayerData = value; }
        public Dictionary<PlayerData.CampType, Dictionary<string, PlayerData>> Dic_AllRealCampPlayerData { get => _dic_AllRealCampPlayerData; set => _dic_AllRealCampPlayerData = value; }

        public List<PlayerData> List_CampBoss => Dic_AllRealCampPlayerData.Select
                                         (kv => kv.Value         // kv.Value 是 Dictionary<string, PlayerData>
                                         .OrderByDescending(p => p.Value.userScore)  // 按 score 倒序
                                         .FirstOrDefault().Value)   // 取出最高分的 PlayerData
                                     .Where(p => p != null)         // 防止为空（比如某个阵营没有玩家）
                                     .ToList();

        #endregion

        //全局初始化
        public override void Init()
        {
            //TODO 加载局外玩家数据（排行榜使用） 


        }

        //PK初始化
        public void InitPK(int number)
        {
            //初始化字典
            for (int i = 0; i < number; i++)
            {
                Dic_AllRealCampPlayerData[(PlayerData.CampType)i] = new Dictionary<string, PlayerData>();
            }
        }

        public void ClearDicPlayerData(PlayerData.CampType type)
        {

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
