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
    [SerializeField] GameObject staminaIconPrefab = default;
    [SerializeField] Transform staminaIconPanel = default;
    [SerializeField] GameObject SettingBG = default;
    [SerializeField] GameObject SoundOffFlog = default;
    [SerializeField] GameObject SoundOnFlog = default;
    

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
    public float rotationTime = 0f;//減速なしで回り続ける時間
    public bool isIkasama;//イカサマモード切り替え
    public int stamina;

    //挙動修正の為
    public float gennsoku1 = 0.996f;
    public float gennsoku2 = 0.999f;
    public float gensokuSpeed = -0.6f;
    public float stopSpeed = -0.2f;
    public float ikasmaStopSpeed = -0.4f;




    //カラー用インデックス
    private int idx = 0;
    private float[] ikasamas = { 0, 0 };
    private float between;
    private bool isBGM = false;
    int isSound;

    private void Awake()
    {
        if(I == null)
        {
            I = this;
        }
    }

    private void Start()
    {
        if (isSound == 1)
        {
            SoundOffFlog.SetActive(false);
            SoundOnFlog.SetActive(true);
        }
        else
        {
            SoundOffFlog.SetActive(true);
            SoundOnFlog.SetActive(false);
        }

        SettingBG.SetActive(false);
        colorPanel.SetActive(false);
        setBG.SetActive(false);
        stamina = PlayerPrefs.GetInt("STAMINA", 0); //セーブの為
        isSound = PlayerPrefs.GetInt("SOUND", 1); //セーブの為

        //カラーパレットの色をリストに追加
        foreach (Transform tf in colorPanelBtn.transform)
        {
            colorList.Add(new Color(tf.GetComponent<Image>().color.r, tf.GetComponent<Image>().color.g, tf.GetComponent<Image>().color.b));
        }
        //スタミナ表示
        for(int i = 0; i < stamina; i++)
        {
            GameObject gameObject = Instantiate(staminaIconPrefab, staminaIconPanel, false);
        }
    }

    private void Update()
    {
        
        if (isRouletteStart)
        {
        
            Roulette.transform.Rotate(0, 0, rotSpeed);//加速
            rotationTime -= Time.deltaTime;
            if(isSound == 1 && !isBGM )
            {
                isBGM = true;
                AudioManager.I.RouletteSound();
            }

            if (rotationTime <= 0 && rotSpeed < gensokuSpeed)
            {
                rotSpeed *= gennsoku1;//減速
            }
            if (rotationTime <= 0 && gensokuSpeed < rotSpeed )  //急に止まると不自然な為２段階減速
            {
                rotSpeed *= gennsoku2;//減速
            }

        }
        if (isIkasama)
        {
            IkasamaStop();
            if (isSound == 1)
            {
                AudioManager.I.ResultSound();
            }
        }
        if (rotSpeed >= stopSpeed && isRouletteStart)
        {
            rotSpeed = 0;
            result();
            if (isSound == 1)
            {
                AudioManager.I.ResultSound();
            }
        }
        
    }

    //回転
    public void RotationBtn()
    {
        if (!isRouletteStart && itemList.Count > 0)
        {
            isRouletteStart = true;
            rotationTime = Random.Range(3.0f, 6.0f); //減速なしで回り続ける時間
            rotSpeed = -6.5f;
        }
    }

    public void SetIkasama(string ikasamaText)
    {
        /*クリックされたテキストとitemのテキストを比較する処理*/
        foreach(var (item,idx) in itemList.Select((item, idx) => (item, idx)))
        {
            if(item.GetText() == ikasamaText)
            {
                
                ikasamas[0] = circles[idx].startAngle;
                ikasamas[1] = circles[idx].endAngle;
                break;
            }
        }
        
        between = Random.Range((ikasamas[1] + ikasamas[0]) / 2f, ikasamas[1]);
        Debug.Log($"イカサマ！{ikasamas[0]}から{ikasamas[1]}の間：結果は{ikasamaText}");
    }

    //イカサマ
    public void IkasamaStop()
    {
        if(ikasamas[0] <= Roulette.transform.localEulerAngles.z && Roulette.transform.localEulerAngles.z < between && ikasmaStopSpeed < rotSpeed)
        {
            rotSpeed *= 0.9f;
        }
    }

    
    //結果表示
    public void result()
    { 
        float resutAngle = Roulette.transform.localEulerAngles.z;
        foreach(var (item, idx) in itemList.Select((item, idx)=> (item, idx)))
        {
            if(circles[idx].startAngle <= resutAngle && circles[idx].endAngle >= resutAngle)
            {
                resultText.text = $"結果 ： {item.GetText()}";
                isRouletteStart = false;
                if (isIkasama)
                {
                    isIkasama = false;
                    stamina -= 1;
                    Destroy(staminaIconPanel.transform.GetChild(0).gameObject);
                }
                
            }
        }
    }

    //項目追加画面表示
    public void SetMenuBtn()
    {
        if (isRouletteStart)
        {
            return;
        }
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

    //ルーレット生成しルーレット画面へ
    public void SetBtn()
    {
        Roulette.transform.localEulerAngles = new Vector3(0, 0, 0);

        if (itemList.Count <= 0)
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
        colorPanel.SetActive(false);

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

    //スタミナプラス処理
    public void StaminaUpBtn()
    {
        if(stamina < 5)
        {
            stamina += 1;
            GameObject gameObject = Instantiate(staminaIconPrefab, staminaIconPanel, false);
            Debug.Log(stamina);
        }
        else
        {
            return;
        }
    }

    //セーブ
    void OnDestroy()
    {
        // スコアを保存
        PlayerPrefs.SetInt("STAMINA", stamina);
        PlayerPrefs.SetInt("SOUND", isSound);
        PlayerPrefs.Save();
    }

    //セッティング周り
    public void SettingBtn()
    {
        SettingBG.SetActive(true);
    }
    public void SettingReturnBtn()
    {
        SettingBG.SetActive(false);
    }
    public void SoundOnBtn()
    {
        isSound = 1;
        SoundOffFlog.SetActive(false);
        SoundOnFlog.SetActive(true);
    }
    public void SoundOffBtn()
    {
        isSound = 0;
        SoundOffFlog.SetActive(true);
        SoundOnFlog.SetActive(false);
    }

}