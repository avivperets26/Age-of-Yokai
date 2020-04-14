using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bag",menuName ="Item/Bag",order = 1)]
public class Bag : Item, IUseable
{
    private int slots;
    
    [SerializeField]
    private GameObject bagPrefab;

    public BagScript MyBagScript { get; set; }
    public int Slots { get => slots; }

    public void Initialize(int slots)
    {
        this.slots = slots;
    }

    public void Use()
    {
        MyBagScript = Instantiate(bagPrefab,InventoryScript.MyInstance.transform).GetComponent<BagScript>();
        MyBagScript.AddSlots(slots);
    }
}
