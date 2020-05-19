using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject AtObj;

    // Start is called before the first frame update
    void Start()
    {
        if(AtObj)
        {
            this.gameObject.transform.LookAt(AtObj.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
