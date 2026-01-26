using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(TMP_Text))]
public class TextMeshGlyphAnimator : MonoBehaviour
{
    [SerializeField] 
    private GlyphAnimationDriver[] m_glyphDrivers;

    [SerializeField]
    private float m_curveTimer;

    TMP_Text m_text;
    private float m_previousTimer;
    public bool IsDriversValid { get { return m_glyphDrivers.Length >= m_text.textInfo.characterCount; } }

    private void Reset()
    {
        ValidateDrivers();
    }

    private void OnEnable()
    {
        m_text = GetComponent<TMP_Text>();
        m_text.OnPreRenderText += OnPreRenderText;

        if (!Application.isPlaying)
            m_text.ForceMeshUpdate();
    }

    private void OnDisable()
    {
        if (m_text != null)
            m_text.OnPreRenderText -= OnPreRenderText;
    }

    public void ValidateDrivers()
    {
        if (!Application.isPlaying)
        {
            if (m_text == null)
                m_text = GetComponent<TMP_Text>();

            int charCount = m_text.textInfo.characterCount;
            m_glyphDrivers = ValidateTransformArrayWithTextSize(m_glyphDrivers, charCount, GlyphAnimationDriver.CreateDefault());
        }
    }

    private T[] ValidateTransformArrayWithTextSize<T>(T[] collection, int count, T defaultValue = default)
    {
        if (collection != null && collection.Length > count)
        {
            return collection.Take(count).ToArray();
        }

        T[] result = collection;

        if (collection == null)
        {
            result = new T[count];
        }
        
        else if(collection.Length < count)
        {
            T[] newArray = new T[count];
            Array.Copy(collection, newArray, collection.Length);
            result = newArray;
        }

        //Apply the custom default if required
        if(!EqualityComparer<T>.Default.Equals(defaultValue, default))
        {
            for(int i = 0; i < result.Length; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(result[i], default))
                    continue;

                result [i] = defaultValue;
            }
        }
        return result;
    }

    private void Update()
    {
        if (m_text == null)
            m_text = GetComponent<TMP_Text>();

        if (m_curveTimer != m_previousTimer)
        {
            m_text.ForceMeshUpdate();
            m_previousTimer = m_curveTimer;
        }
    }

    private void OnPreRenderText(TMP_TextInfo textInfo)
    {
        if (m_glyphDrivers == null)
            return;

        TMP_MeshInfo[] meshInfo = textInfo.meshInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible) 
                continue;

            ApplyCharacterTransformations(meshInfo, charInfo, m_glyphDrivers[i]);            
        }
    }

    private void ApplyCharacterTransformations(TMP_MeshInfo[] meshInfo, TMP_CharacterInfo charInfo, GlyphAnimationDriver driver)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, driver.rotationCurve.Evaluate(m_curveTimer));

        Vector3 offset = default; 
        offset.x = driver.offsetXCurve.Evaluate(m_curveTimer);
        offset.y = driver.offsetYCurve.Evaluate(m_curveTimer);
               
        Vector3 scale = default;
        scale.x = driver.scaleXCurve.Evaluate(m_curveTimer);
        scale.y = driver.scaleYCurve.Evaluate(m_curveTimer);
        scale.z = 1;
        int vIndex = charInfo.vertexIndex;
        int mIndex = charInfo.materialReferenceIndex;

        Vector3[] verts = meshInfo[mIndex].vertices;
        Vector3 mid = (verts[vIndex] + verts[vIndex + 2]) * 0.5f;

        for (int j = 0; j < 4; j++)
        {
            Vector3 relative = verts[vIndex + j] - mid;
            relative = Vector3.Scale(relative, scale);
            relative = rotation * relative;
            verts[vIndex + j] = relative + mid;
            verts[vIndex + j] += offset;
        }
    }
}

[Serializable]
public struct GlyphAnimationDriver
{
    public AnimationCurve offsetXCurve;
    public AnimationCurve offsetYCurve;
    public AnimationCurve scaleXCurve;
    public AnimationCurve scaleYCurve;
    public AnimationCurve rotationCurve;

    public static GlyphAnimationDriver CreateDefault()
    {
        return new GlyphAnimationDriver
        {
            offsetXCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0f)),
            offsetYCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0f)),
            scaleXCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f)),
            scaleYCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f)),
            rotationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0f))
        };
    }
}



