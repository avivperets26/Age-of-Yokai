using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayGame : MonoBehaviour
{
    public TMP_Dropdown ChooseHeroDropdown;
 
    //When Cancelation warning apears and player chooses "yes" -> Go to Main Menu
    public void Cancel()
    {
        SceneManager.LoadScene("Main Menu");
    }
    
}
