using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Netu : MonoBehaviour
{
    float StayTime;
    public GameObject storn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    void TriggerEnter(Collision collision)
    {
        StayTime += Time.deltaTime;
        float PosY = storn.gameObject.transform.position.y;
        if (collision.gameObject.tag =="Player")
        {
            if (StayTime>4)
            {
                PosY = 5.0f;
                storn.gameObject.transform.position = new Vector3(storn.gameObject.transform.position.x, PosY, storn.gameObject.transform.position.z);
            }
        }
        Debug.Log(StayTime);
    }
}
