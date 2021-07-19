using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject RouletteG = default;
    [SerializeField] Transform Roulette = default;
    [SerializeField] Image imagePrefab2 = default;
    [SerializeField] Image imagePrefab4 = default;
    [SerializeField] Text resultText = default;


    [SerializeField] GameObject setBG = default;
    [SerializeField] Transform setPanel = default;
    [SerializeField] Item itemPrefab = default;


    List<Item> itemList = new List<Item>();

    public float rotSpeed = 0;


    private void Start()
    {
        setBG.SetActive(false);
        Debug.Log("スタート");
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
        if (itemList.Count == 2)
        {
            if (0 < RouletteG.transform.localEulerAngles.z && Roulette.transform.localEulerAngles.z <= 180)
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
        if (itemList.Count == 4)
        {
            if ( 0 < RouletteG.transform.localEulerAngles.z && Roulette.transform.localEulerAngles.z <= 90)
            {
                resultText.text = "結果 ： " + itemList[0].GetText();
                Debug.Log(itemList[0].GetText());
            }
            if (270 < RouletteG.transform.localEulerAngles.z && Roulette.transform.localEulerAngles.z <= 360)
            {
                resultText.text = "結果 ： " + itemList[1].GetText();
                Debug.Log(itemList[1].GetText());
            }
            if (180 < RouletteG.transform.localEulerAngles.z && Roulette.transform.localEulerAngles.z <= 270)
            {
                resultText.text ="結果 ： " + itemList[2].GetText();
                Debug.Log(itemList[2].GetText());
            }
            if (90 < RouletteG.transform.localEulerAngles.z && Roulette.transform.localEulerAngles.z <= 180)
            {
                resultText.text = "結果 ： " + itemList[3].GetText();
                Debug.Log(itemList[0].GetText());
            }
        }
    }

    //項目追加画面表示
    public void SetMenuBtn()
    {
        setBG.SetActive(true);
    }

    //項目追加しルーレット画面へ
    public void SetBtn()
    {
        setBG.SetActive(false);
        float sumRate = 0;
        for (int i = 0; i < itemList.Count; i++)
        {
            sumRate += itemList[i].rate;
        }
        float nextAngle = 0;
        for (int i = 0; i < itemList.Count; i++)
        {
            float rate = itemList[i].rate / sumRate;
            // Spawn(nextAngle, rate, itemList[i].GetColor(), itemList[i].GetText());
            // Debug.Log(itemList[i].GetText());
            if (itemList.Count == 2)
            {
                Spawn2(nextAngle, rate, itemList[i].GetColor(), itemList[i].GetText());

            }
            if (itemList.Count == 4)
            {
                Spawn4(nextAngle, rate, itemList[i].GetColor(), itemList[i].GetText());

            }
            nextAngle += 360 * rate;
        }
    }

    

   //項目追加
    public void ItemPlusBtn()
    {
        Item item = Instantiate(itemPrefab, setPanel, false);
        Color color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 1);
        item.Set(color, 1);
        itemList.Add(item);
        // itemList.Add(new ItemData(Color.red, "りんご", 1));
    }

    public void ItemResetBtn()
    {
        List<Item> itemList = new List<Item>();

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
    void Spawn2(float angle, float rate, Color color, string text)
    {
        if (itemList.Count == 0)
        {
            return;
        }
        Image image = Instantiate(imagePrefab2, Roulette, false);
        image.transform.rotation = Quaternion.Euler(0, 0, angle);
        float angleDeg = Mathf.Deg2Rad * (180 / itemList.Count + angle);
        image.transform.localPosition = new Vector3(Mathf.Sign(Mathf.Sin(angleDeg)) * 150,  0, 0);
        image.color = color;
        image.GetComponentInChildren<Text>().text = text;
    }
    void Spawn4(float angle, float rate, Color color, string text)
    {
        if (itemList.Count == 0)
        {
            return;
        }
        Image image = Instantiate(imagePrefab4, Roulette, false);
        image.transform.rotation = Quaternion.Euler(0, 0, angle);
        float angleDeg = Mathf.Deg2Rad * (180 / itemList.Count + angle);
        image.transform.localPosition = new Vector3(Mathf.Sign(Mathf.Cos(angleDeg)) * 150, Mathf.Sign(Mathf.Sin(angleDeg)) * 150, 0);
        image.color = color;
        image.GetComponentInChildren<Text>().text = text;
    }
}
