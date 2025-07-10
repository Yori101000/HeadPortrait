///=====================================================
/// - FileName:      Main.cs
/// - NameSpace:     Slap
/// - Description:   高级定制脚本生成
/// - Creation Time: 2025/6/24 11:36:54
/// -  (C) Copyright 2008 - 2025
/// -  All Rights Reserved.
///=====================================================
using Unity.Burst.CompilerServices;
using UnityEngine;
using XFABManager;
using YukiFrameWork;
using YukiFrameWork.Machine;
namespace Slap
{
    public class Main : MonoBehaviour
    {
        //确保不会重复初始化
        private bool isInitFinish = false;
        private RuntimeStateMachineCore core;
        async void Awake()
        {
            //将摄像机保持持久化，保持Canvas的相机一直为该相机
            DontDestroyOnLoad(Camera.main.gameObject);

            if (!isInitFinish)
            {
                //等待框架初始化
                await Push.StartUp();

                core = Resources.Load<RuntimeStateMachineCore>(ConstModel.DefaultStateMachineCorePath);

                // core = AssetBundleManager.LoadAsset<RuntimeStateMachineCore>(Push.ProjectName, ConstModel.DefaultStateMachineCoreName);
                StateManager.StartMachine(ConstModel.DefaultStateMachineCoreName, core, typeof(Push));
                isInitFinish = true;
            }
        }


    }
    public enum GameState
    {
        MainMenu = 0,
        WaitStart,
        Gaming,
        
        End
    }
}
