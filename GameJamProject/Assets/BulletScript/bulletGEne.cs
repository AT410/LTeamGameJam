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

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.Find("Player"); //追加
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 p1 = transform.position;  //Bulletの中心座標
        Vector2 p2 = this.player.transform.position; //playerの中心座標

        //Bulletの座標をくちにする
        p1.x = this.player.transform.position.x;
        p1.y = this.player.transform.position.y;
        transform.position = p1;

        this.delta += Time.deltaTime;
        if (this.delta > span)
        {
            this.delta = 0;
            // 弾丸の複製
            GameObject Bullets = Instantiate(Bullet) as GameObject;
            Bullets.transform.position = p2; //FireBallをbossの座標に位置する（くち）
            Bullets.transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            rb2d = Bullets.GetComponent<Rigidbody2D>();
            // 弾オブジェクトの移動関数
            BulletMove();
        }
    }
    // 弾オブジェクトの移動関数
    void BulletMove()
    {
        float hori = Input.GetAxis("R_Stick_H");
        float vert = Input.GetAxis("R_Stick_V");

        // 弾オブジェクトの移動量ベクトルを作成（数値情報）
        Vector2 BulletMovement = new Vector2(hori, vert).normalized;
        // Rigidbody2D に移動量を加算する
        rb2d.velocity += BulletMovement * bulletSpeed;
    }
    //Playerと接触したときの関数
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Playerに弾が接触したら弾は消滅する
        if (collision.gameObject.tag == "player")
        {
            Destroy(collision.gameObject);
        }
    }
}
