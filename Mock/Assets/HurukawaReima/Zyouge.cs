using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zyouge : MonoBehaviour
{
    public button2 button2active;
    bool timing = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float PosY = this.gameObject.transform.position.y;
        float PosX = this.gameObject.transform.position.x;
        float PosZ = this.gameObject.transform.position.z;
            if (button2active.button2active == false&&timing==false)
            {
                if (PosY > -3.33)
                {
                    PosY -= 0.5f * Time.deltaTime;
                }
                else
                {
                    timing = true;
                }
            }
            if (button2active.button2active == true&&timing==true)
            {
                if (PosY < 0)
                {
                    PosY += 0.5f * Time.deltaTime;
                }
                else
                 {
                timing = false;
                 }
            }
        this.gameObject.transform.position = new Vector3(PosX,PosY, PosZ);
    }
}
