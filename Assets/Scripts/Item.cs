using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Image image;
    public InputField inputField;
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

}
