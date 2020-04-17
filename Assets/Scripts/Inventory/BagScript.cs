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

    public List<SlotScript> MySlots { get => slots; }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();//Create reference to the canvasGroup
    }

    public void AddSlots(int slotCount)//Creates slots for this bag
    {
        for(int i =0; i < slotCount; i++)
        {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();

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

}
