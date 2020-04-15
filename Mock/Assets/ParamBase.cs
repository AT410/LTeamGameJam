using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamBase : MonoBehaviour
{
    public string Type;
    public string MeshKey="DEFAULT_CUBE";
    public string TexKey="TEST_TX";
    public List<string> Tags;

    public bool SharedActive = false;
    public string SharedKey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
