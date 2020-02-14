using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletGEne : MonoBehaviour
{
    // 弾オブジェクトのRigidbody2Dの入れ物
    private Rigidbody2D rb2d;
    // 弾オブジェクトの移動係数（速度調整用）
    [SerializeField] // Inspectorで操作できるように属性を追加します
    private float bulletSpeed;
    [SerializeField] // Inspectorで操作できるように属性を追加します
    private float span;
    // bullet prefab
    public GameObject Bullet;
    float delta = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.delta += Time.deltaTime;
        if (this.delta > span)
        {
            // 弾丸の複製
            GameObject Bullets = Instantiate(Bullet) as GameObject;
            rb2d = Bullets.GetComponent<Rigidbody2D>();
            this.delta = 0;
            // 弾オブジェクトの移動関数
            BulletMove();
        }
    }
    // 弾オブジェクトの移動関数
    void BulletMove()
    {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        // 弾オブジェクトの移動量ベクトルを作成（数値情報）
        Vector2 BulletMovement = new Vector2(hori, vert).normalized;
        // Rigidbody2D に移動量を加算する
        rb2d.velocity += BulletMovement * bulletSpeed;
    }
    // ENEMYと接触したときの関数
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Playerに弾が接触したら弾は消滅する
        if (collision.gameObject.tag == "player")
        {
            Destroy(gameObject);
        }
    }

}
