using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_bullet_Prefab : MonoBehaviour
{
    public GameObject bullet;
    public float speed;
    float elapsedtime;
    void BulletPrefab()
    {
        elapsedtime = Time.deltaTime;
        Quaternion RandomRot = Random.rotation;
        GameObject bullets = Instantiate(bullet, transform.position, transform.rotation) as GameObject;

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        BulletPrefab();
    }
}
