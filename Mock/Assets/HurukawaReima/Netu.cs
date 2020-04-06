using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Netu : MonoBehaviour
{
    public float StayTime;
    public int MAXTIME;
    public GameObject storn;
    bool fall= false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float PosY = storn.gameObject.transform.position.y;
        if (PosY > -3.33&&fall==true)
        {
            PosY -= 1.0f * Time.deltaTime;
            storn.gameObject.transform.position = new Vector3(storn.gameObject.transform.position.x, PosY, storn.gameObject.transform.position.z);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StayTime += Time.deltaTime;
            if (StayTime > MAXTIME)
            {
                fall = true;
            }
        }
    }
}
