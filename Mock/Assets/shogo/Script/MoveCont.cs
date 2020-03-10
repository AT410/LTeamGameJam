using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCont : MonoBehaviour
{
    public Vector3 MovePos;
    public void ReturnAccess()
    {
        Debug.Log("アクセス成功！！");
    }

// Start is called before the first frame update
void Start()
    {
        MovePos = transform.position; //プレイヤーの位置を取得
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(-0.1f, 0, 0);

        //MovePos.x = Mathf.Clamp(MovePos.x, 6.1f, 0.1f); //x位置が常に範囲内か監視
        ////フレームごとに等速で横移動
        //transform.position = new Vector3(MovePos.x, MovePos.y, MovePos.z); //範囲内であれば常にその位置がそのまま入る
    }
    public void Move()
    {
        transform.Translate(-6.0f, 0, 0);

    }
}
