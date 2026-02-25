using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Helper
{
    public static class Interaction
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

    public static class Animation
    {
        public static async UniTask WaitForCurrentPageAnimationToEnd(Animator anim)
        {
            await UniTask.NextFrame();
            await Timing.DelaySeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        }        
    }

    public static class Timing
    {
        public static async UniTask DelaySeconds(float seconds)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(seconds), ignoreTimeScale: false);
        }
    }

    public static class StringFormatting
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

        public static string PrettifyStat(StatType stat)
        {
            switch(stat)
            {
                case StatType.Level: return "LV";
                case StatType.Strength: return "STR";
                case StatType.Magic: return "MAG";
                case StatType.Endurance: return "END";
                case StatType.Agility: return "AGI";
                case StatType.Attack: return "ATK";
                case StatType.Defence: return "DEF";
                case StatType.Evasion: return "EVA";
                default: return stat.ToString();
            }
        }
    }

    public static class Arrays
    {
        public static int GetSafeIndex(int index, int max, bool loop)
        {
            if (loop)
            {
                if (index < 0)
                    return index = max;
                else if (index > max)
                    return index = 0;
                return index;
            }

            return Mathf.Clamp(index, 0, max);
        }
    }
}
