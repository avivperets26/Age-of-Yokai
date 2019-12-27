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
        AnimateMovement(direction);
    }

    public void AnimateMovement(Vector2 direction)
    {
        animator.SetFloat("X", direction.x);
        animator.SetFloat("Y", direction.y);
    }

}
