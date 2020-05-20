using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;//Prefab for creating slots

    private CanvasGroup canvasGroup;//A canvasgroup for hiding and showing the bag

    private List<SlotScript> slots = new List<SlotScript>();

    public bool IsOpen//Indicates if this bag is open or closed
    {
        get
        {
            return canvasGroup.alpha > 0;
        }
    }

    public List<SlotScript> MySlots {
        get
        {
            return slots;
        }
    }

    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;

            foreach (SlotScript slot in MySlots)
            {
                if (slot.IsEmpty)
                {
                    count++;
                }
            }

            return count;
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();//Create reference to the canvasGroup
    }

    public List<Item> GetItems()
    {
        List<Item> items = new List<Item>();

        foreach (SlotScript slot in slots)
        {
            if (!slot.IsEmpty)
            {
                foreach (Item item in slot.MyItems)
                {
                    items.Add(item);
                }

            }
        }

        return items;
    }

    public void AddSlots(int slotCount)//Creates slots for this bag
    {
        for(int i =0; i < slotCount; i++)
        {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();

            slot.MyBag = this;

            MySlots.Add(slot);
        }
    }

    public bool AddItem(Item item)
    {
        foreach (SlotScript slot in MySlots)
        {
            if (slot.IsEmpty)
            {
                slot.AddItem(item);

                return true;
            }
        }

        return false;
    }

    public void OpenClose()//Opens or closes bag
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;//changes the alpha to open or closed

        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;//Blocks or removes raycast blocking
    }

    public void Clear()
    {
        foreach (SlotScript slot in slots)
        {
            slot.Clear();
        }
    }

}
