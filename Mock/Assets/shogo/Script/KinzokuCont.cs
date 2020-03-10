using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinzokuCont : MonoBehaviour
{
    public GameObject gameobject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (layerName == "Player")
        {
            gameobject.GetComponent<HimoCont>().Dest();

        }
    }

}
