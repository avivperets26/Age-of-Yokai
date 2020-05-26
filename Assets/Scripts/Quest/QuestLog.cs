﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    [SerializeField]
    private GameObject questPrefab;

    [SerializeField]
    private Transform questParent;

    private Quest selected;

    [SerializeField]
    private Text questDescription;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Text questCountTxt;

    [SerializeField]
    private int maxCount;
  
    private int currentCount;

    private List<QuestScript> questScripts = new List<QuestScript>();

    private List<Quest> quests = new List<Quest>();

    private static QuestLog instance;

    public static QuestLog MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestLog>();
            }

            return instance;
        }
    }

    public List<Quest> MyQuests
    {
        get
        {
            return quests;
        }

        set
        {
            quests = value;
        }
    }

    private void Start()
    {
        questCountTxt.text = currentCount + "/" + maxCount;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            OpenClose();
        }
    }

    public void AcceptQuest(Quest quest)
    {
        if (currentCount < maxCount)
        {
            currentCount++;

            questCountTxt.text = currentCount + "/" + maxCount;

            foreach (CollectObjective o in quest.MyCollectObjectives)
            {
                InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(o.UpdateItemCount);

                o.UpdateItemCount();
            }

            foreach (KillObjective o in quest.MyKillObjectives)
            {
                GameManager.MyInstance.killConfirmedEvent += new KillConfirmed(o.UpdateKillCount);
            }

            quests.Add(quest);

            GameObject go = Instantiate(questPrefab, questParent);

            QuestScript qs = go.GetComponent<QuestScript>();

            quest.MyQuestScript = qs;

            qs.MyQuest = quest;

            questScripts.Add(qs);

            go.GetComponent<Text>().text = quest.MyTitle;

            CheckCompletion();
        }
    }

    public void UpdateSelected()
    {
        ShowDescription(selected);
    }

    public void ShowDescription(Quest quest)
    {
        if (quest != null)
        {
            if (selected != null && selected != quest)
            {
                selected.MyQuestScript.DeSelect();
            }

            string objectives = string.Empty;

            selected = quest;

            string title = quest.MyTitle;

            foreach (Objective obj in quest.MyCollectObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }

            foreach (Objective obj in quest.MyKillObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }

            questDescription.text = string.Format("<b>{0}\n<size=10>{1}</size>\n\nObjectives\n<size=10>{2}</size></b>", title, quest.MyDescription, objectives);
        }      
    }

    public void CheckCompletion()
    {
        foreach (QuestScript qs in questScripts)
        {
            qs.MyQuest.MyQuestGiver.UpdateQuestStatus();

            Debug.Log("Checking if complete");
            qs.IsComplete();
        }
    }

    public void OpenClose()
    {
        if (canvasGroup.alpha == 1)
        {
            Close();
        }
        else
        {
            canvasGroup.alpha = 1;

            canvasGroup.blocksRaycasts = true;
        }
    }

    public void Close()
    {
        canvasGroup.alpha = 0;

        canvasGroup.blocksRaycasts = false;
    }

    public void AbondonQuest()//Remove the quest from the quest log
    {
        foreach (CollectObjective o in selected.MyCollectObjectives)
        {
            InventoryScript.MyInstance.itemCountChangedEvent -= new ItemCountChanged(o.UpdateItemCount);
        }

        foreach (KillObjective o in selected.MyKillObjectives)
        {
            GameManager.MyInstance.killConfirmedEvent -= new KillConfirmed(o.UpdateKillCount);
        }

        RemoveQuest(selected.MyQuestScript);

    }

    public void RemoveQuest(QuestScript qs)
    {
        questScripts.Remove(qs);

        Destroy(qs.gameObject);

        quests.Remove(qs.MyQuest);

        questDescription.text = string.Empty;

        selected = null; // Deselecting

        currentCount--;

        questCountTxt.text = currentCount + "/" + maxCount;

        qs.MyQuest.MyQuestGiver.UpdateQuestStatus();

        qs = null;
    }

    public bool HasQuest(Quest quest)
    {
        return quests.Exists(x => x.MyTitle == quest.MyTitle);
    }


}