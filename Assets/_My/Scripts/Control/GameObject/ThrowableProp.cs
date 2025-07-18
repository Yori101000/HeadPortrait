///=====================================================
/// - FileName:      ThrowableProp.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/7/17 11:37:51
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using UnityEngine;
using System;
using XFABManager;
namespace Slap
{
    public class ThrowableProp : MonoBehaviour
    {
        private PlayerData.CampType _campType;

        [SerializeField] private int _damage;

        [Header("追踪设置")]
        [SerializeField] private float duration = 1.2f;        // 飞行时间
        [SerializeField] private float curveHeight = 2f;       // 弧度高度（正为向上，负为向下）
        public bool reverseCurve = false;    // 是否反向抛物线（即向下弯）

        private Vector3 startPos;
        private Vector3 controlPoint;
        private float timer = 0f;

        private Camp _targetCamp;
        
        void Update()
        {
            Trace();
        }

        public void Init(Camp curCamp, Camp targetCamp)
        {

            _targetCamp = targetCamp;

            //处理坐标
            transform.position = curCamp.transform.position;
            //TODO 更换开始点位
            startPos = transform.position;

            _campType = curCamp.campType;


            //TODO 优化道具曲线
            // 控制点：决定轨迹的弯曲方向
            Vector3 midpoint = (startPos + targetCamp.transform.position) / 2f;
            Vector3 curveDir = reverseCurve ? Vector3.down : Vector3.up;

            controlPoint = midpoint + curveDir * curveHeight;
            if (targetCamp.transform.position.y > startPos.y)
                reverseCurve = true;
            else
                reverseCurve = false;
        }

        private void Trace()
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            // 二阶贝塞尔插值
            Vector3 pos = Mathf.Pow(1 - t, 2) * startPos +
                        2 * (1 - t) * t * controlPoint +
                        Mathf.Pow(t, 2) * _targetCamp.transform.position;

            transform.position = pos;

            // 计算当前方向朝向
            Vector3 tangent = (2 * (1 - t) * (controlPoint - startPos) +
                            2 * t * (_targetCamp.transform.position - controlPoint)).normalized;

            if (tangent != Vector3.zero)
            {
                float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            if (t >= 1f)
            {
                timer = 0;
                _targetCamp.ReduceHealth(_damage);
                GameObjectLoader.UnLoad(this.gameObject); // 击中目标
            }
        }
    }
}
