///=====================================================
/// - FileName:      Push.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/6/24 11:18:57
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using YukiFrameWork;
using YukiFrameWork.UI;
using YukiFrameWork.Audio;
using System;
using UnityEngine;
using UnityEditor;
namespace Slap
{
    public class Push : Architecture<Push>
    {

        //可以填写默认进入的场景名称，在架构准备完成后，自动进入
        public override (string, SceneLoadType) DefaultSceneName => default;

        public override string OnProjectName => ConstModel.ProjectName;

        public override void OnInit()
        {
            UIKit.Init(new MyUILoader());
        }
        //配表构建，通过ArchitectureTable可以在架构中缓存部分需要的资源,例如TextAssets ScriptableObject
        protected override ArchitectureTable BuildArchitectureTable() => default;

        //当架构准备完成后触发，当架构加载失败抛出异常则不会执行
        public override void OnCompleted()
        {

        }


    }
    public class MyUILoader : IUIConfigLoader
    {
        public TItem Load<TItem>(string name) where TItem : BasePanel
        {
            //进行固定位置编写
            return Resources.Load<TItem>($"Prefabs/UI/{name}");
        }

        public void LoadAsync<TItem>(string name, Action<TItem> onCompleted) where TItem : BasePanel
        {
            var result = Resources.LoadAsync<TItem>(name);
            result.completed += (op) =>
            {
                if (op.isDone)
                    onCompleted?.Invoke(result.asset as TItem);
            };
        }

        public void UnLoad(BasePanel item)
        {
            Resources.UnloadAsset(item);
        }

    }


}
