
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamepadmove : MonoBehaviour
{
    //Touch touched = new Touch();
    //public RectTransform canvas;
    //Vector3 touchWorldPos;
    //Vector3 startPos;
    //int range = 100;
    //Vector3 stickPos;
    //public GameObject Player;
    //int speed = 100;

    public Transform verRot;
    public Transform horRot;
    // Start is called before the first frame update
    void Start()
    {
        //startPos = transform.position;

        verRot = GetComponent<Transform>();
        horRot = GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        float X_Rotation = Input.GetAxis("L_Stick_V");
        float Y_Rotation = Input.GetAxis("L_Stick_H");
        verRot.transform.Rotate(0, -X_Rotation, 0);
        horRot.transform.Rotate(0, Y_Rotation, 0);
    }
}
        //if (Input.touchCount > 0)
        //{
        //    for (int i = 0; i < Input.touches.Length; i++)
        //    {
        //        touched = Input.touches[i];
        //        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas, touched.position, null, out touchWorldPos);
        //        if (Vector3.Distance(touchWorldPos, startPos) < range)
        //        {
        //            transform.position = touchWorldPos;
        //            Player.transform.Rotate(transform.right, -speed * Time.deltaTime * ((touchWorldPos.y - startPos.y) / range));
        //            Player.transform.Rotate(transform.up, speed * Time.deltaTime * ((touchWorldPos.x - startPos.x) / range));
        //            print(Player.transform.localRotation);
        //        }
        //    }
        //}
        //else
        //{
        //    transform.position = startPos;
        //}
        //L Stick
        //float lsh = Input.GetAxis("L_Stick_H");
        //float lsv = Input.GetAxis("L_Stick_V");
        //if ((lsh != 0) || (lsv != 0))
        //{
        //}



