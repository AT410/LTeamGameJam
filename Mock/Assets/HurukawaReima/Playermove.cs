using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove : MonoBehaviour
{
    public int PlayerSpeed;
    public int PlayerJamp;
    public int Okuyuki;
    public int Okuyukisuu;
    int OkuyukiMAX;
    int OkuyukiMIN = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float PosX = this.gameObject.transform.position.x;
        float PosY = this.gameObject.transform.position.y;
        float PosZ = this.gameObject.transform.position.z;
        OkuyukiMAX = Okuyuki * Okuyukisuu;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            PosX -= PlayerSpeed*Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            PosX += PlayerSpeed*Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            PosY += PlayerJamp * Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if (PosZ < OkuyukiMAX)
            {
                PosZ += Okuyuki;
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (PosZ > OkuyukiMIN)
            {
                PosZ -= Okuyuki;
            }
        }
        this.gameObject.transform.position = new Vector3(PosX,PosY,PosZ);
    }
}
