using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverWindow : Window
{
    private QuestGiver questGiver;

    [SerializeField]
    private GameObject questPrfab;

    [SerializeField]
    private Transform questArea;

    public void ShowQuest(QuestGiver questGiver)
    {
        this.questGiver = questGiver;

        foreach (Quest quest in questGiver.MyQuests)
        {
            GameObject go = Instantiate(questPrfab, questArea);

            go.GetComponent<Text>().text = quest.MyTitle;
        }
    }

    public override void Open(NPC npc)
    {
        ShowQuest((npc as QuestGiver));

        base.Open(npc);
    }
}
