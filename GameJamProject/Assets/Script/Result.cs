using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    [SerializeField]
    private GameObject Winner;
    [SerializeField]
    private Sprite Win1, Win2;
    // Start is called before the first frame update
    void Start()
    {
        var T = PlayerPrefs.GetInt("Win");
        if(T==1)
        {
            Winner.gameObject.GetComponent<SpriteRenderer>().sprite = Win1;
        }
        else if(T==2)
        {
            Winner.gameObject.GetComponent<SpriteRenderer>().sprite = Win2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
