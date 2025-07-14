///=====================================================
/// - FileName:      PropParent.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/7/10 17:15:24
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using UnityEngine;
using System;
using YukiFrameWork.QF;
namespace Slap
{
    public class WeaponParent : MonoBehaviour
    {

        void Start()
        {
            GameObject player = transform.parent.gameObject;
            RectTransform rect = player.GetComponent<RectTransform>();
            float radius = rect.rect.width / 2f;

            // 获取父物体中心的本地坐标
            Vector2 center = rect.localPosition;

            // 角度分布：更集中在0°和180°
            float[] tArr = { 0f, 0.1f, 0.9f, 0.4f, 0.5f, 0.6f, }; // 0和1分别对应0°和180°
            float startAngle = 0f;   // 0°
            float endAngle = 360f;   // 360°

            for (int i = 0; i < 6; i++)
            {
                float t = tArr[i];
                float angle = Mathf.Lerp(startAngle, endAngle, t);
                float rad = angle * Mathf.Deg2Rad;
                float x = Mathf.Cos(rad) * radius;
                float y = Mathf.Sin(rad) * radius;

                Vector2 localPos = center + new Vector2(x, y);

                GameObject go = new GameObject("WeaponUIObj_" + (i + 1), typeof(RectTransform));
                RectTransform goRect = go.GetComponent<RectTransform>();
                goRect.SetParent(transform, false);
                goRect.localPosition = localPos;
                goRect.sizeDelta = Vector2.zero;
            }   

        }


    }
}
