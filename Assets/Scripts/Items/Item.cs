using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Quality {Common, Uncommon, Rare, Epic}

public abstract class Item : ScriptableObject, IMoveable,IDescribable//Superclass for all items
{
    [SerializeField]
    private Sprite icon;//Icon used when moving and placing the items

    [SerializeField]
    private int stackSize;//The size of the stack, less then 2 is not stackable

    [SerializeField]
    private string title;

    [SerializeField]
    private Quality quality;

    protected SlotScript slot;//A reference to the slot that this item is sitting on

    public Sprite MyIcon { get => icon; }//Property for accessing the icon
    public int MyStackSize { get => stackSize; }//Property for accessing the stacksize
    public SlotScript MySlot { get => slot; set => slot = value; }

    public void Remove()//Removes the item from the inventory
    {
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }

    public virtual string GetDescription()//Return a description of this specific item, Virtual to override it by other items
    {
        string color = string.Empty;

        switch (quality)
        {
            case Quality.Common:
                color = "#d6d6d6";
                break;
            case Quality.Uncommon:
                color = "#00ff00ff";
                break;
            case Quality.Rare:
                color = "#0000ffff";
                break;
            case Quality.Epic:
                color = "#800080ff";
                break;
        }

        return string.Format("<color={0}>{1}</color>",color, title);
    }
}
