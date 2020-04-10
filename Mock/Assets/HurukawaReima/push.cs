using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class push : MonoBehaviour
{
    bool Buttonactive = false;
    public GameObject fire;
    public GameObject DeletStage;
    bool Onpush = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Onpush == true)
        {
            if (Input.GetKey("joystick button 2"))
                {
                fire.SetActive(!fire.activeSelf);
                }
        }
    }
    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        fire.SetActive(!fire.activeSelf);
    //        Debug.Log("Fire:"+fire.activeSelf);

    //        DeletStage.SetActive(!DeletStage.activeSelf);
    //        Debug.Log("DeletStage:"+DeletStage.activeSelf);
    //    }
    //}
     void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag =="Player")
        {
            Onpush = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        Onpush = false;
    }
}
