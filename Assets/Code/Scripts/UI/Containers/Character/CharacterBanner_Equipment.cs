using Cysharp.Threading.Tasks;

public class CharacterBanner_Equipment : CharacterBanner
{
    public override void InitializeFromData(Character character)
    {
        base.InitializeFromData(character);    
        m_bindables.ForEach(x => x.BindToProperty(character));
        character.Equipment.OnEquipmentChanged += HandleOnArchetypeChanged;
    }

    public override void ResetForPool()
    {
        Character.Equipment.OnEquipmentChanged -= HandleOnArchetypeChanged;
        base.ResetForPool();
    }

    private async void HandleOnArchetypeChanged()
    {
        m_bindables.ForEach((x) => x.UnBind());
        await UniTask.Yield();
        m_bindables.ForEach(x => x.BindToProperty(m_character));
    }
}
