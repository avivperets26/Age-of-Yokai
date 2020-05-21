using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    
    [SerializeField]
    private Loot[] loot;

    private List<Item> droppedItems = new List<Item>();

    private bool rolled = false;

    public void ShowLoot()
    {
        if (!rolled)
        {
            RollLoot();
        }
   
        LootWindow.MyInstance.CreatePages(droppedItems);
    }

    public void RollLoot()
    {
        foreach (Loot item in loot)
        {
            int roll = Random.Range(0, 100);//0-99

            if (roll <= item.MyDropChance)
            {
                droppedItems.Add(item.MyItem);
            }
        }

        rolled = true;
    }

}
