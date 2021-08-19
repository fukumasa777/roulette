using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Image image;
    public InputField inputField;
    public InputField inputFieldPer;
    public int rate;
    




    public void Set(Color color, int rate)
    {
        image.color = color;
        this.rate = rate;
    }

    public Color GetColor()
    {
        return image.color;
    }

    public string GetText()
    {
        return inputField.text;
    }
    public void OnChangedValue(string value)
    {
        
        if (value == "")
        {
            rate = 1;
        }
        else
        {
            rate = int.Parse(value);
        }
    }

    public void ViewColorPanel(GameObject currentBtn)
    {
        GameManager.I.ColorChoiceBtn(currentBtn);
    }




}