using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IdleEnemy : NPC, IInteractable
{
    public virtual void Interact()
    {
        if (!IsInteracting)
        {
            IsInteracting = true;

            Debug.Log("Player 1 IdleEnemy interact");

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public virtual void StopInteract()
    {
        if (IsInteracting)
        {
            IsInteracting = false;

            
        }
    }
}
