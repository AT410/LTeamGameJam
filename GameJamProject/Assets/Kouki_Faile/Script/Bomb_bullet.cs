﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_bullet : MonoBehaviour
{
    float elapsedtime;
    public float speed;
    private Vector2 lastvelocity;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    public float destroytime;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void BulletReflection()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            int num = collision.gameObject.GetComponent<gamepad>().InputKeyNumber;
            GameManager.GetIncetance().DelHealth(num);
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 refrectVec = Vector2.Reflect(this.lastvelocity, collision.contacts[0].normal);
        rb.velocity = refrectVec;

        if (collision.gameObject.tag == "player")
        {
            Destroy(gameObject);
        }
        
    }
    private void FixedUpdate()
    {
        lastvelocity = rb.velocity;
        elapsedtime = Time.deltaTime;
        var angle = transform.eulerAngles;
        var anglez = angle.z * (Mathf.PI / 180.0f);
        Vector3 dir = new Vector3(Mathf.Cos(anglez), Mathf.Sin(anglez), 0.0f);
        rb.AddForce(dir * speed);
        Destroy(gameObject, destroytime);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
