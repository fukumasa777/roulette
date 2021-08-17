﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject RouletteG = default;
    [SerializeField] Transform Roulette = default;
    [SerializeField] Image circlePrefab = default;
    [SerializeField] Image imagePrefab2 = default;
    [SerializeField] Image TitlePrefab = default;

    [SerializeField] Image imagePrefab4 = default;
    [SerializeField] Text resultText = default;


    [SerializeField] GameObject setBG = default;
    [SerializeField] GameObject colorPanel = default;

    [SerializeField] Transform setPanel = default;
    [SerializeField] Item itemPrefab = default;


    List<Item> itemList = new List<Item>();
    List<GameObject> itemRouletteObjList = new List<GameObject>();
    List<GameObject> titleList = new List<GameObject>();

    List<Color> colorList = new List<Color>();


    public float rotSpeed = 0;

    public float sumRate = 0;

    private void Start()
    {
        setBG.SetActive(false);
        Debug.Log("スタート");
        colorList.Add(new Color( 255,205,0,1));

    }

    public bool isRouletteStart;

    private void Update()
    {
        if (isRouletteStart)
        {
            Roulette.transform.Rotate(0, 0, rotSpeed);//加速
            rotSpeed *= 0.99f;//減速
        }
        if (rotSpeed >= -0.01f && isRouletteStart)
        {
            isRouletteStart = false;
            Debug.Log("止まる");
            result();
        }
    }

    //回転
    public void RotationBtn()
    {
        isRouletteStart = true;
        rotSpeed = -1;
    }

    //結果表示
    public void result()
    {
        if (180 < RouletteG.transform.localEulerAngles.z && Roulette.transform.localEulerAngles.z <= 360)
        {
            resultText.text = "結果 ： " + itemList[0].GetText();
            Debug.Log(itemList[0].GetText());
        }
        if (180 < RouletteG.transform.localEulerAngles.z && Roulette.transform.localEulerAngles.z <= 360)
        {
            resultText.text = "結果 ： " + itemList[1].GetText();
            Debug.Log(itemList[1].GetText());
        }
    }

    //項目追加画面表示
    public void SetMenuBtn()
    {
        DestroyListRouletteObj();
        setBG.SetActive(true);
    }

    // リセットのときに実行
    public void DestroyListObj()
    {
        foreach (Item item in itemList)
        {
            Destroy(item.gameObject);
        }
        itemList.Clear();
    }
    void DestroyListRouletteObj()
    {
        foreach (GameObject obj in itemRouletteObjList)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in titleList)
        {
            Destroy(obj);
        }
        itemRouletteObjList.Clear();
        titleList.Clear();
    }

    //項目追加しルーレット画面へ
    public void SetBtn()
    {
        if(itemList.Count <= 0)
        {
            setBG.SetActive(false);
            return;
        }
        setBG.SetActive(false);
        sumRate = 0;
        for (int i = 0; i < itemList.Count; i++)
        {
            sumRate += itemList[i].rate;
        }
        float nextAngle = 0;
        float nextTitleAngle = (360f * itemList[0].rate / sumRate) / 2f;
        for (int i = 0; i < itemList.Count; i++)
        {
            float rate = itemList[i].rate / sumRate;
            Spawn(nextAngle, rate, itemList[i].GetColor(), itemList[i].GetText());
            TitleSpawn(nextTitleAngle, rate, itemList[i].GetText());
            nextAngle += 360 * rate;
            nextTitleAngle += 360f * rate;
        }
    }



    //項目追加
    public void ItemPlusBtn()
    {
        Item item = Instantiate(itemPrefab, setPanel, false);
        Color color = colorList[0];
        //Color color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 1);
        item.Set(color, 1);
        itemList.Add(item);
        // itemList.Add(new ItemData(Color.red, "りんご", 1));
    }

    public void ItemResetBtn()
    {
        // List<Item> itemList = new List<Item>();
        DestroyListObj();
    }

    //色選択画面へ
    public void ColorChoiceBtn()
    {
        colorPanel.SetActive(true);
    }

    //色選択画面から戻る
    public void ColorReturnBtn()
    {
        colorPanel.SetActive(false);
    }




    //項目生成



    /*原型
    void Spawn(float angle, float rate, Color color, string text)
    {
        Image image = Instantiate(imagePrefab2, Roulette, false);
        image.transform.rotation = Quaternion.Euler(0, 0, angle);
        image.fillAmount = rate;
        image.color = color;
        //image.GetComponentInChildren<Text>().text = text;
    }
    */

    void Spawn(float angle, float rate, Color color, string text)
    {
        Image image = Instantiate(circlePrefab, Roulette, false);
        image.transform.rotation = Quaternion.Euler(0, 0, 360 - angle);
        image.fillAmount = rate;
        Debug.Log(angle);
        image.color = color;
        itemRouletteObjList.Add(image.gameObject);
    }

    void TitleSpawn(float angle, float rate, string text)
    {
        if (itemList.Count == 0)
        {
            return;
        }
        Image image = Instantiate(TitlePrefab, Roulette, false);
        float angleDeg = Mathf.Deg2Rad * (angle);
        // Debug.Log($"angleDeg{angleDeg}");
        int k = -200;
        image.transform.localPosition = new Vector3(k * Mathf.Sin(angleDeg), k * Mathf.Cos(angleDeg), 0);
        image.GetComponentInChildren<Text>().text = text;
        titleList.Add(image.gameObject);
    }

    void Spawn2(float angle, float rate, Color color, string text)
    {
        Debug.Log(rate);
        if (itemList.Count == 0)
        {
            return;
        }
        Image image = Instantiate(imagePrefab2, Roulette, false);
        image.transform.rotation = Quaternion.Euler(0, 0, angle);
        // float angleDeg = Mathf.Deg2Rad * (180 / itemList.Count + angle);
        float angleDeg = Mathf.Deg2Rad * (angle);
        image.transform.localPosition = new Vector3(GetSign(Mathf.Cos(angleDeg)) * 150, GetSign(Mathf.Sin(angleDeg)) * 150, 0);
        image.color = color;
        image.GetComponentInChildren<Text>().text = text;
    }

    float GetSign(float value)
    {
        if (Mathf.Abs(value) < 0.0001f)
        {
            return 0;
        }
        return Mathf.Sign(value);
    }
    void Spawn4(float angle, float rate, Color color, string text)
    {
        Debug.Log(rate);

        if (itemList.Count == 0)
        {
            return;
        }
        Image image = Instantiate(imagePrefab4, Roulette, false);
        image.transform.rotation = Quaternion.Euler(0, 0, angle);
        float angleDeg = Mathf.Deg2Rad * (180 / itemList.Count + angle);
        // image.transform.localPosition = new Vector3(GetSign(Mathf.Cos(angleDeg)) * 150, GetSign(Mathf.Sin(angleDeg)) * 150, 0);
        image.transform.localPosition = new Vector3(Mathf.Sign(Mathf.Cos(angleDeg)) * 150, Mathf.Sign(Mathf.Sin(angleDeg)) * 150, 0);
        image.color = color;
        image.GetComponentInChildren<Text>().text = text;
    }
}