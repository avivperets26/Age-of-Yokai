using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{

    private Image content;

    [SerializeField]
    private Text statValue;

    [SerializeField]
    private float lerpSpeed;
    private float currnetFill;
    private float currentValue;
    public float MyMaxValue { get; set; }
    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            if (value > MyMaxValue)
            {
                currentValue = MyMaxValue;
            }
            else if (value < 0)
            {
                currentValue = 0;
            }
            else {
                currentValue = value;
            }

            
            currnetFill = currentValue / MyMaxValue;

            statValue.text = currentValue + "/" + MyMaxValue;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        content = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currnetFill != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currnetFill, Time.deltaTime * lerpSpeed);
        }
    }

    public void Initialize(float currentVlaue, float maxValue)
    {
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
    }
}
