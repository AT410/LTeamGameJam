using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public List<GameObject> Objects;

    // Start is called before the first frame update
    void Start()
    {
        SetPlayerObject();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    IEnumerable GenerateBomb()
    {
        return new WaitForSeconds();
    }
}
