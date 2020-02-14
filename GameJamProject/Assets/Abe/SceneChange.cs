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

    [SerializeField]
    private string ChangeSceneName;

    // Start is called before the first frame update
    void Start()
    {
        CurrntScene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrntScene.name == "MainGameStage")
            return;

        if (CurrntScene.name == "ResultScene")
        {
            //タイトルへ遷移
        }
    }
}
