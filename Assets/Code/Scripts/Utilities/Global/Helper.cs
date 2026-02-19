using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Helper
{
    public class Interaction
    {
        public static class PointerEventFactory
        {
            public static PointerEventData FromRaycastHit(EventSystem eventSystem, Camera cam, RaycastHit hit, BaseRaycaster raycaster)
            {
                var data = new PointerEventData(eventSystem);

                // Convert world hit point → screen position
                data.position = cam.WorldToScreenPoint(hit.point);

                // Build a RaycastResult that mimics a UI hit
                data.pointerCurrentRaycast = new RaycastResult
                {
                    worldPosition = hit.point,
                    worldNormal = hit.normal,
                    screenPosition = data.position,
                    module = raycaster,
                    gameObject = hit.collider.gameObject,
                    distance = hit.distance
                };

                return data;
            }
        }
    }

    public class Animation
    {
        public static async UniTask WaitForCurrentPageAnimationToEnd(Animator anim)
        {
            await UniTask.NextFrame();
            await Timing.DelaySeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        }        
    }

    public class Timing
    {
        public static async UniTask DelaySeconds(float seconds)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(seconds), ignoreTimeScale: false);
        }
    }

    public class StringFormatting
    {
        public static string FormatIntForUI(int value, int minSize, bool lowStrength)
        {
            int insertAlphaAt = 0;
            string lowOpacity = lowStrength ? "<alpha=#11>" : "<alpha=#66>";
            string normalOpacity = "<alpha=#FF>";
            string format = "";

            for(int i = 0; i < minSize; i++)
            {
                format += "0";
            }

            string valueParse = value.ToString(format);

            if (value < 10)
                insertAlphaAt = minSize - 1;
            else if (value < 100)
                insertAlphaAt = minSize - 2;

            if (insertAlphaAt > 0)
            {
                return $"{lowOpacity}{valueParse.Insert(insertAlphaAt, normalOpacity)}";
            }

            return valueParse;
        }
    }
}
