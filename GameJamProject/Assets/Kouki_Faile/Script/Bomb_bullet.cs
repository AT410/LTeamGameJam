using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_bullet : MonoBehaviour
{
    float elapsedtime;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Quaternion RandomRot = Random.rotation;
        elapsedtime = Time.deltaTime;
        var angle = transform.eulerAngles;
        var anglez = angle.z * (Mathf.PI / 180.0f);
        Vector3 dir = new Vector3(Mathf.Cos(anglez), Mathf.Sin(anglez), 0.0f);
        transform.position += new Vector3(transform.position.x ,transform.position.y, transform.position.z);
    }
}
