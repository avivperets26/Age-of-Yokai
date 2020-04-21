using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ArmorType { Helmet,Shulders,Chest,Gloves,Feet,MainHand,Offhand,TwoHand}

[CreateAssetMenu(fileName = "Armor", menuName = "Item/Armor", order = 2)]
public class Armor : Item
{
    [SerializeField]
    private ArmorType armorType;

    [SerializeField]
    private int intellect;

    [SerializeField]
    private int strength;

    [SerializeField]
    private int stamina;
 
    public override string GetDescription()
    {
        string stats = string.Empty;

        if (intellect > 0)
        {
            stats += string.Format("\n +{0} intelect", intellect);
        }
        else if (strength > 0)
        {
            stats += string.Format("\n +{0} strength", strength);
        }
        else if (stamina > 0)
        {
            stats += string.Format("\n +{0} stamina", stamina);
        }

        return base.GetDescription() + stats;
    }
}
