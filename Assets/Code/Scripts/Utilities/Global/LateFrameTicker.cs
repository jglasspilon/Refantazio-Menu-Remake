using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

public static class LateFrameTicker
{
    public static event Action OnLateTick;

    [RuntimeInitializeOnLoadMethod]
    private static void Install()
    {
        var loop = PlayerLoop.GetCurrentPlayerLoop();

        InsertSystem<PostLateUpdate>(ref loop, new PlayerLoopSystem
        {
            type = typeof(LateFrameTicker),
            updateDelegate = LateTick
        });

        PlayerLoop.SetPlayerLoop(loop);
    }

    private static void LateTick()
    {
        OnLateTick?.Invoke();
    }

    private static void InsertSystem<T>(ref PlayerLoopSystem loop, PlayerLoopSystem systemToAdd)
    {
        for (int i = 0; i < loop.subSystemList.Length; i++)
        {
            if (loop.subSystemList[i].type == typeof(T))
            {
                var list = loop.subSystemList[i].subSystemList;
                Array.Resize(ref list, list.Length + 1);
                list[list.Length - 1] = systemToAdd;
                loop.subSystemList[i].subSystemList = list;
                return;
            }
        }
    }

}
