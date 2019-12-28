using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private float speed;

    protected Vector2 direction;

    //protected Vector3 targetPosition;

    private Animator animator;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
    }

    //Move the Player
    public void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if(direction.x !=0 || direction.y != 0)
        {
            //Animate Player movements
            AnimateMovement(direction);
        }
        else
        {
            // Player movment Animate return to  be Idle
            animator.SetLayerWeight(1, 0);
        }
        
    }

    public void AnimateMovement(Vector2 direction)
    {
        //Make Player Animate move
        animator.SetLayerWeight(1, 1);

        animator.SetFloat("X", direction.x);
        animator.SetFloat("Y", direction.y);
    }

}
