using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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

        public static async UniTask ApplyAlphaToGraphicFromCurve(MaskableGraphic graphic, AnimationCurve curve, CancellationToken token)
        {
            float duration = curve.GetDuration();
            float timer = 0f;
            Color cachedColor = graphic.color;

            try
            {
                await UniTask.Yield(PlayerLoopTiming.Update, token);
                while (timer < duration)
                {
                    token.ThrowIfCancellationRequested();
                    Color alphaed = cachedColor;
                    alphaed.a = curve.Evaluate(timer);
                    graphic.color = alphaed;

                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                    timer += Time.deltaTime;
                }

                Color final = cachedColor;
                final.a = curve.Evaluate(curve.keys.Last(x => true).time);
                graphic.color = final;
            }
            catch (OperationCanceledException)
            {
                
            }
        }

        public static async UniTask ApplyAlphaToCanvasGroupFromCurve(CanvasGroup group, AnimationCurve curve, CancellationToken token)
        {
            float duration = curve.GetDuration();
            float timer = 0f;

            try
            {
                while (timer < duration)
                {
                    token.ThrowIfCancellationRequested();
                    group.alpha = curve.Evaluate(timer);

                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                    timer += Time.deltaTime;
                }

                group.alpha = curve.Evaluate(curve.keys.Last(x => true).time);
            }
            catch (OperationCanceledException)
            {
                
            }
        }

        public static async UniTask LerpSlider(Slider slider, float newProportion, AnimationCurve curve, CancellationToken token)
        {
            float oldProportion = slider.value;
            float duration = curve.GetDuration();
            float timer = 0f;

            try
            {
                while (timer < duration)
                {
                    token.ThrowIfCancellationRequested();
                    slider.value = Mathf.Lerp(oldProportion, newProportion, curve.Evaluate(timer));
                    
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                    timer += Time.deltaTime;
                }

                slider.value = newProportion;
            }
            catch (OperationCanceledException)
            {
                
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

    public static class Math
    {
        public static string PercentToHex(float percent)
        {
            percent = Mathf.Clamp01(percent);
            int alpha = Mathf.RoundToInt(percent * 255f);
            return alpha.ToString("X2");
        }
    }

    public static class GameMath
    {
        private static readonly float HP_LEVEL_FACTOR = 0.1f;
        private static readonly float MP_LEVEL_FACTOR = 0.075f;

        public static int GetMaxHpFromEndurance(int baseHp, int endurance, int level)
        {
            return Mathf.FloorToInt(baseHp * (1f + level * HP_LEVEL_FACTOR) * (1f + endurance / 100f));
        }

        public static int GetMapMpFromMagic(int baseMp, int magic, int level)
        {
            return Mathf.FloorToInt(baseMp * (1f + level * MP_LEVEL_FACTOR) * (1f + magic / 100f));
        }
    }

    public static class StringFormatting
    {
        public static string StylizeWords(string input, int sizePercent = 100)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            string size = sizePercent.ToString();
            string[] words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                string w = words[i];
                char first = char.ToUpper(w[0]);
                string rest = w.Substring(1);

                words[i] = $"<size={size}%>{first}</size>{rest}";
            }

            return string.Join(" ", words);
        }


        public static string FormatIntForUI(int value, int minSize, float opacityPercent)
        {
            string lowOpacity = $"<alpha=#{Math.PercentToHex(opacityPercent)}>";
            string normalOpacity = "<alpha=#FF>";

            string format = new string('0', minSize);
            string valueParse = value.ToString(format);

            int firstNonZero = valueParse.TakeWhile(c => c == '0').Count();
            int normalStartIndex = value == 0 ? valueParse.Length - 1 : firstNonZero;

            if (normalStartIndex == 0)
            {
                return $"{normalOpacity}{valueParse}";
            }

            return $"{lowOpacity}{valueParse.Insert(normalStartIndex, normalOpacity)}";
        }

        public static string PrettifyStat(EStatType stat)
        {
            switch (stat)
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

    public static class DataHandling
    {
        public static Dictionary<string, IObservableProperty> BuildPropertyMap(object root)
        {
            var map = new Dictionary<string, IObservableProperty>();
            BuildRecursive(root, "", map);
            return map;
        }

        private static void BuildRecursive(object obj, string parentKey, Dictionary<string, IObservableProperty> map)
        {
            if (obj == null)
                return;

            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (var field in obj.GetType().GetFields(flags))
            {
                var value = field.GetValue(obj);
                var key = string.IsNullOrEmpty(parentKey) ? field.Name : parentKey + "." + field.Name;

                // Direct observable property
                if (value is IObservableProperty observable)
                {
                    map[key] = observable;
                }

                // Sub-property provider (recurse)
                if (value is ISubPropertyProvider provider)
                {
                    foreach (var sub in provider.GetSubProperties(key))
                        map[sub.Key] = sub.Value;
                }
            }
        }

        public static IEnumerable<KeyValuePair<string, IObservableProperty>>GetObservableFields(object instance, string parentKey)
        {
            if (instance == null)
                yield break;

            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (var field in instance.GetType().GetFields(flags))
            {
                var fieldValue = field.GetValue(instance);
                if (fieldValue == null)
                    continue;

                string key = string.IsNullOrEmpty(parentKey) ? field.Name : $"{parentKey}.{field.Name}";

                // Case 1: Direct observable property
                if (fieldValue is IObservableProperty observable)
                {
                    yield return new KeyValuePair<string, IObservableProperty>(key, observable);
                    continue;
                }

                // Case 2: Derived ObservableProperty<T> 
                var baseType = field.FieldType.BaseType;
                if (baseType != null && baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(ObservableProperty<>))
                {
                    if (fieldValue is IObservableProperty derivedObservable)
                        yield return new KeyValuePair<string, IObservableProperty>(key, derivedObservable);

                    continue;
                }

                // Case 3: Nested provider → recurse
                if (fieldValue is ISubPropertyProvider)
                {
                    foreach (var sub in GetObservableFields(fieldValue, key))
                        yield return sub;
                }
            }
        }

        public static IEnumerable<string> GetObservablePropertyNamesFromType(Type type, Type valueTypeFilter)
        {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (var field in type.GetFields(flags))
            {
                //Check for explicit Observable Property
                if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(ObservableProperty<>))
                {
                    var valueType = field.FieldType.GetGenericArguments()[0];

                    if (valueTypeFilter == null || valueType == valueTypeFilter)
                        yield return field.Name;
                }

                //Check for derived Observable Property
                var baseType = field.FieldType.BaseType;
                if (baseType != null && baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(ObservableProperty<>))
                {
                    var valueType = baseType.GetGenericArguments()[0];
                    if (valueTypeFilter == null || valueType == valueTypeFilter)
                        yield return field.Name;
                }

                //Check for sub properties provider recursively
                if (typeof(ISubPropertyProvider).IsAssignableFrom(field.FieldType))
                {
                    foreach (var sub in GetObservablePropertyNamesFromType(field.FieldType, valueTypeFilter))
                        yield return $"{field.Name}.{sub}";
                }
            }
        }

        public static IEnumerable<string> GetObservablePropertyTypesFromProviderType(Type type, Type valueTypeFilter)
        {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (var field in type.GetFields(flags))
            {
                var fieldType = field.FieldType;

                //Check for explicit Observable Property
                if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(ObservableProperty<>))
                {
                    var valueType = fieldType.GetGenericArguments()[0];

                    if (valueTypeFilter == null || valueType == valueTypeFilter)
                        yield return valueType.Name;
                }

                //Check for derived Observable Property
                var baseType = fieldType.BaseType;
                if (baseType != null && baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(ObservableProperty<>))
                {
                    var valueType = baseType.GetGenericArguments()[0];

                    if (valueTypeFilter == null || valueType == valueTypeFilter)
                        yield return valueType.Name;
                }

                //Check for sub properties provider recursively
                if (typeof(ISubPropertyProvider).IsAssignableFrom(fieldType))
                {
                    foreach (var sub in GetObservablePropertyTypesFromProviderType(fieldType, valueTypeFilter))
                        yield return sub;
                }
            }
        }
    }
}
