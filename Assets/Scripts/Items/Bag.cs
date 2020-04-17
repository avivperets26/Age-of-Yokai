using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bag",menuName ="Item/Bag",order = 1)]
public class Bag : Item, IUseable
{
    private int slots;//The amount of slots this bag has
    
    [SerializeField]
    private GameObject bagPrefab;//A reference to a bag prefab, so that we can instanitate a bag in the game

    public BagScript MyBagScript { get; set; }//A reference to the bagScript, this bag belong to

    public int Slots { get => slots; }//Property for getting the slots

    public void Initialize(int slots)
    {
        this.slots = slots;
    }

    public void Use()//Equip the Bag
    {
        if (InventoryScript.MyInstance.CanAddBag)//Only if less than 5 bags exist
        {
            Remove();

            MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();

            MyBagScript.AddSlots(slots);

            InventoryScript.MyInstance.AddBag(this);
        }
    }
}
