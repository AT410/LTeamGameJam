using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class MapEditor : EditorWindow
{
    string[] Area = { "Area1", "Area2" };
    string[] Stage = { "Stage1", "Stage2", "Stage3", "Stage4","Stage5" };

    int AreaNumber = 0;
    int StageNumber = 0;

    string FilePath = "";
    string FileName = "";
    string[] FileFilter = { "xml" };

    bool IsNewCreate;

    [MenuItem("MakeEditor/MapEditor")]
    private static void CreateWindow()
    {
        GetWindow<MapEditor>("TEST");
        
    }

    private void Awake()
    {
        FilePath = Application.dataPath + "/GenerateMap/";
        FileName = "DefaultPath";
        IsNewCreate = true;
    }

    private void OnGUI()
    {
        Color defaultColor = GUI.backgroundColor;
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            GUI.backgroundColor = Color.gray;
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("設定");
            }
            GUI.backgroundColor = defaultColor;
            AreaNumber = EditorGUILayout.Popup("Area", AreaNumber, Area);
            StageNumber = EditorGUILayout.Popup("Stage", StageNumber, Stage);
            IsNewCreate = EditorGUILayout.Toggle("新規作成", IsNewCreate);
        }

        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            GUI.backgroundColor = Color.gray;
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("パス");
            }
            GUI.backgroundColor = defaultColor;

            GUILayout.TextArea(FilePath);
            using (new GUILayout.HorizontalScope(GUI.skin.box))
            {
                if (IsNewCreate)
                {
                    FilePath = Application.dataPath + "/GenerateMap/";
                    FileName = GUILayout.TextField(FileName);
                }
                else
                {
                    if (GUILayout.Button("..."))
                    {
                        FilePath = EditorUtility.OpenFilePanel("Test", Application.dataPath, ".xml");
                    }
                }
            }
            using (new GUILayout.HorizontalScope(GUI.skin.box))
            {
                GUI.backgroundColor = Color.magenta;
                // 書き込みボタン
                if (GUILayout.Button("書き込み"))
                {
                    Write();
                }
                GUI.backgroundColor = defaultColor;
            }
        }
    }

    private void Write()
    {
        //新規作成の場合
        if (IsNewCreate)
        {
            //新規作成
            //保存ファイルパスを作成しシリアライズ化する。
            root rot = new root();
            rot = Util.DataSet(AreaNumber, StageNumber);
            FilePath += FileName+".xml";
            XmlUtil.Seialize<root>(FilePath,rot,FileMode.Create);

        }
        else
        {
            //追記処理
            if (FilePath == null)
                return;
            if (!File.Exists(FilePath))
            {
                EditorUtility.DisplayDialog("Warning", "フォルダがありません", "OK");
                return;
            }
            //対応ファイルのマップデータをデシリアライズ
            root result = new root();
            result = XmlUtil.Deserialize<root>(FilePath);
            //新たなデータを追加してシリアライズ化
            Util.AddData(AreaNumber, StageNumber, ref result);
            XmlUtil.Seialize<root>(FilePath, result);
        }
        AssetDatabase.Refresh();
    }
}
