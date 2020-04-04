using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandler : MonoBehaviour
{
    [SerializeField] private Transform HeroCharacter;

    private void Start()
    {
        SpawnCharacter(true);
        SpawnCharacter(false);
    }

    private void SpawnCharacter(bool isPlayerTeam)
    {
        Vector3 position;
        if (isPlayerTeam)
        {
            position = new Vector3(-5, 0);
        }
        else
        {
            position = new Vector3(+5, 0);
        }
        Instantiate(HeroCharacter, position, Quaternion.identity);
    }
}
