using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SCTTYPE { DAMAGE,HEAL,XP}

public class CombatTextManager : MonoBehaviour
{

    private static CombatTextManager instace;

    public static CombatTextManager MyInstace//Singeltone
    {
        get
        {
            if (instace == null)
            {
                instace = FindObjectOfType<CombatTextManager>();
            }
            return instace;
        }
    }

    [SerializeField]
    private GameObject combatTextPrefab;

    public void CreatText(Vector2 position, string text, SCTTYPE type, bool crit)
    {
        position.y += 0.5f;//offset

        Text sct = Instantiate(combatTextPrefab, transform).GetComponent<Text>();

        sct.transform.position = position;

        string operation = string.Empty;

        string before = string.Empty;

        string after = string.Empty;

        switch (type)
        {
            case SCTTYPE.DAMAGE:
                operation = "-";
                sct.color = Color.red;
                break;
            case SCTTYPE.HEAL:
                operation = "+";
                sct.color = Color.green;
                break;
            case SCTTYPE.XP:
                operation = "+";
                after = " XP";
                sct.color = Color.yellow;
                break;
            default:
                break;
        }

        sct.text = operation + text + after;

        if (crit)
        {
            sct.GetComponent<Animator>().SetBool("Crit", crit);
        }
    }
}
