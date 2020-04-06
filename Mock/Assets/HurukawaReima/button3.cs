using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button3 : MonoBehaviour
{
    bool button3active = false;
    public GameObject Ice2Prehub;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(button3active==true)
        {
            Instantiate(Ice2Prehub);
            button3active = false;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            button3active = !button3active;
        }
    }
}
