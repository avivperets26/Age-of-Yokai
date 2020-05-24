using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField]
    private Quest[] quests;

    public Quest[] MyQuests
    {
        get
        {
            return quests;
        }
    }
}
