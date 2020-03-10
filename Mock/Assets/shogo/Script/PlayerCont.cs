using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCont : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 左に移動
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(-0.1f, 0.0f, 0.0f);
        }
        // 右に移動
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(0.1f, 0.0f, 0.0f);
        }
        // 前に移動
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(0.0f, 0.0f, 0.1f);
        }
        // 後ろに移動
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(0.0f, 0.0f, -0.1f);
        }
        //ジャンプ
        if (Input.GetKey(KeyCode.Space))
        {
            this.transform.Translate(0.0f, 0.2f, 0.0f);
        }
        //下
        if (Input.GetKey(KeyCode.LeftShift))
        {
            this.transform.Translate(0.0f, -0.2f, 0.0f);
        }
    }
}

