using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text title;

    private LootWindow lootWindow;

    public Image MyIcon { get => icon; }
    public Text MyTitle { get => title; }

    public Item MyLoot { get; set; }

    private void Awake()
    {
        lootWindow = GetComponentInParent<LootWindow>(); 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryScript.MyInstance.AddItem(MyLoot))//Loot item from lootwindow to inventory
        {
            gameObject.SetActive(false);

            lootWindow.TakeLoot(MyLoot);

            UIManager.MyInstance.HideTooltip();
        }        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.MyInstance.ShowTooltip(transform.position, MyLoot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }
}
