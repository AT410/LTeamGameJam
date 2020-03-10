using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class push : MonoBehaviour
{
    bool Buttonactive = false;
    public GameObject fire;
    public GameObject DeletStage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            fire.SetActive(!fire.activeSelf);
            Debug.Log("Fire:"+fire.activeSelf);

            DeletStage.SetActive(!DeletStage.activeSelf);
            Debug.Log("DeletStage:"+DeletStage.activeSelf);
        }
    }
}
