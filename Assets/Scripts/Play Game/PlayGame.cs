using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayGame : MonoBehaviour
{   

    public TMP_Dropdown ChooseHumanHeroPlayer1Dropdown;
    public TMP_Dropdown ChooseYokaiHeroPlayer1Dropdown;
    public TMP_Dropdown ChooseHumanHeroPlayer2Dropdown;
    public TMP_Dropdown ChooseYokaiHeroPlayer2Dropdown;

    public TMP_Dropdown ChosenArmyPlayer1Dropdown;
    public TMP_Dropdown HumanArmyPlayer2Dropdown;
    public TMP_Dropdown YokaiArmyPlayer2Dropdown;

    //Manipulation on dropdown buttons
    public void HandleHeroesPlayer1Dropdown(int val)
    {

        if (val == 0) //player 1 chooses humans as army 1
        {
            //player 1 can choose human heroes only
            ChooseHumanHeroPlayer1Dropdown.gameObject.SetActive(true);
            ChooseYokaiHeroPlayer1Dropdown.gameObject.SetActive(false);

            //player 2 can choose only the yokai's as army 2
            YokaiArmyPlayer2Dropdown.gameObject.SetActive(true);
            HumanArmyPlayer2Dropdown.gameObject.SetActive(false);

            //player 2 can choose only the yokai's heroes in army 2
            ChooseYokaiHeroPlayer2Dropdown.gameObject.SetActive(true);
            ChooseHumanHeroPlayer2Dropdown.gameObject.SetActive(false);


        }
        else if (val == 1) //player chooses yokai's as army 1 
        {
            //player 1 can choose Yokai's heroes only
            ChooseHumanHeroPlayer1Dropdown.gameObject.SetActive(false);
            ChooseYokaiHeroPlayer1Dropdown.gameObject.SetActive(true);

            //player 2 can choose only the humans as army 2
            YokaiArmyPlayer2Dropdown.gameObject.SetActive(false);
            HumanArmyPlayer2Dropdown.gameObject.SetActive(true);

            //player 2 can choose only the humans heroes in army 2
            ChooseYokaiHeroPlayer2Dropdown.gameObject.SetActive(false);
            ChooseHumanHeroPlayer2Dropdown.gameObject.SetActive(true);
        }
     
    }




    //When Cancelation warning apears and player chooses "yes" -> Go to Main Menu
    public void Cancel()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void NextScene()
    {
        //Here will be the next scene loaded after pressing "Continue"
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
