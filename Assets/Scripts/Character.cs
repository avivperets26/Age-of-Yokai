using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private float speed;//Player movement speed

    protected Animator myAnimator;//A reference to the character's animator

    private Vector2 direction;//The palyer's direction

    private Rigidbody2D myRigidbody;//The character rigidbody

    protected bool isAttacking;

    protected Coroutine attackRoutine;//A reference to the attack coroutine

    [SerializeField]
    protected Transform hitBox;

    [SerializeField]
    protected Stat health;

    public Stat Myhealth
    {
        get { return health; }
    }

    [SerializeField]
    private float initHealth;//The Character initial Health

    public bool IsMoving//Indicate if Character is moving or not
    {
        get
        {
            return Direction.x != 0 || Direction.y != 0;
        }
    }

    public Vector2 Direction { get => direction; set => direction = value; }
    public float Speed { get => speed; set => speed = value; }

    protected virtual void Start()
    {
        health.Initialize(initHealth, initHealth);

        myRigidbody = GetComponent<Rigidbody2D>();//Makes a reference to the rigidbody2D

        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()//Update runs once per frame
    {
        HandleLayers();
    }
    private void FixedUpdate()//FixedUpdate can run once, zero, or several times per frame, depending on how many physics frames per second are set in the time settings, and how fast/slow the framerate is. fixed Update garanteed to run and if you make them heavy then your framerate will drop. you can easily test it
    {
        Move();
    }

    //Move the Player
    public void Move()
    {      
        myRigidbody.velocity = Direction.normalized * Speed;//make sure that the player moves

    }
    public void HandleLayers()
    {       

        if (IsMoving)//checks if we are moving or standing still, if we are moving then we need to play the move animation
        {
            //Make Player Animate move
            ActivateLayer("WalkLayer");

            //Sets the animation parameter so that he faces the correct direction
            myAnimator.SetFloat("X", Direction.x);
            myAnimator.SetFloat("Y", Direction.y);

            StopAttack();//Will not able to animate-attack while wallking
        }
        else if(isAttacking)
        {
            ActivateLayer("AttackLayer");
        }
        else
        {
            //Make sure that we will go back to idle when we aren't pressing any keys.
            ActivateLayer("IdleLayer");
        }
    }

    //Activates an aimation layer based on a string
    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < myAnimator.layerCount; i++)
        {
            myAnimator.SetLayerWeight(i, 0);
        }
        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex(layerName), 1);

    }

    //Stops the attack
    public virtual void StopAttack()
    {

        isAttacking = false;//Makes sure that we are not attacking

        myAnimator.SetBool("attack", isAttacking);//Stops the attack animation

        if (attackRoutine != null)//Checks if we have a reference to an co routine
        {
            StopCoroutine(attackRoutine);           
        }        
    }

    public virtual void TakeDamage(float damage)
    {
        health.MyCurrentValue -= damage;

        if(health.MyCurrentValue <= 0)
        {
            myAnimator.SetTrigger("die");
        }
    }
}
