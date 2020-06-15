using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeTurnButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField]
    private Image pointBarImage = default;

    [SerializeField]
    private Text title;

    private int currentScore = 0;

    private int maxScore = 1;

    private bool isPowerUp = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Hero.MyInstance.MyStamina.MyCurrentValue == 0 && isPowerUp == true)
        {
            ChangeScore();
        }
        else if (Hero.MyInstance.MyStamina.MyCurrentValue != 0 && isPowerUp == false)
        {
            isPowerUp = true;
        }

    }
    public void ChangeTurn()
    {
        Hero.MyInstance.ChangeTurn();               
    }

    public void ChangeScore()
    {
        if (currentScore < maxScore)
        {
            currentScore++;
            //pointBar.SetValue(currentScore);
            //text.text = currentScore.ToString();
        }
        if (currentScore == maxScore)
        {
            isPowerUp = false;
            StartCoroutine(IndicatePowerUp());
        }
    }
    private IEnumerator IndicatePowerUp()
    {
        //float j = 1;

        //for (int i = 1; i < 10; i++)
        //{

        //    pointBarImage.rectTransform.localScale = new Vector3(1, 1, 0);
        //}

        for (int i = 0; i < 3; i++)
        {
           // int j = 1;
            pointBarImage.enabled = false;
            //pointBarImage.rectTransform.localScale = new Vector3(1, 1, 0);
            title.enabled = false;
            yield return new WaitForSeconds(0.3f);
            //j += 0.2;
            pointBarImage.enabled = true;
            //pointBarImage.rectTransform.localScale = new Vector3(2, 2, 0);
            title.enabled = true;
            yield return new WaitForSeconds(0.3f);
        }
        ResetPowerUp();
    }
    public void ResetPowerUp()
    {
        if (isPowerUp)
        {
            currentScore = 0;
            isPowerUp = false;
            //pointBar.SetValue(currentScore);
            //healthUp.Play();
            //currentHealth = maxHealth;
            //healthBar.SetValue(maxHealth);
        }
    }
}
