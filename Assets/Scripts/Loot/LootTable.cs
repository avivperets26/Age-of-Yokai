using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    
    [SerializeField]
    private Loot[] loot;

    public List<Drop> MyDroppedItems { get; set; }

    private bool rolled = false;

    public List<Drop> GetLoot()
    {
        if (!rolled)
        {
            MyDroppedItems = new List<Drop>(); 

            RollLoot();
        }

        return MyDroppedItems;
    }

    public void RollLoot()
    {
        foreach (Loot item in loot)
        {
            int roll = Random.Range(0, 100);//0-99

            if (roll <= item.MyDropChance)
            {
                MyDroppedItems.Add(new Drop(item.MyItem,this));
            }
        }

        rolled = true;
    }

}
