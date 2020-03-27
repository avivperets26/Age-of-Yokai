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

    protected Vector2 direction;//The palyer's direction

    private Rigidbody2D myRigidbody;   

    protected bool isAttacking;

    protected Coroutine attackRoutine;//A reference to the attack coroutine

    [SerializeField]
    protected Transform hitBox;

    [SerializeField]
    protected Stat health;

    [SerializeField]
    private float initHealth;//The Character initial Health

    public bool IsMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
        }
    }

   
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
        myRigidbody.velocity = direction.normalized * speed;//make sure that the player moves

    }
    public void HandleLayers()
    {       

        if (IsMoving)//checks if we are moving or standing still, if we are moving then we need to play the move animation
        {
            //Make Player Animate move
            ActivateLayer("WallkLayer");

            //Sets the animation parameter so that he faces the correct direction
            myAnimator.SetFloat("X", direction.x);
            myAnimator.SetFloat("Y", direction.y);

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
