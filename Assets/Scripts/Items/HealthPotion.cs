using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/Potion",order =1)]//to be automatically listed in the Assets/Create submenu
public class HealthPotion : Item,IUseable
{
    [SerializeField]
    private int health;

    

    public void Use()
    {
        
        if (Hero.MyInstance.MyHealth.MyCurrentValue < Hero.MyInstance.MyHealth.MyMaxValue)
        {
            Remove();

            Hero.MyInstance.GetHealth(health);
        }
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>Use: Restores {0} health</color>", health);
    }
}
