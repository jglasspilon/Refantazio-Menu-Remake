public class ItemExecutor
{
    public void Use(InventoryEntry item, Character[] targets)
    {
        if (item.Item is not UsableItem usable)
            return;

        item.ApplyAmount(-1);
        foreach (Character target in targets)
        {
            ApplyEffects(target, usable.Effects);
        }
    }

    public void Use(InventoryEntry item, Character target)
    {
        if (item.Item is not UsableItem usable)
            return;

        item.ApplyAmount(-1);
        ApplyEffects(target, usable.Effects);
    }

    private void ApplyEffects(Character target, ItemEffect[] effects)
    {
        foreach (ItemEffect effect in effects)
        {
            effect.Apply(target);
        }
    }
}

