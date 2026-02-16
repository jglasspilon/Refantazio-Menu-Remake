using Cysharp.Threading.Tasks;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_valueText;

    [SerializeField]
    private Slider m_valueSlider;

    [SerializeField]
    private AnimationCurveAsset m_sliderAnimCurve;

    [SerializeField]
    private PartyBannerEffect[] m_healEffect;

    [SerializeField]
    private PartyBannerEffect[] m_damageEffect;

    private Resource m_resource;

    public void Initialize(Resource resource)
    {
        m_resource = resource;
        resource.OnResourceChange += Display;
        DisplayInstant(resource.Current, resource.CurrentProportion);
    }

    public void Unbind()
    {
        m_resource.OnResourceChange -= Display;
        m_resource = null;
    }

    private void DisplayInstant(int current, float proportion)
    {
        m_valueText.text = FormatResourceValue(current);
        m_valueSlider.value = proportion;
    }

    private void Display(int current, float proportion, int delta)
    {       
        m_valueText.text = FormatResourceValue(current);
        LerpSlider(proportion);      

        if (delta == 0)
            return;

        if (delta > 0)
        {
            PlayHealingEffects();
            return;
        }

        PlayDamageEffects();
    }

    private async UniTask LerpSlider(float newProportion)
    {
        float oldProportion = m_valueSlider.value;
        float duration = m_sliderAnimCurve.GetDuration();
        float timer = 0f;

        while (timer < duration)
        {
            m_valueSlider.value = Mathf.Lerp(oldProportion, newProportion, m_sliderAnimCurve.Evaluate(timer));
            await UniTask.Yield(PlayerLoopTiming.Update);
            timer += Time.deltaTime;
        }

        m_valueSlider.value = newProportion;
    }

    private string FormatResourceValue(int current)
    {
        int insertAlphaAt = 0;
        string lowOpacity = "<alpha=#66>";
        string normalOpacity = "<alpha=#FF>";
        string valueParse = current.ToString("000");

        if (current < 10)
            insertAlphaAt = 2;
        else if (current < 100)
            insertAlphaAt = 1;

        if (insertAlphaAt > 0)
        {
            return $"{lowOpacity}{valueParse.Insert(insertAlphaAt, normalOpacity)}";
        }

        return valueParse;
    }

    private void PlayHealingEffects()
    {
        foreach (PartyBannerEffect effect in m_damageEffect)
            effect.StopEffect();

        foreach (PartyBannerEffect effect in m_healEffect)
            effect.PlayEffect();
    }

    private void PlayDamageEffects()
    {
        foreach (PartyBannerEffect effect in m_healEffect)
            effect.StopEffect();

        foreach (PartyBannerEffect effect in m_damageEffect)
            effect.PlayEffect();
    }
}
