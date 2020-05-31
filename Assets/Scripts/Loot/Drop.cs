using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop
{
    public Item MyItem { get; set; }

    public LootTable MyLootTable { get; set; }

    public Drop(Item item, LootTable lootTable)
    {
        MyLootTable = lootTable;

        MyItem = item;
    }

    public void Remove()
    {
        MyLootTable.MyDroppedItems.Remove(this);
    }
}
