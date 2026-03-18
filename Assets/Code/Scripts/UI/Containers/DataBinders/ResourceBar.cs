using Cysharp.Threading.Tasks;
using System;
using System.Threading;
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
    private CancellationTokenSource cts;

    public Type ProviderType => typeof(Character); 

    private enum EBindableResource
    {
        Hp,
        Mp,
        Exp,
        ArchetypeExp
    }

    public void BindToProperty(IPropertyProvider provider)
    {
        if(provider is Character character)
        {
            m_resource = GetResourceFromCharacter(m_resourceType, character);
            m_resource.OnResourceChange += Display;
            DisplayInstant(m_resource.Current, m_resource.CurrentProportion);
            return;
        }

        if(m_resourceType == EBindableResource.ArchetypeExp && provider is Archetype archetype)
        {
            m_resource = archetype.Rank.Exp;
            m_resource.OnResourceChange += Display;
            DisplayInstant(m_resource.Current, m_resource.CurrentProportion);
            return;
        }

        Logger.LogError($"Received unrecognized provider. ResourceBar only accepts Character as provider.", m_logProfile);
        return;       
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
        cts?.Cancel();
        cts = new CancellationTokenSource();
        Helper.Animation.LerpSlider(m_valueSlider, resource.CurrentProportion, m_sliderAnimCurve.Curve, cts.Token);      

        if (delta == 0)
            return;

        if (delta > 0)
        {
            PlayHealingEffects();
            return;
        }

        PlayDamageEffects();
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
            case EBindableResource.Exp: return character.Level.Exp;
            case EBindableResource.ArchetypeExp: return character.Equipment.Archetype.Rank.Exp;
        }

        return null;
    }
}
