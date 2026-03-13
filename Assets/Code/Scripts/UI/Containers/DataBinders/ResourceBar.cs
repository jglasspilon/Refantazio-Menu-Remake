using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour, IBindableToProperty
{
    [SerializeField] private EBindableResource m_resourceType;
    [SerializeField] private Slider m_valueSlider;
    [SerializeField] private AnimationCurveAsset m_sliderAnimCurve;
    [SerializeField] private UIEffect[] m_healEffect;
    [SerializeField] private UIEffect[] m_damageEffect;
    [SerializeField] private LoggingProfile m_logProfile;

    private Resource m_resource;

    public Type ProviderType => typeof(Character); 

    private enum EBindableResource
    {
        Hp,
        Mp,
        Exp,
    }

    public void BindToProperty(IPropertyProvider provider)
    {
        if(provider is not Character character)
        {
            Logger.LogError($"Received unrecognized provider. ResourceBar only accepts Character as provider.", m_logProfile);
            return;
        }

        m_resource = GetResourceFromCharacter(m_resourceType, character);
        m_resource.OnResourceChange += Display;
        DisplayInstant(m_resource.Current, m_resource.CurrentProportion);
    }

    public void UnBind()
    {
        if (m_resource != null)
        {
            m_resource.OnResourceChange -= Display;
            m_resource = null;
        }       
    }

    private void DisplayInstant(int current, float proportion)
    {
        m_valueSlider.value = proportion;
    }

    private void Display(Resource resource, int delta)
    {    
        LerpSlider(resource.CurrentProportion);      

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

    private Resource GetResourceFromCharacter(EBindableResource resourceType, Character character)
    {
        switch (resourceType)
        {
            case EBindableResource.Hp: return character.HP;
            case EBindableResource.Mp: return character.MP;
            case EBindableResource.Exp: return character.Exp;
        }

        return null;
    }
}
