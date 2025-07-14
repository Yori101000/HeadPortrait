///=====================================================
/// - FileName:      HitEffect.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/7/14 17:57:21
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using UnityEngine;
using System;
using XFABManager;
using Unity.VisualScripting;
namespace Slap
{
    public class HitEffect : MonoBehaviour
    {
        [SerializeField] private float duration;
        private float timer;
        void OnEnable()
        {
            timer = 0;
        }
        void Update()
        {
            timer += Time.deltaTime;
            if (timer > duration)
            {
                UnLoad();
            }
        }
        private void UnLoad()
        {
            GameObjectLoader.UnLoad(this.gameObject);
        }

    }
}
