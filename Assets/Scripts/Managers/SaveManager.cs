using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private Item[] items;

    private Chest[] chests;

    private CharButton[] equipment;

    [SerializeField]
    private ActionButton[] actionButtons;

    [SerializeField]
    private SavedGame[] saveSlots;

    [SerializeField]
    private GameObject dialogue;

    [SerializeField]
    private Text dialogueText;
   
    private SavedGame current;

    private string action;

    void Awake()
    {
        chests = FindObjectsOfType<Chest>();

        equipment = FindObjectsOfType<CharButton>();

        foreach (SavedGame saved in saveSlots)
        {
            ShowSavedFiles(saved);//Show saved Files
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Load"))
        {
            Load(saveSlots[PlayerPrefs.GetInt("Load")]);
            PlayerPrefs.DeleteKey("Load");
        }
        else
        {
            Hero.MyInstance.SetDefaultValues();
        }
    }

    public void ShowDialogue(GameObject clickButton)
    {
        action = clickButton.name;

        switch (action)
        {
            case "Load":
                dialogueText.text = "Load game?";
                break;
            case "Save":
                dialogueText.text = "Save game?";
                break;
            case "Delete":
                dialogueText.text = "Delete save?";
                break;
        }

        current = clickButton.GetComponentInParent<SavedGame>();

        dialogue.SetActive(true);
    }

    public void ExecuteAction()
    {
        switch (action)
        {
            case "Load":
                LoadScene(current);
                break;
            case "Save":
                Save(current);
                break;
            case "Delete":
                Delete(current);
                break;
        }

        CloseDialogue();
    }

    private void LoadScene(SavedGame savedGame)
    {
        if (File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);

            SaveData data = (SaveData)bf.Deserialize(file);

            file.Close();

            PlayerPrefs.SetInt("Load", savedGame.MyIndex);

            SceneManager.LoadScene(data.MyScene);
        }
    }

    public void CloseDialogue()
    {
        dialogue.SetActive(false);
    }

    private void Delete(SavedGame savedGame)
    {
        File.Delete(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat");

        savedGame.HideVisuals();
    }

    private void ShowSavedFiles(SavedGame savedGame)
    {
        if (File.Exists(Application.persistentDataPath + "/"+savedGame.gameObject.name+".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat",FileMode.Open);

            SaveData data = (SaveData)bf.Deserialize(file);

            file.Close();

            savedGame.ShowInfo(data);
        }
    }

    public void Save(SavedGame savedGame)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name+ ".dat", FileMode.Create);

            SaveData data = new SaveData();

            data.MyScene = SceneManager.GetActiveScene().name;

            SaveEquipment(data);

            SaveBags(data);

            SaveInventory(data);

            SavePlayer(data);

            SaveChests(data);

            SaveActionButtons(data);

            SaveQuest(data);

            SaveQuestGivers(data);

            bf.Serialize(file, data);

            file.Close();//!!remember to close() alaways!!

            ShowSavedFiles(savedGame);
        }
        catch (System.Exception)
        {
            Delete(savedGame);

            PlayerPrefs.DeleteKey("Load");
        }
    }

    private void SavePlayer(SaveData data)
    {
        data.MyPlayerData = new PlayerData(Hero.MyInstance.MyLevel,
            Hero.MyInstance.MyXp.MyCurrentValue, Hero.MyInstance.MyXp.MyMaxValue,
            Hero.MyInstance.MyHealth.MyCurrentValue,Hero.MyInstance.MyHealth.MyMaxValue,
            Hero.MyInstance.MyStamina.MyCurrentValue,Hero.MyInstance.MyStamina.MyMaxValue,
            Hero.MyInstance.transform.position);
    }

    private void SaveChests(SaveData data)
    {
        for (int i = 0; i < chests.Length; i++)
        {
            data.MyChestData.Add(new ChestData(chests[i].name));

            foreach (Item item in chests[i].MyItems)
            {
                if (chests[i].MyItems.Count > 0)
                {
                    data.MyChestData[i].MyItems.Add(new ItemData(item.MyTitle, item.MySlot.MyItems.Count,item.MySlot.MyIndex));
                }
            }
        }
    }

    private void SaveBags(SaveData data)
    {
        for (int i = 1; i < InventoryScript.MyInstance.MyBags.Count; i++)//First bag is the standert so we will pass it
        {
            data.MyInventoryData.MyBags.Add(new BagData(InventoryScript.MyInstance.MyBags[i].MySlotCount, InventoryScript.MyInstance.MyBags[i].MyBagButton.MyBagIndex));
        }
    }

    private void SaveEquipment(SaveData data)
    {
        foreach (CharButton charButton in equipment)
        {
            if (charButton.MyEquippedArmor != null)
            {
                data.MyEquipmentData.Add(new EquipmenntData(charButton.MyEquippedArmor.MyTitle, charButton.name));
            }
        }
    }
    
    private void SaveActionButtons(SaveData data)
    {
        for (int i = 0; i < actionButtons.Length; i++)
        {
            if (actionButtons[i].MyUseable != null)
            {
                ActionButtonData action;

                if (actionButtons[i].MyUseable is Spell)
                {
                    action = new ActionButtonData((actionButtons[i].MyUseable as Spell).MyTitle, false, i);
                }
                else
                {
                    action = new ActionButtonData((actionButtons[i].MyUseable as Item).MyTitle, true, i);
                }

                data.MyActionButtonData.Add(action);
            }
        }
    }

    private void SaveInventory(SaveData data)
    {
        List<SlotScript> slots = InventoryScript.MyInstance.GetAllItems();

        foreach (SlotScript slot in slots)
        {
            data.MyInventoryData.MyItems.Add(new ItemData(slot.MyItem.MyTitle, slot.MyItems.Count, slot.MyIndex, slot.MyBag.MyBagIndex));
        }
    }

    private void SaveQuest(SaveData data)
    {
        foreach (Quest quest in QuestLog.MyInstance.MyQuests)
        {
            data.MyQuestData.Add(new QuestData(quest.MyTitle, quest.MyDescription, quest.MyCollectObjectives, quest.MyKillObjectives,quest.MyQuestGiver.MyQuestGiverID));
        }
    }

    private void SaveQuestGivers(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestGiver questGiver in questGivers)
        {
            data.MyQuestGiverData.Add(new QuestGiverData(questGiver.MyQuestGiverID, questGiver.MyCompletedQuests));
        }
    }

    private void Load(SavedGame savedGame)//saveSlots[PlayerPrefs.GetInt("Load")]
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);

            SaveData data = (SaveData)bf.Deserialize(file);

            file.Close();//!!remember to close() alaways!!

            Debug.Log("LoadEquipment(data);");
            LoadEquipment(data);

            Debug.Log("LoadBags(data);");
            LoadBags(data);

            Debug.Log("LoadInventory(data);");
            LoadInventory(data);//Inventory load must be after bag !!

            Debug.Log("LoadPlayer(data);");
            LoadPlayer(data);

            Debug.Log("LoadChest(data);");
            LoadChest(data);

            Debug.Log("LoadActionButtons(data);");
            LoadActionButtons(data);

            Debug.Log("LoadQuests(data);");
            LoadQuests(data);

            Debug.Log("LoadQuestGiver(data);");
            LoadQuestGiver(data);

        }
        catch (System.Exception)
        {
            //Debug.Log("Failed to save");
            Delete(savedGame);           

            PlayerPrefs.DeleteKey("Load");

            SceneManager.LoadScene(0);
        }
    }


    private void LoadPlayer(SaveData data)
    {
        Hero.MyInstance.MyLevel = data.MyPlayerData.MyLevel;

        Hero.MyInstance.UpdateLevel();

        Hero.MyInstance.MyHealth.Initialize(data.MyPlayerData.MyHealth, data.MyPlayerData.MyMaxHealth);

        Hero.MyInstance.MyStamina.Initialize(data.MyPlayerData.MyStamina, data.MyPlayerData.MyMaxStamina);

        Hero.MyInstance.MyXp.Initialize(data.MyPlayerData.MyXp, data.MyPlayerData.MyMaxXp);

        Hero.MyInstance.transform.position = new Vector2(data.MyPlayerData.MyX, data.MyPlayerData.MyY);
    }

    private void LoadChest(SaveData data)
    {
        foreach (ChestData chest in data.MyChestData)
        {
            Chest c = Array.Find(chests, x => x.name == chest.MyName);

            foreach (ItemData itemData in chest.MyItems)
            {
                Item item = Instantiate(Array.Find(items, x => x.MyTitle == itemData.MyTitle));

                item.MySlot = c.MyBag.MySlots.Find(x => x.MyIndex == itemData.MySlotIndex);

                c.MyItems.Add(item);
            }
        }
    }

    private void LoadBags(SaveData data)
    {
        foreach (BagData bagData in data.MyInventoryData.MyBags)
        {           
            Bag newBag = (Bag)Instantiate(items[0]);//Bag have to be Element 0 at SaveManager Object!

            newBag.Initialize(bagData.MySlotCount);

            InventoryScript.MyInstance.AddBag(newBag, bagData.MyBagIndex);
        }
    }

    private void LoadEquipment(SaveData data)
    {
        foreach (EquipmenntData equipmenntData in data.MyEquipmentData)
        {
            CharButton cb = Array.Find(equipment, x => x.name == equipmenntData.MyType);//Looking after the Character Buttons

            cb.EquipArmor(Array.Find(items, x => x.MyTitle == equipmenntData.MyTitle) as Armor);//After finding it, equiping it to the armor place
        }
    }

    private void LoadActionButtons(SaveData data)
    {
        foreach (ActionButtonData buttonData in data.MyActionButtonData)
        {
            if (buttonData.IsItem)
            {
                actionButtons[buttonData.MyIndex].SetUseable(InventoryScript.MyInstance.GetUseable(buttonData.MyAction));
            }
            else
            {
                actionButtons[buttonData.MyIndex].SetUseable(SpellBook.MyInstance.GetSpell(buttonData.MyAction));
            }
        }
    }

    //private void LoadInventory(SaveData data)
    //{
    //    foreach (ItemData itemData in data.MyInventoryData.MyItems)
    //    {
    //        Item item = Array.Find(items, x => x.MyTitle == itemData.MyTitle);

    //        for (int i = 0; i < itemData.MyStackCount; i++)
    //        {
    //            InventoryScript.MyInstance.PlaceInSpecific(item, itemData.MySlotIndex, itemData.MyBagIndex);
    //        }
    //    }
    //}
    private void LoadInventory(SaveData data)
    {
        Debug.Log("In LoadInventory method at SaveManager Sript");
        foreach (ItemData itemData in data.MyInventoryData.MyItems)
        {
            Debug.Log("In foreach Loop");

            Item item = Instantiate(Array.Find(items, x => x.MyTitle == itemData.MyTitle));

            Debug.Log("After Instantiate item");

            for (int i = 0; i < itemData.MyStackCount; i++)
            {
                Debug.Log("In For Loop");

                InventoryScript.MyInstance.PlaceInSpecific(item, itemData.MySlotIndex, itemData.MyBagIndex);
            }
        }
    }

    private void LoadQuests(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestData questData in data.MyQuestData)
        {
            QuestGiver qg = Array.Find(questGivers, x => x.MyQuestGiverID == questData.MyQuestGiverID);

            Quest q = Array.Find(qg.MyQuests, x => x.MyTitle == questData.MyTitle);

            q.MyQuestGiver = qg;

            q.MyKillObjectives = questData.MykillObjectives;

            QuestLog.MyInstance.AcceptQuest(q);
        }
    }

    private void LoadQuestGiver(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestGiverData questGiverData in data.MyQuestGiverData)
        {
            QuestGiver questGiver = Array.Find(questGivers, x => x.MyQuestGiverID == questGiverData.MyQuestGiverID);

            questGiver.MyCompletedQuests = questGiverData.MyCompletedQuests;

            questGiver.UpdateQuestStatus();
        }
     }
}

