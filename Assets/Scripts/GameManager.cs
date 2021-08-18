using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{

    //参照出来るようにstatic変数用意
    public static GameManager I;
    /*シリアライズフィールド達*/
    [SerializeField] Transform Roulette = default;
    [SerializeField] Image circlePrefab = default;
    [SerializeField] Image TitlePrefab = default;    
    [SerializeField] Text resultText = default;
    [SerializeField] GameObject setBG = default;
    [SerializeField] GameObject colorPanel = default;
    [SerializeField] GameObject colorPanelBtn = default;
    [SerializeField] Transform setPanel = default;
    [SerializeField] Item itemPrefab = default;

    /*List達*/
    List<Item> itemList = new List<Item>();
    List<GameObject> itemRouletteObjList = new List<GameObject>();
    List<GameObject> titleList = new List<GameObject>();
    List<Circle> circles = new List<Circle>();
    List<Color> colorList = new List<Color>();
    
    /*public変数*/
    public float rotSpeed = 0;
    public float sumRate = 0;
    public GameObject currentColorBtn; //itemのボタン格納用変数（item側でクリックされたらこの中にgameobjectが入る）
    public bool isRouletteStart;
    //カラー用インデックス
    private int idx = 0;

    private void Awake()
    {
        if(I == null)
        {
            I = this;
        }
        
    }

    private void Start()
    {
        setBG.SetActive(false);
        
        //カラーパレットの色をリストに追加
        foreach(Transform tf in colorPanelBtn.transform)
        {
            colorList.Add(new Color(tf.GetComponent<Image>().color.r, tf.GetComponent<Image>().color.g, tf.GetComponent<Image>().color.b));
        }

    }

    

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
            result();
        }
    }

    //回転
    public void RotationBtn()
    {
        isRouletteStart = true;
        rotSpeed = -5f;
    }

    
    //結果表示
    public void result()
    {
        /*Debug.Log("総アイテム数"+itemList.Count);
        Debug.Log($"サークル数:{circles.Count}");
        Debug.Log("静止時アングル" + Roulette.transform.localEulerAngles.z);*/
        
        float resutAngle = Roulette.transform.localEulerAngles.z;
        
        foreach(var (item, idx) in itemList.Select((item, idx)=> (item, idx)))
        {
            if(circles[idx].startAngle <= resutAngle && circles[idx].endAngle >= resutAngle)
            {
                resultText.text = $"結果 ： {item.GetText()}";
                Debug.Log($"結果：{item.GetText()} / index:{idx}");
            }
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

        float startAngle = 0;//スタートの角度
        float endAngle = 0;//終了の角度
        for (int i = 0; i < itemList.Count; i++)
        {
            float rate = itemList[i].rate / sumRate;

            endAngle += 360 * rate;

            Spawn(startAngle,endAngle, rate, itemList[i].GetColor(), itemList[i].GetText());
            TitleSpawn(itemList[i].GetText());
            //次のスタートアングルのためにendAngleをstartAngleに代入
            startAngle = endAngle;
        }
    }



    //項目追加
    public void ItemPlusBtn()
    {
        Item item = Instantiate(itemPrefab, setPanel, false);
        Color color = colorList[idx];
        item.Set(color, 1);
        itemList.Add(item);
        
        idx++;
        //カラーを最初から呼び出すための分岐
        if (idx ==  colorList.Count)
        {
            idx = 0;
        }
    }

    //ルーレット項目リセット処理
    public void ItemResetBtn()
    {
        //カラー呼び出し用indexを初期化
        idx = 0;
        DestroyListObj();
    }


    //色選択画面へ
    //押されたボタンを受け取る currentColorBtn
    public void ColorChoiceBtn(GameObject btn)
    {
        colorPanel.SetActive(true);
        //受け取ったgameObjectを格納
        currentColorBtn = btn;
        
    }


    //色選択画面から戻る
    public void ColorReturnBtn()
    {
        colorPanel.SetActive(false);
    }

    //カラーパネルのボタンを押した処理
    public void SelectColorBtn(Color baseBtnColor)
    {
        //送られたボタンがnullなら処理終了
        if (currentColorBtn == null)
        {
            return;
        }
        currentColorBtn.GetComponent<Image>().color = baseBtnColor;

    }

    //円生成
    void Spawn(float startAngle,float endAngle, float rate, Color color, string text)
    {
        Image image = Instantiate(circlePrefab, Roulette, false);
        image.transform.rotation = Quaternion.Euler(0, 0, 360 - startAngle);
        image.fillAmount = rate;
        circles.Add(image.GetComponent<Circle>());
        circles.Last().startAngle = startAngle;
        circles.Last().endAngle = endAngle;
        image.color = color;
        itemRouletteObjList.Add(image.gameObject);
    }

    //各円の見出し生成
    void TitleSpawn( string text)
    {
        if (itemList.Count == 0)
        {
            return;
        }
        Image image = Instantiate(TitlePrefab, Roulette, false);
        float angleDeg = Mathf.Deg2Rad * ((circles.Last().startAngle + circles.Last().endAngle) / 2);
        int k = 200;//円中心からの何か
        image.transform.localPosition = new Vector3(k * Mathf.Sin(angleDeg), k * Mathf.Cos(angleDeg), 0);
        image.GetComponentInChildren<Text>().text = text;
        titleList.Add(image.gameObject);
    }

 

 
}