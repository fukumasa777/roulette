using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager: MonoBehaviour
{
    // 画像の生成
    [SerializeField] Image imagePrefab = default;



    [SerializeField] Transform Roulette = default;

    [SerializeField] Item itemPrefab = default;
    [SerializeField] Transform setPanel = default;

    [SerializeField] GameObject setBG = default;

    List<Item> itemList = new List<Item>();
    public float rotSpeed = 0;

    [SerializeField] Arrow arrow = default;

    private void Start()
    {
        setBG.SetActive(false);

        // 生成はできた
        // ３つ配置
        // 角度を120ずつずらす

        Debug.Log("スタート");
        

        

        //List<ItemData> itemList = new List<ItemData>();
        //itemList.Add(new ItemData(Color.red, "りんご", 1));
        //itemList.Add(new ItemData(Color.yellow, "ばなな", 1));
        //itemList.Add(new ItemData(Color.green, "もも", 1));



        // Spawn(360/ 3 * 0, 2/4); => Spawn(0, 0.5f); 180ど埋まっている
        // Spawn(360/ 3 * 1, 1/4); => Spawn(180, 0.25f); 180+90
        // Spawn(360/ 3 * 1, 1/4); => Spawn(270, 0.25f);


        //Spawn(120*0);
        //Spawn(120*1);
        //Spawn(120*2);
    }

    public bool rouletteStart;

    private void Update()
    {
        Roulette.transform.Rotate(0, 0, rotSpeed);
        rotSpeed *= 0.99f;
        if (rotSpeed >= -0.01f && rouletteStart)
        {
            rouletteStart = false;
            Debug.Log("止まる" + arrow.targetName);
        }
    }

    public void SetMenuBtn()
    {
        setBG.SetActive(true);
    }

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
            Debug.Log(itemList[i].rate);

            Debug.Log(rate);

            Spawn(nextAngle, rate, itemList[i].GetColor(), itemList[i].GetText());
            nextAngle += 360 * rate;
            // Debug.Log(itemList[i].GetText());
        }

        
    }

    public void RotationBtn()
    {
        rotSpeed = -10;
        rouletteStart = true;
    }

    public void ItemPlusBtn()
    {
        Item item = Instantiate(itemPrefab,setPanel,false);
        Color color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 1);
        item.Set(color, 1);
        // itemList.Add(new ItemData(Color.red, "りんご", 1));
        itemList.Add(item);
    }


    void Spawn(float angle, float rate ,Color color, string text)
    {
        Image image = Instantiate(imagePrefab, Roulette, false);
        image.transform.rotation = Quaternion.Euler(0, 0, angle);
        image.fillAmount = rate;
        image.color = color;
        //image.GetComponentInChildren<Text>().text = text;
    }

    





}

//public class ItemData
//{
//    public Color color;
//    public string itemName;
//    public int rate;
//    public ItemData(Color color, string itemName, int rate)
//    {
//        this.color = color;
//        this.itemName = itemName;
//        this.rate = rate;
//    }
//}
