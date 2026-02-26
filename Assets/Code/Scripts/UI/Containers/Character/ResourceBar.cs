using Cysharp.Threading.Tasks;
using TMPro;
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
    private UIEffect[] m_healEffect;

    [SerializeField]
    private UIEffect[] m_damageEffect;

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
        if(m_valueText != null)
            m_valueText.text = Helper.StringFormatting.FormatIntForUI(current, 3, false);

        m_valueSlider.value = proportion;
    }

    private void Display(int current, float proportion, int delta)
    {    
        if(m_valueText != null)
            m_valueText.text = Helper.StringFormatting.FormatIntForUI(current, 3, false);

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

    private void PlayHealingEffects()
    {
        foreach (UIEffect effect in m_damageEffect)
            effect.StopEffect();

        foreach (UIEffect effect in m_healEffect)
            effect.PlayEffect();
    }

    private void PlayDamageEffects()
    {
        foreach (UIEffect effect in m_healEffect)
            effect.StopEffect();

        foreach (UIEffect effect in m_damageEffect)
            effect.PlayEffect();
    }
}
