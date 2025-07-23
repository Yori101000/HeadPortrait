///=====================================================
/// - FileName:      Camp.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/7/14 15:14:34
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using YukiFrameWork;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace Slap
{
    public class Camp : MonoBehaviour, IController
    {
        //常量
        //动画名
        private const string AnimationName_BeHurt = "BeHurt";
        //物体名
        private const string ObjName_LeftPropPoint = "LeftThrowPropPoint";
        private const string ObjName_RightPropPoint = "RightThrowPropPoint";

        private GlobalDataSystem globalDataSystem;

        #region UI

        private Image _badge;                       //徽章
        private TextMeshProUGUI _userNameText;      //用户名
        private Image _userIcon;                    //头像
        private Image _iconFrame;                   //头像框
        private TextMeshProUGUI _joinCurCampHint;   //加入当前阵营提示
        private TextMeshProUGUI _healthText;        //阵营血量文本
        private GameObject _shieldObj;              //阵营护盾对象
        private TextMeshProUGUI _shieldText;        //阵营护盾文本
        private GameObject _playerParent;            //玩家父物体
        private Image[] _players; //玩家显示

        #endregion

        #region 状态
        [Header("状态")]
        public int health;
        public int Shield { get; set; } = 0; //护盾值
        public bool hasDead { get; set; } = false;
        public int point { get; set; } = 0;      //当前阵容积分
        public int winPoint { get; set; } = 0;  //当前阵容胜点
        public int maxWeapon { get; private set; } = 6;
        public PlayerData.CampType aimCamp = PlayerData.CampType.None;

        public List<GameObject> list_Weapon { get; set; }
        public PlayerData.CampType campType { get; private set; } = PlayerData.CampType.None;

        [Header("放大设置")]
        private Vector3 originalScale;  //当前实际的基础大小
        [Header("移动设置")]
        [SerializeField] private float moveDuration = 1.5f;
        private const float radianHight = 1;
        [Header("呼吸效果设置")]
        [SerializeField] private float maxScaleMultiplier = 1.05f;
        [SerializeField] private float breathSpeed = 2f;

        #endregion

        private PlayerData _bossPlayerData;

        public Transform LeftPropPoint { get; set; }
        public Transform RightPropPoint { get; set; }

        private Animator _animator;

        private Coroutine _curMoveCoroutine;

        public void Init(PlayerData.CampType _campType)
        {
            if (_campType == PlayerData.CampType.None)
            {
                return;
            }

            //UI初始化
            _badge = FindDeepChild(transform, "徽章")?.GetComponent<Image>();
            _userNameText = FindDeepChild(transform, "UserName")?.GetComponent<TextMeshProUGUI>();
            _userIcon = FindDeepChild(transform, "头像")?.GetComponent<Image>();
            _iconFrame = FindDeepChild(transform, "头像框")?.GetComponent<Image>();
            _joinCurCampHint = FindDeepChild(transform, "阵容选择")?.GetComponent<TextMeshProUGUI>();
            _healthText = FindDeepChild(transform, "血量")?.GetComponentInChildren<TextMeshProUGUI>();
            _shieldObj = FindDeepChild(transform, "护盾")?.gameObject;
            _shieldText = _shieldObj?.GetComponentInChildren<TextMeshProUGUI>();
            _playerParent = FindDeepChild(transform, "Players")?.gameObject;
            _players = _playerParent.GetComponentsInChildren<Image>();

            _badge.gameObject.SetActive(false); //默认不显示徽章
            _shieldObj.SetActive(false); //默认不显示护盾文本

            foreach (var player in _players)
            {
                player.gameObject.SetActive(false); //默认隐藏玩家头像
            }
            //初始化阵容选择名称
            _joinCurCampHint.text = $"加{(int)_campType + 1}";

            //状态初始化
            //初始化武器列表
            list_Weapon = new List<GameObject>();
            for (int i = 0; i < maxWeapon; i++)
            {
                list_Weapon.Add(null);
            }
            campType = _campType;
            originalScale = transform.localScale;


            UpdateHealthUI();
            UpdatePlayerUI();

        }
        void Awake()
        {
            _animator = GetComponent<Animator>();

            LeftPropPoint = transform.Find(ObjName_LeftPropPoint)?.transform;
            RightPropPoint = transform.Find(ObjName_RightPropPoint)?.transform;

            globalDataSystem = this.GetSystem<IGlobalDataSystem>() as GlobalDataSystem;

            globalDataSystem.OnCampDeath += Death;
        }

        void Update()
        {
            Breath();
        }

        #region UI相关
        /// <summary>
        /// 查找子物体
        /// 通过递归方式查找子物体，返回第一个匹配的
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private Transform FindDeepChild(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name == name)
                    return child;
                var result = FindDeepChild(child, name);
                if (result != null)
                    return result;
            }
            return null;
        }


        //玩家数据更新时调用
        public void UpdatePlayerUI()
        {
            
            //顶部玩家得分大于零，则更新Top玩家UI
            List<PlayerData> bottomPlayers =
                globalDataSystem.playersModel.Dic_AllRealCampPlayerData[campType].Select(p => p.Value).ToList();

            _bossPlayerData = bottomPlayers.OrderByDescending(p => p.userScore).FirstOrDefault();

            if (_bossPlayerData != null && _bossPlayerData.userScore > 0)
            {
                UpdateTopPlayerUI(_bossPlayerData);
                //获得除顶部第一个的其他元素
                bottomPlayers = globalDataSystem.playersModel.Dic_AllRealCampPlayerData[campType]
                    .Where(p => p.Value != _bossPlayerData)
                    .Select(p => p.Value)
                    .ToList();
            }

            UpdateBottomPlayerUI(bottomPlayers);

        }
        private void UpdateBottomPlayerUI(List<PlayerData> playerDatas)
        {
            for (int i = 0; i < _players.Length; i++)
            {
                if (i < playerDatas.Count)
                {
                    //如果有玩家数据，则更新UI
                    var playerData = playerDatas[i];
                    _players[i].gameObject.SetActive(true);
                    _players[i].sprite = playerData.icon;
                }
                else
                {
                    //没有玩家数据，则隐藏UI
                    _players[i].gameObject.SetActive(false);
                }
            }
        }
        private void UpdateTopPlayerUI(PlayerData playerData)
        {
            //更新头像
            _userIcon.sprite = playerData.icon;
            //更新用户名
            _userNameText.text = playerData.userName;
            //更新头像框
            _iconFrame.sprite = playerData.iconFrame;

            if (playerData.badge != null)
            {
                _badge.gameObject.SetActive(true);
                _badge.sprite = playerData.badge;
            }
            else
                _badge.gameObject.SetActive(false);
        }

        public void UpdateHealthUI()
        {
            if (Shield > 0)
            {
                //更新护盾UI
                _shieldObj.SetActive(true);
                _shieldText.text = HandleNumber(Shield);   
            }
            else
                _shieldObj.SetActive(false);


            //更新血量UI
            _healthText.text = HandleNumber(health);

            string HandleNumber(int num)
            {
                return (num / 10000f).ToString("0.00") + "万";
            }
            
        }

        #endregion

        /// <summary>
        /// 呼吸效果
        /// 通过正弦函数实现物体的缩放效果，模拟呼吸
        /// </summary>
        public void Breath()
        {
            // t: 0 ~ 1
            float t = (Mathf.Sin(Time.time * breathSpeed) + 1f) / 2f;

            // 缩放倍数：1.0 ~ maxScaleMultiplier
            float scale = Mathf.Lerp(1f, maxScaleMultiplier, t);

            transform.localScale = originalScale * scale;
        }


        public void MoveTo(Transform aimTrans, float _radianHight = radianHight)
        {
            if (_curMoveCoroutine != null)
                StopCoroutine(_curMoveCoroutine);
            _curMoveCoroutine = StartCoroutine(MoveAlongCurve(aimTrans, _radianHight));
        }

        private IEnumerator MoveAlongCurve(Transform target, float _radianHight = radianHight)
        {
            //移动和动画会产生冲突，所以在移动过程中不播放动画
            _animator.enabled = false;

            Vector3 start = transform.position;

            // 控制点可以控制弧度的“高度”
            Vector3 control = (start + target.position) / 2 + new Vector3(0, _radianHight, 0);

            float timer = 0f;
            while (timer < moveDuration)
            {
                float t = timer / moveDuration;

                // 二次贝塞尔插值公式
                Vector3 pos = Mathf.Pow(1 - t, 2) * start +
                            2 * (1 - t) * t * control +
                            Mathf.Pow(t, 2) * target.position;

                transform.position = pos;

                timer += Time.deltaTime;
                yield return null;
            }

            transform.position = target.position;
            transform.SetParent(target);

            _animator.enabled = true;
            _curMoveCoroutine = null;
        }

        public void ReduceHealth(int damage, Camp attacker)
        {
            if (_animator.enabled)
                _animator.Play(AnimationName_BeHurt);
            health -= damage;
            Debug.Log(health);

            if (health <= 0 && !hasDead)
            {
                attacker.aimCamp = PlayerData.CampType.None;
                globalDataSystem.OnCampDeath.Invoke(campType);
            }

            UpdateHealthUI();

        }

        private void Death(PlayerData.CampType type)
        {
            if (type != this.campType)
                return;


            hasDead = true;
            gameObject.SetActive(false);
            globalDataSystem.HandleDeathCamp(type);
           
        }
        void OnEnable()
        {
            
        }
        private void OnDisable()
        {
            globalDataSystem.OnCampDeath -= Death;
        }

        public IArchitecture GetArchitecture()
        {
            return Push.Global;
        }
    }
}
