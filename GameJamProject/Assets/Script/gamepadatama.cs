using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamepadatama : MonoBehaviour
{
    [SerializeField] private float PlayerSpeed;
    [SerializeField]
    private int InputKeyNumber;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.GetIncetance().GameStartFlag)
            return;
        switch (InputKeyNumber)
        {
            case 1:
                Player1Move();
                break;
            case 2:
                Player2Move();
                break;
        }

        ////R stick
        //float rsh = Input.GetAxis("R_Stick_H");
        //float rsv = Input.GetAxis("R_Stick_V");
        //Debug.Log("Gamepadatama Rstick:" + rsh + "," + rsv);
        //Vector3 test2 = transform.position;
        //if (rsh != 0 || rsh != 0)
        //{
        //    this.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(rsh, rsv) * -Mathf.Rad2Deg, new Vector3(0, 0, 1));
        //}
    }

    void Player1Move()
    {
        float rsh = Input.GetAxis("GamePad1_RX");
        float rsv = Input.GetAxis("GamePad1_RY");
        Vector3 test2 = transform.position;
        if (rsh != 0 || rsh != 0)
        {
            this.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(rsh, rsv) * -Mathf.Rad2Deg, new Vector3(0, 0, 1));
        }
    }

    void Player2Move()
    {
        float rsh = Input.GetAxis("GamePad2_RX");
        float rsv = Input.GetAxis("GamePad2_RY");
        Vector3 test2 = transform.position;
        if (rsh != 0 || rsh != 0)
        {
            this.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(rsh, rsv) * -Mathf.Rad2Deg, new Vector3(0, 0, 1));
        }
    }

}
