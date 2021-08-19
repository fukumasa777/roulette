using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleBtn : MonoBehaviour
{
    string myText;
    private void Start()
    {
        Text t = transform.GetChild(0).gameObject.GetComponent<Text>();
        myText = t.text;
        
    }

    public void onClickMe()
    {
        
        GameManager.I.SetIkasama(myText);
        GameManager.I.RotationBtn(true);
    }

    
}
