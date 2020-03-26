using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{

    private Rigidbody2D myRigidbody;

    [SerializeField]
    private float speed;

    public Transform MyTarget { get; set; }

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

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
        if(collision.tag == "HitBox" && collision.transform == MyTarget.transform)
        {
            GetComponent<Animator>().SetTrigger("impact");

            myRigidbody.velocity = Vector2.zero;

            MyTarget = null;
        }
    }
}
