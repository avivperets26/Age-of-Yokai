﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bag",menuName ="Item/Bag",order = 1)]
public class Bag : Item, IUseable
{
    [SerializeField]
    private int slots;//The amount of slots this bag has
    
    [SerializeField]
    private GameObject bagPrefab;//A reference to a bag prefab, so that we can instanitate a bag in the game

    public BagScript MyBagScript { get; set; }//A reference to the bagScript, this bag belong to

    public BagButton MyBagButton { get; set; }

    
    public int MySlotCount { get => slots; }//Property for getting the slots

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

            if (MyBagButton == null)
            {
                InventoryScript.MyInstance.AddBag(this);
            }
            else
            {
                InventoryScript.MyInstance.AddBag(this, MyBagButton);
            }

            MyBagScript.MyBagIndex = MyBagButton.MyBagIndex;
        }
    }

    public void SetUpScript()
    {
        MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();

        MyBagScript.AddSlots(slots);
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n{0} slot bag", slots);

    }
}
