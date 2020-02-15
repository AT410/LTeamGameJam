using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // 弾オブジェクト（Inspectorでオブジェクトを指定）
    [SerializeField] // Inspectorで操作できるように属性を追加します
    private GameObject Bullet;

    public GameObject ParentObj;

    float delta;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.GetIncetance().GameStartFlag)
            return;

        delta += Time.deltaTime;
        if (delta >= 1)
        {
            Destroy(gameObject);
        }
    }

    // Playerと接触したときの関数
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Playerに弾が接触したら弾は消滅する
        if (collision.gameObject.tag == "player")
        {
            if (collision.gameObject == ParentObj)
                return;
            Destroy(gameObject);
            int num = collision.gameObject.GetComponent<gamepad>().InputKeyNumber;
            GameManager.GetIncetance().DelHealth(num);
            Debug.Log("敵と接触した！");
        }

        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
            Debug.Log("壁と接触した！");
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Playerに弾が接触したら弾は消滅する
        if (collision.gameObject.tag == "player")
        {
            if (collision.gameObject == ParentObj)
                return;
            Destroy(gameObject);
            GameManager.GetIncetance().DelHealth(1);
            Debug.Log("敵と接触した！");
        }

        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
            Debug.Log("壁と接触した！");
        }
    }
}
