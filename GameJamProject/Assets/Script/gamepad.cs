using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamepad : MonoBehaviour
{
    public GameObject Cube;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var speed = 2*Time.deltaTime;
        var PosX = this.gameObject.transform.position.x;
        var PosY = this.gameObject.transform.position.y;
        var PosZ = this.gameObject.transform.position.z;
        Vector3 pos = this.gameObject.transform.position;

        Transform myTransform = this.transform;
        //Vector3 localAngle = myTransform.localEulerAngles;
        //float local_angle_x = localAngle.x;
        //float local_angle_y = localAngle.y;
        //float local_angle_z = localAngle.z;



        if (Input.GetKeyDown("joystick button 0"))
        {
            Debug.Log("button0");
        }
        if (Input.GetKeyDown("joystick button 1"))
        {
            Debug.Log("button1");
        }
        if (Input.GetKeyDown("joystick button 2"))
        {
            Debug.Log("button2");
        }
        if (Input.GetKeyDown("joystick button 3"))
        {
            Debug.Log("button3");
        }
        if (Input.GetKeyDown("joystick button 4"))
        {
            Debug.Log("button4");
        }
        if (Input.GetKeyDown("joystick button 5"))
        {
            Debug.Log("button5");
        }
        if (Input.GetKeyDown("joystick button 6"))
        {
            Debug.Log("button6");
        }
        if (Input.GetKeyDown("joystick button 7"))
        {
            Debug.Log("button7");
        }
        if (Input.GetKeyDown("joystick button 8"))
        {
            Debug.Log("button8");
        }
        if (Input.GetKeyDown("joystick button 9"))
        {
            Debug.Log("button9");
        }
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        if ((hori != 0) || (vert != 0))
        {
            Debug.Log("stick:" + hori + "," + vert);
        }
        //L Stick
        float lsh = Input.GetAxis("L_Stick_H");
        float lsv = Input.GetAxis("L_Stick_V");
        if (lsh==1&&lsv==1)
        {
            //Debug.Log("L stick:" + lsh + "," + lsv);
            transform.Translate(1*Time.deltaTime, 0, 0);
        }
        if (lsh == -1&&lsv==-1)
        {
           //Debug.Log("L stick:" + lsh + "," + lsv);
            transform.Translate(-1 * Time.deltaTime, 0, 0);
        }
        if (lsh >0.4&&lsv>0.4&&lsh != 1&&lsv!=1)
        {
            //Debug.Log("L stick:" + lsh + "," + lsv);
            transform.Translate(1 * Time.deltaTime, 1*Time.deltaTime, 0);
        }
        if (lsh > -0.4 && lsv > -0.4 && lsh != -1 && lsv != -1)
        {
            //Debug.Log("L stick:" + lsh + "," + lsv);
            transform.Translate(-1 * Time.deltaTime, -1 * Time.deltaTime, 0);
        }
        //R stick
        float rsh = Input.GetAxis("R_Stick_H");
        float rsv = Input.GetAxis("R_Stick_V");
        if ((rsh != 0) || (rsv != 0))
        {
            Debug.Log("R stick:" + rsh + "," + rsv);

        }
     }
}
