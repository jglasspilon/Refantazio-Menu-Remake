using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;

public class ItemsMenuPage : MenuPage, IItemSelectable, ICharacterSelectable
{
    [SerializeField]
    private ItemSelectionSection m_itemSelectionSection;

    [SerializeField]
    private CharacterSelectionSection m_characterSelectionSection;

    private InventoryEntry m_selectedItem;
    private ItemExecutor m_itemExecutor = new ItemExecutor();

    private void Awake()
    {
        m_itemSelectionSection.OnItemSelected += SelectItem;
        m_characterSelectionSection.OnCharacterSelected += SelectCharacter;
    }

    private void OnDestroy()
    {
        m_itemSelectionSection.OnItemSelected -= SelectItem;
        m_characterSelectionSection.OnCharacterSelected -= SelectCharacter;
    }

    public override UniTask EnterDefaultSection()
    {
        EnterSection(m_itemSelectionSection);
        return default;
    }

    public void SelectItem(InventoryEntry item)
    {
        if (item.Item is not UsableItem usable || usable.BattleOnly)
            return;

        m_selectedItem = item;

        if(usable.TargetingType == ETargetingTypes.AllAllies || usable.TargetingType == ETargetingTypes.All)
        {
            m_characterSelectionSection.SelectAll();
        }

        m_characterSelectionSection.UpdateSelectabilityOfContent(banner => usable.Effects.Any(effect => effect.CanApply(banner.Character)));
        EnterSection(m_characterSelectionSection);
    }

    public void SelectCharacter(Character character)
    {
        if (!CanUseItem(m_selectedItem, character))
            return;

        Logger.Log($"Using {m_selectedItem.Item.Name} on {character.Name}", m_logProfile);        
        UseItem(m_selectedItem, character);

        if (m_selectedItem.Count == 0)
        {
            m_itemSelectionSection.RemoveCurrentSelection();
            TryExitCurrentSection();
            return;
        }
    }

    private void UseItem(InventoryEntry item, Character character)
    {
        if (item.Item is not UsableItem usable)
            return;

        if (usable.TargetingType == ETargetingTypes.SingleAlly)
            m_itemExecutor.Use(item, character);
        else
            m_itemExecutor.Use(item, ObjectResolver.Instance.Resolve<PartyData>().GetAllPartyMembers());

        m_characterSelectionSection.UpdateSelectabilityOfContent(banner => usable.Effects.Any(effect => effect.CanApply(banner.Character)));
    }

    private bool CanUseItem(InventoryEntry item, Character character)
    {
        if(item.Item is not UsableItem usable)
        {
            return false;
        }

        if (usable.TargetingType == ETargetingTypes.SingleAlly)
        {
            return usable.Effects.Any(x => x.CanApply(character));
        }
        else
        {
            return ObjectResolver.Instance.Resolve<PartyData>().GetAllPartyMembers().Any(c => usable.Effects.Any(x => x.CanApply(c)));
        }
    }
}
