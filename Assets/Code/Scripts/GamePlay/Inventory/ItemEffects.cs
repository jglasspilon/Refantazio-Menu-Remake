using UnityEngine;

public abstract class ItemEffect: ScriptableObject
{
    public abstract void Apply();
}

public class ItemEffect_Heal : ItemEffect
{
    public override void Apply()
    {
        throw new System.NotImplementedException();
    }
}
