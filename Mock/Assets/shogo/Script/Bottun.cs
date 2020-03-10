using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottun : MonoBehaviour
{
    public GameObject gameobject;

    // Start is called before the first frame update
    void Start()
    {
        //Reseaver 内の number という変数を取得する
        //Vector3 M_Pos = gameobject.GetComponent<MoveCont>().MovePos;

        //Reseaver 内の　ReturnAccess という関数を使用する
        gameobject.GetComponent<MoveCont>().ReturnAccess();


    }



    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (layerName == "Omori")
        {
            gameobject.GetComponent<MoveCont>().Move();

        }
    }

}
