using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ButtonExtention))]
public class ButtonAction : MonoBehaviour
{
    void Start()
    {
        var button = GetComponent<ButtonExtention>();
        button.onClick.AddListener(() => Debug.Log("Click!!"));
        button.onLongPress.AddListener(() => Debug.Log("LongPress!!"));
    }
}