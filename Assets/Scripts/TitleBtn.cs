using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleBtn : MonoBehaviour
{
    string myText;
    public int idx;

    private void Start()
    {
        Text t = transform.GetChild(0).gameObject.GetComponent<Text>();
        myText = t.text;
        var button = GetComponent<ButtonExtention>();
        //button.onClick.AddListener(() => Debug.Log("Click!!"));
        //button.onLongPress.AddListener(() => Debug.Log("LongPress!!"));
        button.onLongPress.AddListener(() => onClickMe());

    }

    public void onClickMe()
    {
        if(GameManager.I.stamina > 0)
        {
            Debug.Log("イカサマ準備OK");
            GameManager.I.SetIkasama(myText,idx);
            GameManager.I.isIkasama = true;
            GameManager.I.IkasamaFlagOn();
        }
        else
        {
            Debug.Log("スタミナが足りません");
            return;
        }
    }
}
