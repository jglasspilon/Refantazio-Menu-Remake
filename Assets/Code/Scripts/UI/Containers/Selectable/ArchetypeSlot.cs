using System;
using UnityEngine;

public class ArchetypeSlot : SelectableSlot
{
    public Archetype Archetype => SlotContent as Archetype;
}
