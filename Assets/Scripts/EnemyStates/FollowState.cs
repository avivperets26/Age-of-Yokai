using UnityEngine;

/// <summary>
/// The enmy's follow state
/// </summary>
class FollowState : IState
{
    /// <summary>
    /// A reference to the parent
    /// </summary>
    private Enemy parent;

    private Vector3 offset;

    /// <summary>
    /// This is called whenever we enter the state
    /// </summary>
    /// <param name="parent">The parent enemy</param>
    public void Enter(Enemy parent)
    {
        Hero.MyInstance.AddAttacker(parent);

        this.parent = parent;

        parent.MyPath = null;
    }

    /// <summary>
    /// This is called whenever we exit the state
    /// </summary>
    public void Exit()
    {
        parent.Direction = Vector2.zero;

        parent.MyRigidbody.velocity = Vector2.zero;
    }

    /// <summary>
    /// This is called as long as we are inside the state
    /// </summary>
    public void Update()
    {
        if (parent.MyTarget != null)//As long as we have a target, then we need to keep moving
        {
            //Find the target's direction
            parent.Direction = ((parent.MyTarget.transform.position + offset) - parent.transform.position).normalized;
            
            float distance = Vector2.Distance((parent.MyTarget.position + offset), parent.transform.position);

            string animName = parent.MySpriteRenderer.sprite.name;

            if (animName.Contains("right"))
            {
                offset = new Vector3(0.5f, 0.8f);
            }
            else if (animName.Contains("left"))
            {
                offset = new Vector3(-0.5f, 0.8f);
            }
            else if (animName.Contains("up"))
            {
                offset = new Vector3(0f, 1.2f);
            }
            else if (animName.Contains("down"))
            {
                offset = new Vector3(0, 0);
            }


            if (distance <= parent.MyAttackRange)
            {
                parent.ChangeState(new AttackState());
            }

        }
        if (!parent.InRange)
        {
            parent.ChangeState(new EvadeState());
        } //if we don't have a target, then we need to go back to idle.
        else if (!parent.CanSeePlayer())
        {
            parent.ChangeState(new PathState());
        }
    }
}
