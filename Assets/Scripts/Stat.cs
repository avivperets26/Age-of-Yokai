using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{

    private Image content;//The actual image that we are chaing the fill of

    [SerializeField]
    private Text statValue;//A reference to the value text on the bar

    [SerializeField]
    private float lerpSpeed;//The lerp speed

    private float currnetFill;// Hold the current fill valu, we use this so that we know if we need to lerp a value

    private float currentValue =100;//The currentValue for example the current health or mana

    public float MyMaxValue { get; set; }//The stat's maxValue for example max health or mana

    public float MyCurrentValue//Proprty for setting the current value, this has to be used every time we change the currentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            if (value > MyMaxValue)//Makes sure that we don't get too much health
            {
                currentValue = MyMaxValue;
            }
            else if (value < 0)// Makes sure that we don't get health below 0
            {
                currentValue = 0;
            }
            else//Makes sure that we set the current value within the bound of 0 to max health
            {
                currentValue = value;
            }

            
            currnetFill = currentValue / MyMaxValue;//Calculates the currentFill, so that we can lerp

            if(statValue != null)
            {
                statValue.text = currentValue + "/" + MyMaxValue;//Makes sure that we update the value text
            }
          
        }
    }

    
    void Start()//Use it for initialization
    {
        content = GetComponent<Image>();// Creates a reference to the content
    }

    // Update is called once per frame
    void Update()
    {
        HandleBar();
    }

    public void Initialize(float currentVlaue, float maxValue)///Initialises the bar, currentVlaue - The current value of the bar, maxValue - The max value of the bar
    {
        if(content == null)
        {
            content = GetComponent<Image>();
        }

        MyMaxValue = maxValue;

        MyCurrentValue = currentValue;

        content.fillAmount = MyCurrentValue / MyMaxValue;
    }

    private void HandleBar()//Makes sure that the bar updates
    {
        if (currnetFill != content.fillAmount)//if we have a new fill amount then we know that we need to update
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currnetFill, Time.deltaTime * lerpSpeed);//Lerp the fill amount so that we get a smooth movement
        }
    }
}
