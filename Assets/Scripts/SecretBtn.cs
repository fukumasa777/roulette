using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SecretBtn : MonoBehaviour
{
    [SerializeField] GameObject StaminaPanel = default;
    bool isStaminaPanel;

    private void Start()
    {
        isStaminaPanel = false;
        StaminaPanel.SetActive(false);
        var button = GetComponent<ButtonExtention>();
        button.onLongPress.AddListener(() => onClickMe());

    }

    public void onClickMe()
    {
        if (!isStaminaPanel)
        {
            StaminaPanel.SetActive(true);
            isStaminaPanel = true;
        }
        else
        {
            StaminaPanel.SetActive(false);
            isStaminaPanel = false;
        }
    }


}
