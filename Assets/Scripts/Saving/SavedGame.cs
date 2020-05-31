using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedGame : MonoBehaviour
{
    [SerializeField]
    private Text dateTime;

    [SerializeField]
    private Image stamina;

    [SerializeField]
    private Image health;

    [SerializeField]
    private Image xp;

    [SerializeField]
    private Text staminaText;

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text xpText;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private GameObject visuals;

    [SerializeField]
    private int index;

    public int MyIndex
    {
        get
        {
            return index;
        }
    }

    private void Awake()
    {
        //visuals.SetActive(false);
    }

    public void ShowInfo(SaveData saveData)
    {
        visuals.SetActive(true);

        dateTime.text = "Date: " + saveData.MyDateTime.ToString("dd/MM/yyy") + " - Time: " + saveData.MyDateTime.ToString("H:mm");

        stamina.fillAmount = saveData.MyPlayerData.MyStamina / saveData.MyPlayerData.MyMaxStamina;

        staminaText.text = saveData.MyPlayerData.MyStamina + " / " + saveData.MyPlayerData.MyMaxStamina;

        health.fillAmount = saveData.MyPlayerData.MyStamina / saveData.MyPlayerData.MyMaxHealth;

        healthText.text = saveData.MyPlayerData.MyHealth + " / " + saveData.MyPlayerData.MyMaxHealth;

        xp.fillAmount = saveData.MyPlayerData.MyXp / saveData.MyPlayerData.MyMaxXp;

        xpText.text = saveData.MyPlayerData.MyXp + " / " + saveData.MyPlayerData.MyMaxXp;

        levelText.text = saveData.MyPlayerData.MyLevel.ToString();
    }

    public void HideVisuals()
    {
        visuals.SetActive(false);
    }

}
