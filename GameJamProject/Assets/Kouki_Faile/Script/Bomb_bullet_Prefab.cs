using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_bullet_Prefab : MonoBehaviour
{
    public GameObject bullet;
    public float speed;
    float elapsedtime;
    float max = 360.0f;
    float min = 0.0f;
    bool Explosion;
    public int bulletCount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bullet" || collision.gameObject.tag == "bomb")
        {
            Explosion = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");

        if(collision.gameObject.tag == "bullet" || collision.gameObject.tag == "bomb")
        {
            Explosion = true;
        }
    }
    void BulletPrefab()
    {
        if (Explosion == true)
        {
            elapsedtime = Time.deltaTime;
            float RandomZ = Random.Range(min, max);
            GameObject bullets = Instantiate(bullet, transform.position, Quaternion.Euler(0.0f, 0.0f, RandomZ)) as GameObject;
            bulletCount -= 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BulletPrefab();
        if (bulletCount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
