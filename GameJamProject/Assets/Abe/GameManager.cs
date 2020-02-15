using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
///　ゲームマネージャークラス
///　<info>
///　ゲーム開始のプレイヤー出現位置決定（障害物なし）
///　
/// </info>
/// </summary>
public class GameManager : MonoBehaviour
{
    static private GameManager Incetance;

    static public GameManager GetIncetance()
    {
        return Incetance;
    }

    private void Awake()
    {
        Incetance = this;
    }


    [SerializeField,Header("出現位置"),Tooltip("出現位置X")]
    private Vector2 HeightRange;
    [SerializeField, Tooltip("出現位置Y")]
    private Vector2 WidthRange;

    [SerializeField, Header("爆弾生成関連"), Tooltip("ボム出現間隔")]
    private float BompGenerateLerp = 5.0f;
    [SerializeField]
    private GameObject BombObject;
    private List<GameObject> BombPool;

    [SerializeField, Header("システム関連"), Tooltip("制限時間")]
    private float TotalTime = 0;
    [SerializeField, Tooltip("TimeUI")]
    private Text TimeUI;
    [SerializeField, Tooltip("開始UI")]
    private List<Sprite> StartUIList,NumberList;
    [SerializeField, Tooltip("開始UI")]
    private Image StartUI;
    [SerializeField, Tooltip("Player1HP")]
    [Range(0.0f,1.0f)]
    private float HP1,HP2,HP3,HP4;

    [SerializeField]
    private List<GameObject> HealthObjects;
    [SerializeField]
    private List<GameObject> PlayerObjects;

    public GameObject Object;

    public float CountTime = 5.0f;

    public bool GameStartFlag =false;

    // Start is called before the first frame update
    void Start()
    {
        TimeUI.enabled = false;
        BombPool = new List<GameObject>();
        SetPlayerObject();
        StartCoroutine("GenerateBomb");
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameStartFlag)
        {
            GameStart();
            return;
        }

        //開始カウントダウン
        //制限時間がなくなったらリザルト画面へ
        if (TotalTime <= 0.0f)
            SceneManager.LoadScene("ResultScene");
        TotalTime -= Time.deltaTime;
        UpdateUI();

        if (HP1 <= 0.0f || HP2 <= 0.0f)
        {
            int Num = HP1 >= HP2 ? 1 : 2;
            PlayerPrefs.SetInt("Win", Num);
            SceneManager.LoadScene("ResultScene");
        }

    }

    //
    void GameStart()
    {
        //五秒間表示
        if(CountTime < 0.0f)
        {
            GameStartFlag = true;
            TimeUI.enabled = true;
            StartUI.enabled = false;
        }
        CountTime -= Time.deltaTime;
        int Key = (int)CountTime;
        StartUI.sprite = StartUIList[Key];
    }

    /// <summary>
    /// プレイ人数に応じたオブジェクト配置
    /// </summary>
    void SetPlayerObject()
    {
        foreach(var test in PlayerObjects)
        {
            Vector3 OutPosition = new Vector3();
            OutPosition.x = Random.Range(WidthRange.x, WidthRange.y);
            OutPosition.y = Random.Range(HeightRange.x, HeightRange.y);
            var Obj = GameObject.Instantiate(test,OutPosition,Quaternion.identity);
            Obj.GetComponent<SpriteRenderer>().color = Color.white;
            HealthObjects.Add(Obj);
        }
    }

    /// <summary>
    /// 爆弾の生成
    /// </summary>
    /// <info>
    /// 生成地点にオブジェクトがあった場合の処理（未実装）
    /// </info>
    /// <returns></returns>
    private IEnumerator GenerateBomb()
    {
        while(true)
        {
            Vector3 OutPosition = new Vector3();
            OutPosition.x = Random.Range(WidthRange.x, WidthRange.y);
            OutPosition.y = Random.Range(HeightRange.x, HeightRange.y);
            BombPool.Add(GameObject.Instantiate(BombObject, OutPosition, Quaternion.identity));
            yield return new WaitForSeconds(BompGenerateLerp);
            if (BompGenerateLerp > 2.0f)
                BompGenerateLerp -= 0.5f;
            if (BombPool.Count == 100)
                yield break;
        }
    }

    private void UpdateUI()
    {
        int mini = (int)(TotalTime / 60.0f);
        int second = (int)(TotalTime % 60.0f);
        TimeUI.text = mini.ToString() +":"+ second.ToString("00");
    }

    public void DelHealth(int PlayerNum)
    {
        Color result = Color.white;

        switch (PlayerNum)
        {
            case 1:
                //体力を減らす
                HP1 -= 0.1f;
                result = Color.Lerp(Color.blue, Color.white, HP1);
                HealthObjects[0].GetComponent<SpriteRenderer>().color = result;
                break;
            case 2:
                HP2 -= 0.1f;
                result = Color.Lerp(Color.red, Color.white, HP2);
                HealthObjects[1].GetComponent<SpriteRenderer>().color = result;
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }
}
