using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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
        return string.Format("<color={0}>{1}</color>",QualityColor.MyColors[quality], title);
    }
}
