using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
   public void GoToGameBoardScene()
    {
        SceneManager.LoadScene("Game Board Scene");
    }
}
