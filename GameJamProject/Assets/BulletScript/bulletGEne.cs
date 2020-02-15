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
    [SerializeField]
    private int InputKeyNumber;

    // bullet prefab
    public GameObject Bullet;
    float delta = 0;
    //add
    public AudioSource soundSE01;
    public AudioSource soundSE02;

    // Start is called before the first frame update
    void Start()
    {
        //add
        AudioSource[] audioSources = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.GetIncetance().GameStartFlag)
            return;

        switch (InputKeyNumber)
        {
            case 1:
                Player1Shot();
                break;
            case 2:
                Player2Shot();
                break;
        }

        /*
        Vector2 p1 = transform.position;  //Bulletの中心座標
        Vector2 p2 = this.player.transform.position; //playerの中心座標

        Bulletの座標をくちにする
        p1.x = this.player.transform.position.x;
        p1.y = this.player.transform.position.y;
        transform.position = p1;



        float hori = Input.GetAxis("R_Stick_H");
        float vert = Input.GetAxis("R_Stick_V");
        if (hori != 0 || vert != 0)
        {
            this.delta += Time.deltaTime;
            if (this.delta > span)
            {
                this.delta = 0;
                 弾丸の複製
                GameObject Bullets = Instantiate(Bullet) as GameObject;
                soundSE01.PlayOneShot(soundSE02.clip);
                Bullets.transform.position = p2; //FireBallをbossの座標に位置する（くち）
                Bullets.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                Bullets.GetComponent<bullet>().ParentObj = this.gameObject;

                rb2d = Bullets.GetComponent<Rigidbody2D>();
                 弾オブジェクトの移動関数
                BulletMove();
            }
        }
        else
        {
            delta = 0;
        }
        add
        test();
        */
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

    void Player1Shot()
    {
        float hori = Input.GetAxis("GamePad1_RX");
        float vert = Input.GetAxis("GamePad1_RY");
        if (hori != 0 || vert != 0)
        {
            this.delta += Time.deltaTime;
            if (this.delta > span)
            {
                this.delta = 0;
                // 弾丸の複製
                GameObject Bullets = Instantiate(Bullet) as GameObject;
                soundSE01.PlayOneShot(soundSE02.clip);
                Bullets.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                Bullets.GetComponent<bullet>().ParentObj = this.gameObject;

                rb2d = Bullets.GetComponent<Rigidbody2D>();
                // 弾オブジェクトの移動関数
                Vector2 BulletMovement = new Vector2(hori, vert).normalized;
                // Rigidbody2D に移動量を加算する
                rb2d.velocity += BulletMovement * bulletSpeed;
            }
        }
        else
        {
            delta = 0;
        }

    }

    void Player2Shot()
    {
        float hori = Input.GetAxis("GamePad2_RX");
        float vert = Input.GetAxis("GamePad2_RY");
        if (hori != 0 || vert != 0)
        {
            this.delta += Time.deltaTime;
            if (this.delta > span)
            {
                this.delta = 0;
                // 弾丸の複製
                GameObject Bullets = Instantiate(Bullet) as GameObject;
                Bullets.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                Bullets.GetComponent<bullet>().ParentObj = this.gameObject;


                rb2d = Bullets.GetComponent<Rigidbody2D>();
                // 弾オブジェクトの移動関数
                Vector2 BulletMovement = new Vector2(hori, vert).normalized;
                // Rigidbody2D に移動量を加算する
                rb2d.velocity += BulletMovement * bulletSpeed;
                soundSE01.PlayOneShot(soundSE02.clip);
            }
        }
        else
        {
            delta = 0;
        }

    }

    //Playerと接触したときの関数
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Playerに弾が接触したら弾は消滅する
        if (collision.gameObject.tag == "player")
        {
            Destroy(collision.gameObject);
            //add
            soundSE01.PlayOneShot(soundSE01.clip);
        }
    }
    //add
    //void test()
    //{
    //    if (Input.GetKeyDown(KeyCode.W))
    //    {
    //        soundSE01.PlayOneShot(soundSE01.clip);
    //    }
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        soundSE01.PlayOneShot(soundSE02.clip);
    //    }
    //}
}
