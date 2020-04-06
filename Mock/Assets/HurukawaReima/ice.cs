using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ice : MonoBehaviour
{
    
    public GameObject Ice;
    public GameObject Stage;
    public Netu Netu;
    [SerializeField] private Vector3 localGravity;
    private Rigidbody rBody;
    public bool flg = false;

    // Use this for initialization
    private void Start()
    {
        rBody = this.GetComponent<Rigidbody>();
        rBody.useGravity = false; //最初にrigidBodyの重力を使わなくする
        rBody.constraints = RigidbodyConstraints.FreezeAll;

    }
    void Update()
    {
        float StayTime;
        StayTime = Netu.StayTime;
        Debug.Log(StayTime);
        if (Ice.transform.position.y<-3.10)
        {
            flg = true;
        }
        float hyoumen = Stage.transform.position.y - Ice.transform.localScale.y;
        if (flg == true)
        {
            rBody.useGravity = true;
            rBody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
