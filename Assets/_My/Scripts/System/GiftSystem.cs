///=====================================================
/// - FileName:      GiftSystem.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/7/2 11:08:01
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Slap.UI;
using YukiFrameWork.UI;
using XFABManager;
using UnityEditor.Rendering;


namespace Slap
{
    [Registration(typeof(Slap.Push))]
    public class GiftSystem : AbstractSystem
    {
        //常量
        private const string PropParentName = "PropParent";


        #region  弹窗
        //左侧弹窗
        private bool isPlayingLeftPopWindow;
        private Queue<PopData> que_LeftPopWindow;
        private PopData curLeftPopWindow;
        //右侧弹窗
        private bool isPlayingRightPopWindow;
        private Queue<PopData> que_RightPopWindow;
        private PopData curRightPopWindow;
        #endregion

        //道具
        private Transform _propParent;
        private List<GameObject> list_Prop;

        #region 动画
        private bool isPlayingFullScreenGiftAnimation;
        private Queue<PlayGiftAnimationData> que_FullScreenGiftAnimation;
        private PlayGiftAnimationData curFullScreenPlayGiftAnimationData;
        //左侧动画
        private bool isPlayingLeftGiftAnimation;
        private Queue<PlayGiftAnimationData> que_LeftPlayGiftAnimation;
        private PlayGiftAnimationData curLeftPlayGiftAnimationData;
        //右侧动画
        private bool isPlayingRightGiftAnimation;
        private Queue<PlayGiftAnimationData> que_RightPlayGiftAnimation;
        private PlayGiftAnimationData curRightPlayGiftAnimationData;
        #endregion

        //面板
        private PopPanel popPanel;
        private CharacterPanel characterPanel;

        private AnimationPanel animationPanel;

        //系统
        private GlobalDataSystem globalDataSystem;


        public override void Init()
        {
            que_LeftPopWindow = new Queue<PopData>();
            que_RightPopWindow = new Queue<PopData>();

            list_Prop = new List<GameObject>();

            que_FullScreenGiftAnimation = new Queue<PlayGiftAnimationData>();
            que_LeftPlayGiftAnimation = new Queue<PlayGiftAnimationData>();
            que_RightPlayGiftAnimation = new Queue<PlayGiftAnimationData>();


        }

        public void Start()
        {
            MonoHelper.Update_AddListener(Update);

            popPanel = UIKit.GetPanel<PopPanel>();
            characterPanel = UIKit.GetPanel<CharacterPanel>();

            animationPanel = UIKit.GetPanel<AnimationPanel>();
            globalDataSystem = this.GetSystem<GlobalDataSystem>();

            _propParent = characterPanel.Find(PropParentName);
        }
        public void End()
        {
            MonoHelper.Update_RemoveListener(Update);
        }
        public void Update(MonoHelper monoHelper)
        {
            CheckPopWindow();
            CheckGiftAnimation();
        }

        //处理点赞
        public void HandleLike(PlayerData playerData, GiftScoreData scoreData)
        {
            if (playerData.userName == String.Empty)
            {
                Debug.LogWarning("玩家为空");
                return;
            }

            //加分系统
            //将礼物添加到处理携程中
            string timeStamp = DateTime.Now.Ticks.ToString();

            globalDataSystem.AddScoreCor(playerData, scoreData).Start();

            //TODO 剩余点赞处理


        }


        /// <summary>
        /// 处理礼物事件
        /// </summary>
        /// <param name="playerData"></param>
        /// <param name="config"></param>
        /// <param name="number">礼物配置序号（对应配置中不同的组刷礼物）</param>
        public void HandleGift(PlayerData playerData, GiftsConfig config, int number)
        {
            if (playerData.userName == String.Empty)
            {
                Debug.LogWarning("玩家为空");
                return;
            }

            //加分系统
            //将礼物添加到处理携程中
            globalDataSystem?.AddScoreCor(playerData, config.scoreDatas[number]).Start();

            //弹窗
            PopWindow(playerData, config.popDatas[number]);

            //道具实际效果
            SpawnProp(playerData, config.propDatas[number]);

            //动画播放
            PlayGiftAnimation(playerData, config.animtionDatas[number]);


        }

        #region 弹窗设置
        private void PopWindow(PlayerData playerData, GiftPopData giftPopData)
        {
            if (giftPopData.giftIcon == null)
            {
                Debug.Log("本礼物无弹窗效果");
                return;
            }

            var popData = new PopData();
            popData.playerData = playerData;
            popData.giftPopData = giftPopData;

            if ((int)playerData.userCamp == 1)
                que_LeftPopWindow.Enqueue(popData);
            else
                que_RightPopWindow.Enqueue(popData);

        }
        public void StartPopWindow(int camp)
        {
            if (camp == 1)
                isPlayingLeftPopWindow = true;
            else
                isPlayingRightPopWindow = true;
        }
        public void StopPopWindow(int camp)
        {
            if (camp == 1)
            {
                isPlayingLeftPopWindow = false;
                if (que_LeftPopWindow != null && que_LeftPopWindow.Count > 0)
                    que_LeftPopWindow.Dequeue();
            }
            else
            {
                isPlayingRightPopWindow = false;
                if (que_RightPopWindow != null && que_RightPopWindow.Count > 0)
                    que_RightPopWindow.Dequeue();
            }
        }
        private void CheckPopWindow()
        {
            if (que_LeftPopWindow != null && que_LeftPopWindow.Count > 0)
            {
                if (!isPlayingLeftPopWindow && curLeftPopWindow != que_LeftPopWindow.Peek())
                {
                    curLeftPopWindow = que_LeftPopWindow.Peek();
                    popPanel.PopWindow(curLeftPopWindow.playerData, curLeftPopWindow.giftPopData);
                    StartPopWindow((int)curLeftPopWindow.playerData.userCamp);
                }
            }

            if (que_RightPopWindow != null && que_RightPopWindow.Count > 0)
            {
                if (!isPlayingRightPopWindow && curRightPopWindow != que_RightPopWindow.Peek())
                {
                    curRightPopWindow = que_RightPopWindow.Peek();
                    popPanel.PopWindow(curRightPopWindow.playerData, curRightPopWindow.giftPopData);
                    StartPopWindow((int)curRightPopWindow.playerData.userCamp);
                }
            }

        }
        private void ClearPop()
        {
            que_LeftPopWindow.Clear();
            que_RightPopWindow.Clear();
        }

        #endregion

        #region 道具设置

        private void SpawnProp(PlayerData playerData, GiftPropData propData)
        {
            if (propData.propPre == null)
            {
                Debug.Log("该礼物没有具体道具");
                return;
            }
            //进行道具判断
            if (propData.type == GiftPropData.PropType.ThrowableProp)
                MonoHelper.Instance.StartCoroutine(HandleProp(playerData, propData));
            else
                HandleWeapon(playerData, propData);
        }
        // 道具处理
        private IEnumerator HandleProp(PlayerData playerData, GiftPropData propData)
        {
            //找到当前阵营
            var curCamp = globalDataSystem.campModel.dic_camp[playerData.userCamp.ToString()];
            var targetCamp = globalDataSystem.campModel.dic_camp[curCamp.aimCamp.ToString()];

            Transform startTrans = curCamp.LeftPropPoint;

            //生成指定数量的道具
            for (int i = 0; i < propData.propCount; i++)
            {
                var propObj = GameObjectLoader.Load(propData.propPre, _propParent.transform);
                var prop = propObj.GetComponent<ThrowableProp>();
                //初始化道具（之后道具自己管理自己）
                prop.Init(curCamp, targetCamp);

                //更换发射位置
                startTrans = startTrans == curCamp.RightPropPoint ? curCamp.LeftPropPoint : curCamp.RightPropPoint;

                list_Prop.Add(propObj);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void HandleWeapon(PlayerData playerData, GiftPropData propData)
        {
            bool isAddBullet = false;

            var campWeapons = globalDataSystem.campModel.dic_camp[playerData.userCamp.ToString()].list_Weapon;

            //检查当前阵营中是否有放武器的位置
            foreach (var curWeapon in campWeapons)
            {
                //当前阵营武器为空，则跳过
                if (curWeapon == null)
                    continue;
                if (curWeapon.name == propData.propPre.name)
                {

                    isAddBullet = true;
                    curWeapon.GetComponent<Weapon_Base>().bulletCount += propData.propCount;
                    //TODO 更新武器UI子弹数量
                }

            }
            if (!isAddBullet)
                CreateWeapon();

            void CreateWeapon()
            {
                int emptyIndex = campWeapons.FindIndex(w => w == null);
                if (emptyIndex != -1 && emptyIndex < globalDataSystem.campModel.dic_camp[playerData.userCamp.ToString()].maxWeapon)
                {

                    //创建物体，分配位置， 记录数据
                    Transform weaponParent = characterPanel.GetWeaponParent((int)playerData.userCamp, emptyIndex);

                    GameObject prop = GameObjectLoader.Load(propData.propPre, weaponParent);
                    //设置物体
                    prop.name = propData.propPre.name;
                    prop.GetComponent<Weapon_Base>()?.Init(playerData.userCamp);

                    campWeapons[emptyIndex] = prop;
                    list_Prop.Add(prop);

                }
                else
                {
                    Debug.LogError("超出范围");
                }
            }

        }


        private void ClearProp()
        {
            for (int i = 0; i < list_Prop.Count; i++)
            {
                GameObjectLoader.UnLoad(list_Prop[i]);
            }
            list_Prop.Clear();
        }



        #endregion

        #region 动画播放
        private void PlayGiftAnimation(PlayerData playerData, GiftAnimationData giftAnimationData)
        {
            if (giftAnimationData.AnimationName == string.Empty)
            {
                Debug.Log("本礼物无动画效果");
                return;
            }

            var playGiftAnimationData = new PlayGiftAnimationData();
            playGiftAnimationData.playerData = playerData;
            playGiftAnimationData.giftAnimationData = giftAnimationData;

            if (giftAnimationData.type == GiftAnimationData.AnimationType.windowed)
            {
                if ((int)playerData.userCamp == 1)
                    que_LeftPlayGiftAnimation.Enqueue(playGiftAnimationData);
                else
                    que_RightPlayGiftAnimation.Enqueue(playGiftAnimationData);
            }
            else
            {
                que_FullScreenGiftAnimation.Enqueue(playGiftAnimationData);
            }

        }

        private void StartFullGiftAnimtion()
        {
            isPlayingFullScreenGiftAnimation = true;
        }
        public void StopFullScreenGiftAnimation()
        {
            isPlayingFullScreenGiftAnimation = false;
            que_FullScreenGiftAnimation.Dequeue();
        }


        private void StartWindowedGiftAnimation(int camp)
        {
            if (camp == 1)
                isPlayingLeftGiftAnimation = true;
            else
                isPlayingRightGiftAnimation = true;
        }
        public void StopGiftAnimation(int camp)
        {
            if (camp == 1)
            {
                isPlayingLeftPopWindow = false;
                que_LeftPopWindow.Dequeue();
            }
            else
            {
                isPlayingRightPopWindow = false;
                que_RightPopWindow.Dequeue();
            }
        }
        private void CheckGiftAnimation()
        {
            if (que_FullScreenGiftAnimation != null && que_FullScreenGiftAnimation.Count > 0)
            {
                if (!isPlayingFullScreenGiftAnimation && curFullScreenPlayGiftAnimationData != que_LeftPlayGiftAnimation.Peek())
                {
                    curFullScreenPlayGiftAnimationData = que_LeftPlayGiftAnimation.Peek();
                    //播放动画
                    animationPanel.PlayAnimation(curLeftPlayGiftAnimationData.playerData, curLeftPlayGiftAnimationData.giftAnimationData);
                    StartFullGiftAnimtion();
                }

                //播放全屏动画时，其他动画延后播放
                return;
            }

            if (que_LeftPlayGiftAnimation != null && que_LeftPlayGiftAnimation.Count > 0)
            {
                if (!isPlayingLeftGiftAnimation && curLeftPlayGiftAnimationData != que_LeftPlayGiftAnimation.Peek())
                {
                    curLeftPlayGiftAnimationData = que_LeftPlayGiftAnimation.Peek();
                    animationPanel.PlayAnimation(curLeftPlayGiftAnimationData.playerData, curLeftPlayGiftAnimationData.giftAnimationData);
                    StartWindowedGiftAnimation((int)curLeftPopWindow.playerData.userCamp);

                }
            }

            if (que_RightPlayGiftAnimation != null && que_RightPlayGiftAnimation.Count > 0)
            {
                if (!isPlayingRightGiftAnimation && curRightPlayGiftAnimationData != que_RightPlayGiftAnimation.Peek())
                {
                    curRightPlayGiftAnimationData = que_RightPlayGiftAnimation.Peek();
                    animationPanel.PlayAnimation(curRightPlayGiftAnimationData.playerData, curRightPlayGiftAnimationData.giftAnimationData);
                    StartWindowedGiftAnimation((int)curRightPopWindow.playerData.userCamp);

                }
            }

        }
        private void ClearGiftAnimation()
        {
            que_FullScreenGiftAnimation.Clear();
            que_LeftPlayGiftAnimation.Clear();
            que_RightPlayGiftAnimation.Clear();
        }


        #endregion

        public void Clear()
        {
            ClearPop();
            ClearProp();
            ClearGiftAnimation();
        }

    }
    public class PopData
    {
        public PlayerData playerData;
        public GiftPopData giftPopData;
    }
    public class PlayGiftAnimationData
    {
        public PlayerData playerData;
        public GiftAnimationData giftAnimationData;
    }
}
