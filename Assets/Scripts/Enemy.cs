using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    [SerializeField]
    private CanvasGroup healthGroup;//A canvasgroup for the health bar

    public override Transform Select()//When the enemy is selected
    {
        healthGroup.alpha = 1;//Shows the health bar

        return base.Select();
    }

    public override void DeSelect()//When we deselct our enemy
    {
        healthGroup.alpha = 0;//Hides the healthbar

        base.DeSelect();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        OnHealthChanged(health.MyCurrentValue);
    }

}
