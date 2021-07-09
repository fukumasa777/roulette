using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    public GameManager gameManager;

    public string targetName;

    public void OnTriggerStay2D(Collider2D other)
    {
        
        Debug.Log("OnTriggerStay2D: " + other.gameObject.name);
        targetName = other.gameObject.GetComponentInChildren<Text>().text;

    }
    
}
