using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanel : MonoBehaviour
{
    private static CharacterPanel instance;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private CharButton Head, Chest, MainHand, Offhand, Charm1, Charm2, Charm3;

    public static CharacterPanel MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CharacterPanel>();
            }
            return instance;
        }
    }

    public void OpenClose()
    {
        if (canvasGroup.alpha <= 0)
        {
            canvasGroup.blocksRaycasts = true;

            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.blocksRaycasts = false;

            canvasGroup.alpha = 0;
        }
    }

    public void EquipArmor (Armor armor)
    {
        switch (armor.MyArmorType)
        {
            case ArmorType.Head:
                Head.EquipArmor(armor);
                break;
            case ArmorType.Chest:
                Chest.EquipArmor(armor);
                break;
            case ArmorType.MainHand:
                MainHand.EquipArmor(armor);
                break;
            case ArmorType.Offhand:
                Offhand.EquipArmor(armor);
                break;
            case ArmorType.Charm:
                Charm1.EquipArmor(armor);
                break;
        }
    }
}
