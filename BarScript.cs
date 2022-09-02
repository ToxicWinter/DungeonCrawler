using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    [SerializeField]
    private float fillAmount;
    [SerializeField]
    private Image bar;
    [SerializeField]
    private Text barText;

    public float MaxValue {get; set;}
    public float Value
    {
        set
        {
            fillAmount = mapValue(value, 0, MaxValue);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        calculateBar();
    }
    private void calculateBar()
    {
        if( fillAmount != bar.fillAmount)
        {
            bar.fillAmount = fillAmount;
        }
    }

    private float mapValue(float currentValue, float minValue, float maxValue) 
    {
        if(currentValue <= 0)
        {
            currentValue = 0;
        }
        barText.text = currentValue.ToString() + "/" + maxValue.ToString();
        return (currentValue - minValue)/(maxValue - minValue);
    }
}
