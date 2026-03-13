using Cysharp.Threading.Tasks;

public class CharacterBanner_Equipment : CharacterBanner
{
    public override void InitializeFromData(Character character)
    {
        base.InitializeFromData(character);    
        m_bindables.ForEach(x => x.BindToProperty(character));
    }

    public override void ResetForPool()
    {
        m_bindables.ForEach(x => x.UnBind());
        m_character = null;
    }

    private async void OnArchetypeChanged()
    {
        m_bindables.ForEach((x) => x.UnBind());
        await UniTask.Yield();
        m_bindables.ForEach(x => x.BindToProperty(m_character));
    }
}
