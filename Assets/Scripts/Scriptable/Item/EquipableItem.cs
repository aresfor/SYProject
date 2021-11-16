#if UNITY_EDITOR

using Sirenix.OdinInspector;
public class EquipableItem:Item
{
    public override ItemType[] SupportedItemTypes
    {
        get
        {
            return new ItemType[]
            {
                ItemType.LongGun,
                ItemType.ShortGun,
            };
        }
    }
}

#endif
