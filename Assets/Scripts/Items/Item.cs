using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject, IMoveable//Superclass for all items
{
    [SerializeField]
    private Sprite icon;//Icon used when moving and placing the items

    [SerializeField]
    private int stackSize;//The size of the stack, less then 2 is not stackable

    protected SlotScript slot;//A reference to the slot that this item is sitting on

    public Sprite MyIcon { get => icon; }//Property for accessing the icon
    public int MyStackSize { get => stackSize; }//Property for accessing the stacksize
    public SlotScript MySlot { get => slot; set => slot = value; }

    public void Remove()
    {
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }
}
