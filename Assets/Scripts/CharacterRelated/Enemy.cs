﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void HealthChanged(float health);

public delegate void CharacterRemoved();

public class Enemy : Character, IInteractable
{
    public event HealthChanged healthChanged;

    public event CharacterRemoved characterRemoved;
    /// <summary>
    /// A canvasgroup for the healthbar
    /// </summary>
    [SerializeField]
    private CanvasGroup healthGroup;

    // The enemys current state
    private IState currentState;

    [SerializeField]
    private LootTable lootTable;

    [SerializeField]
    private AStar astar;

    [SerializeField]
    private int damage;//temp for testing

    private bool canDoDamage = true;

    [SerializeField]
    private LayerMask losMask;//Line of Sight mask

    // The enemys attack range
    public float MyAttackRange { get; set; }

    // How much time has passed since the last attack
    public float MyAttackTime { get; set; }

    [SerializeField]
    private Sprite portrait;

    public Sprite MyPortrait
    {
        get
        {
            return portrait;
        }
    }

    public Vector3 MyStartPosition { get; set; }

    [SerializeField]
    private float initAggroRange;

    public float MyAggroRange { get; set; }

    public bool InRange
    {
        get
        {
            return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange;
        }
    }

    public AStar MyAstar
    {
        get
        {
            return astar;
        }
    }

    protected void Awake()
    {
        health.Initialize(initHealth, initHealth);

        //SpriteRenderer sr;
        //sr = GetComponent<SpriteRenderer>();
        //sr.enabled = true;

        MyStartPosition = transform.position;
        MyAggroRange = initAggroRange;
        MyAttackRange = 1;
        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        if (IsAlive)
        {

            if (!IsAttacking)
            {
                MyAttackTime += Time.deltaTime;
            }

            currentState.Update();

            if (MyTarget != null && !Hero.MyInstance.IsAlive)
            {
                ChangeState(new EvadeState());
            }
        }

        base.Update();

    }

    /// <summary>
    /// When the enemy is selected
    /// </summary>
    /// <returns></returns>
    public Transform Select()
    {
        //Shows the health bar
        healthGroup.alpha = 1;

        return hitBox;
    }

    /// <summary>
    /// When we deselect our enemy
    /// </summary>
    public void DeSelect()
    {
        //Hides the healthbar
        healthGroup.alpha = 0;

        healthChanged -= new HealthChanged(UIManager.MyInstance.UpdateTargetFrame);

        characterRemoved -= new CharacterRemoved(UIManager.MyInstance.HideTargetFrame);

    }

    /// <summary>
    /// Makes the enemy take damage when hit
    /// </summary>
    /// <param name="damage"></param>
    public override void TakeDamage(float damage, Transform source)
    {
        if (!(currentState is EvadeState))
        {
            if (IsAlive)
            {
                SetTarget(source);

                base.TakeDamage(damage, source);

                OnHealthChanged(health.MyCurrentValue);

                if (!IsAlive)
                {
                    Hero.MyInstance.MyAttackers.Remove(this);

                    Hero.MyInstance.GainXP(XPManager.CalculateXP((this as Enemy)));
                }
            }
        }

    }

    public void DoDamage()
    {
        if (canDoDamage)
        {
            Hero.MyInstance.TakeDamage(damage, transform);

            canDoDamage = false;
        }

        
    }


    public void CanDoDamage()
    {
        canDoDamage = true;
    }
    /// <summary>
    /// Changes the enemys state
    /// </summary>
    /// <param name="newState">The new state</param>
    public void ChangeState(IState newState)
    {
        if (currentState != null) //Makes sure we have a state before we call exit
        {
            currentState.Exit();
        }

        //Sets the new state
        currentState = newState;

        //Calls enter on the new state
        currentState.Enter(this);
    }

    public void SetTarget(Transform target)
    {
        if (MyTarget == null && !(currentState is EvadeState))
        {
            float distance = Vector2.Distance(transform.position, target.position);
            MyAggroRange = initAggroRange;
            MyAggroRange += distance;
            MyTarget = target;
        }
    }

    public void Reset()
    {
        this.MyTarget = null;
        this.MyAggroRange = initAggroRange;
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;
        OnHealthChanged(health.MyCurrentValue);
    }

    public void Interact()
    {
        Debug.Log("Enemy interact");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);

        //if (!IsAlive)
        //{
        //    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);

        //    List<Drop> drops = new List<Drop>();

        //    foreach (IInteractable interactable in Hero.MyInstance.MyInteractables)
        //    {
        //        if (interactable is Enemy && !(interactable as Enemy).IsAlive)
        //        {
        //            drops.AddRange((interactable as Enemy).lootTable.GetLoot());
        //        }
        //    }

        //    LootWindow.MyInstance.CreatePages(drops);
        //}
    }

    public void StopInteract()
    {
        LootWindow.MyInstance.Close();
    }

    public void OnHealthChanged(float health)
    {
        if (healthChanged != null)
        {
            healthChanged(health);
        }

    }

    public void OnCharacterRemoved()
    {
        if (characterRemoved != null)
        {
            characterRemoved();
        }

        Destroy(gameObject);
    }

    public bool CanSeePlayer()
    {
        Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position),losMask);

        if (hit.collider != null)
        {
            return false;
        }

        return true;

    }
}
