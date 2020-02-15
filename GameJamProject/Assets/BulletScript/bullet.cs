﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // 弾オブジェクト（Inspectorでオブジェクトを指定）
    [SerializeField] // Inspectorで操作できるように属性を追加します
    private GameObject Bullet;
    float delta;

    public GameObject effectPrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;
        if (delta >= 2)
        {
            Destroy(gameObject);
        }
    }
    // Playerと接触したときの関数
    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    // Playerに弾が接触したら弾は消滅する
    //    if (collision.gameObject.tag == "player")
    //    {
    //        Destroy(gameObject);
    //        Debug.Log("敵と接触した！");
    //    }
    //}
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            Destroy(gameObject);

            foreach (ContactPoint2D contactPoint2D in other.contacts)
            {
                GameObject effect = (GameObject)Instantiate(effectPrefab, (Vector3)contactPoint2D.point, Quaternion.identity);

                // 衝突位置を確認してみる。
                print((Vector3)contactPoint2D.point);
                Destroy(effect, 1.5f);
            }
        }
    }
}
