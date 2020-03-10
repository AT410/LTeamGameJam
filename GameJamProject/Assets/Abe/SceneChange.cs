using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///　シーン遷移用プログラム
/// </summary>

public class SceneChange : MonoBehaviour
{
    private Scene CurrntScene;

    //add
    private AudioSource soundSE01;

    // Start is called before the first frame update
    void Start()
    {
        CurrntScene = SceneManager.GetActiveScene();

        //add
        soundSE01 = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrntScene.name == "mainGameStage")
        {
            SceneManager.LoadScene(0);
            return;
        }
        if (CurrntScene.name == "TitleScene")
        {
            if(Input.GetKeyDown("joystick button 1"))
            {
                SceneManager.LoadScene("MainGameStage");
                soundSE01.PlayOneShot(soundSE01.clip);
            }
            return;
        }
        if (CurrntScene.name == "ResultScene")
        {
            //タイトルへ遷移
            if (Input.GetKeyDown("joystick button 7"))
            {
                SceneManager.LoadScene(0);
                //add
                soundSE01.PlayOneShot(soundSE01.clip);
            }
            return;
        }

        if (CurrntScene.name == "TitleScene")
        {
            //タイトルへ遷移
            if (Input.GetKeyDown("joystick button 7"))
            {
                SceneManager.LoadScene(1);
            }
            return;
        }
    }
}
