using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        public static async UniTask ApplyAlphaToGraphicFromCurve(MaskableGraphic graphic, Color color, AnimationCurve curve, CancellationToken token)
        {
            float duration = curve.GetDuration();
            float timer = 0f;

            while (timer < duration)
            {
                Color alphaed = color;
                alphaed.a = curve.Evaluate(timer);
                graphic.color = alphaed;

                await UniTask.Yield(PlayerLoopTiming.Update, token).SuppressCancellationThrow();
                timer += Time.deltaTime;
            }
        }

        public static async UniTask ApplyAlphaToCanvasGroupFromCurve(CanvasGroup group, AnimationCurve curve, CancellationToken token)
        {
            float duration = curve.GetDuration();
            float timer = 0f;

            while (timer < duration)
            {
                group.alpha = curve.Evaluate(timer);
                await UniTask.Yield(PlayerLoopTiming.Update, token).SuppressCancellationThrow();
                timer += Time.deltaTime;
            }
        }
    }

    public static class Timing
    {
        public static UniTask DelaySeconds(float seconds, bool ignoreTimeScale = false)
        {
            return UniTask.Delay(TimeSpan.FromSeconds(seconds), ignoreTimeScale);
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

        public static string PrettifyStat(EStatType stat)
        {
            switch(stat)
            {
                case EStatType.Strength: return "STR";
                case EStatType.Magic: return "MAG";
                case EStatType.Endurance: return "END";
                case EStatType.Agility: return "AGI";
                case EStatType.Attack: return "ATK";
                case EStatType.Defence: return "DEF";
                case EStatType.Evasion: return "EVA";
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
