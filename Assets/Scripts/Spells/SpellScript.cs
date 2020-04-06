using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{

    private Rigidbody2D myRigidbody;//A reference to the spell's rigid body

    [SerializeField]
    private float speed;//The spell's movement speed


    public Transform MyTarget { get;private set; }//The spells target

    private Transform source;

    private int damage;

    void Start()//Use for initalization
    {
        myRigidbody = GetComponent<Rigidbody2D>();//Creates a reference to the spell's rigidbody

    }

    public void Initialized(Transform target, int damage, Transform source)
    {
        this.MyTarget = target;
        this.damage = damage;
        this.source = source;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (MyTarget != null)
        {
            Vector2 direction = MyTarget.position - transform.position; //target = Enemy position, Transform = Fire ball position , by decreasing them we will be able to make sure that the fire ball will follow the enemy.

            myRigidbody.velocity = direction.normalized * speed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "HitBox" && collision.transform == MyTarget)
        {
            Character c = collision.GetComponentInParent<Character>();

            speed = 0;

            c.TakeDamage(damage, source);

            GetComponent<Animator>().SetTrigger("impact");

            myRigidbody.velocity = Vector2.zero;

            MyTarget = null;
        }
    }
}
