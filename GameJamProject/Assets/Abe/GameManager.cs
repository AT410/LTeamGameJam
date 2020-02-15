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
    public List<GameObject> Objects;
    
    private bool StartFlag =true;

    // Start is called before the first frame update
    void Start()
    {
        BombPool = new List<GameObject>();
        SetPlayerObject();
        StartCoroutine("GenerateBomb");
    }

    // Update is called once per frame
    void Update()
    {
        //制限時間がなくなったらリザルト画面へ
        if (TotalTime <= 0.0f)
            SceneManager.LoadScene("ResultScene");
        TotalTime -= Time.deltaTime;
        UpdateUI();
    }

    /// <summary>
    /// プレイ人数に応じたオブジェクト配置
    /// </summary>
    void SetPlayerObject()
    {
        foreach(var test in Objects)
        {
            Vector3 OutPosition = new Vector3();
            OutPosition.x = Random.Range(WidthRange.x, WidthRange.y);
            OutPosition.y = Random.Range(HeightRange.x, HeightRange.y);
            GameObject.Instantiate(test,OutPosition,Quaternion.identity);
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
        while(StartFlag)
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
        TimeUI.text = mini.ToString() +":"+ second.ToString();
    }
}
