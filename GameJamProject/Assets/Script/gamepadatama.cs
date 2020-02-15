using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamepadatama : MonoBehaviour
{
    [SerializeField] private float PlayerSpeed;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        Transform myTransform = this.transform;

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
        }

        //R stick
        float rsh = Input.GetAxis("R_Stick_H");
        float rsv = Input.GetAxis("R_Stick_V");
            Debug.Log("Gamepadatama Rstick:" + rsh+ "," + rsv);
            Vector3 test2 = transform.position;
            this.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(rsh, rsv) * Mathf.Rad2Deg, new Vector3(0, 0, 1));
            test2 += new Vector3(rsh, rsv, 0) * PlayerSpeed * Time.deltaTime;

        transform.position = test2;
    }
}
