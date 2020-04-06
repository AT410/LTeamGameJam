using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gateopen : MonoBehaviour
{

    public GameObject gateright;
    public GameObject gateleft;
    public GameObject stone;
    float StayTime;
    bool active = false;
    public int openspeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float gateRX = gateright.transform.position.x;
        float gateRY = gateright.transform.position.y;
        float gateRZ = gateright.transform.position.z;


        float gateLX = gateleft.transform.position.x;
        float gateLY = gateleft.transform.position.y;
        float gateLZ = gateleft.transform.position.z;
        if (active==true)
        {
            if (gateRZ > -3)
            {
                gateRZ -= openspeed * Time.deltaTime;
            }
            if (gateLZ < 12)
            {
                gateLZ += openspeed * Time.deltaTime;
            }
        }
        gateright.transform.position = new Vector3(gateRX, gateRY, gateRZ);
        gateleft.transform.position = new Vector3(gateLX, gateLY, gateLZ);
    }

    void OnTriggerStay(Collider other)
    {
        if(other.transform.tag=="storn")
        {
            StayTime += Time.deltaTime;
            if (StayTime>4)
            {
                active = true;
                Debug.Log("あく");
            }
        }
    }
}
