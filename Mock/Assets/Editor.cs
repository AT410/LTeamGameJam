using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

public enum AreaEnum
{
    None =-1,
    Area1 = 0,
    Area2,
    Area3
}

public enum StageEnum
{
    None = -1,
    Stage1=0,
    Stage2,
    Stage3
}

public class MapEditor : EditorWindow
{
    AreaEnum areaEnum;
    StageEnum stageEnum;

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
            areaEnum = (AreaEnum)EditorGUILayout.EnumPopup("Area", areaEnum);
            stageEnum = (StageEnum)EditorGUILayout.EnumPopup("Stage", stageEnum);
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
            if (IsNewCreate)
            {
                GUI.backgroundColor = Color.gray;
                using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    GUILayout.Label("ファイル名");
                }
                GUI.backgroundColor = defaultColor;
                using (new GUILayout.HorizontalScope(GUI.skin.box))
                {
                    FilePath = Application.dataPath + "/GenerateMap/";
                    FileName = GUILayout.TextField(FileName);
                    if (GUILayout.Button("...", GUILayout.Width(30.0f)))
                    {
                        FilePath = EditorUtility.OpenFilePanel("書き出しファイルを指定する", Application.dataPath, "xml");
                        FileName = System.IO.Path.GetFileNameWithoutExtension(FilePath);
                        FilePath = Path.GetDirectoryName(FilePath);
                    }
                }
            }
            else
            {
                if (GUILayout.Button("..."))
                {
                    FilePath = EditorUtility.OpenFilePanel("書き出しファイルを指定する", Application.dataPath, "xml");
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
        int AreaNumber = (int)areaEnum;
        int StageNumber = (int)stageEnum;

        bool Success = false;

        //新規作成の場合
        if (IsNewCreate)
        {
            //新規作成
            //保存ファイルパスを作成しシリアライズ化する。
            root rot = new root();
            rot = Util.DataSet(AreaNumber, StageNumber, out Success);
            FilePath += FileName + ".xml";
            if (Success)
            {
                XmlUtil.Seialize<root>(FilePath, rot, FileMode.Create);
            }
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
            Success = Util.AddData(AreaNumber, StageNumber, ref result);
            if (Success)
            {
                XmlUtil.Seialize<root>(FilePath, result);
            }
        }

        AssetDatabase.Refresh();
        if(Success)
        {
            EditorUtility.DisplayDialog("通知", "Mapの書き出しに成功しました", "閉じる");
        }
        else
        {
            EditorUtility.DisplayDialog("Warning", "Mapの書き出しに失敗しました", "閉じる");
        }
    }
}
