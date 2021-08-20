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
        var button = GetComponent<ButtonExtention>();
        button.onClick.AddListener(() => Debug.Log("Click!!"));
        button.onLongPress.AddListener(() => Debug.Log("LongPress!!"));
        button.onLongPress.AddListener(() => onClickMe());

    }

    public void onClickMe()
    {
        
        GameManager.I.SetIkasama(myText);
        GameManager.I.isIkasama = true;
    }

    
}
