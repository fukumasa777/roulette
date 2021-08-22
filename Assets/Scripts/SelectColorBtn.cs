using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectColorBtn : MonoBehaviour
{
    private Button button;
    private Color myColor;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Selected);
        myColor = GetComponent<Image>().color;
    }

    private void Selected()
    {
        GameManager.I.SelectColorBtn(myColor);
        
    }
}
