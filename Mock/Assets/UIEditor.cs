using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

public enum UISetType
{
    Title,
    Load,
    DataSelect,
    AreaSelect,
    GameStage,
    Ending
}

public class UIEditor : EditorWindow
{
    UISetType SetType;

    bool IsNewCreate = false;

    string FilePath = "";
    string FileName = "";

    [MenuItem("MakeEditor/UIEditor")]
    private static void CreateWindow()
    {
        GetWindow<UIEditor>();
    }

    private void Awake()
    {
        FilePath = FilePath = Application.dataPath + "/GenerateUISetMap/";
    }

    public void OnGUI()
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
            SetType = (UISetType)EditorGUILayout.EnumPopup("UISet", SetType);
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
                    FilePath = Application.dataPath + "/GenerateUISetMap/";
                    FileName = GUILayout.TextField(FileName);

                    if (GUILayout.Button("...", GUILayout.Width(30.0f)))
                    {
                        string FullPath = EditorUtility.OpenFilePanel("書き出しファイルを指定する", Application.dataPath, "xml");
                        FilePath= System.IO.Path.GetDirectoryName(FullPath);
                        FileName = System.IO.Path.GetFileNameWithoutExtension(FullPath);
                    }
                }
                else
                {
                    if (GUILayout.Button("..."))
                    {
                        FilePath = EditorUtility.OpenFilePanel("書き出しファイルを指定する", Application.dataPath, "xml");
                    }
                }
            }
            using (new GUILayout.HorizontalScope(GUI.skin.box))
            {
                GUI.backgroundColor = Color.magenta;
                // 書き込みボタン
                if (GUILayout.Button("書き込み"))
                {
                    WriteUI();
                }
                GUI.backgroundColor = defaultColor;
            }
        }
    }

    private void WriteUI()
    {
        //新規作成の場合
        if (IsNewCreate)
        {
            //新規作成
            //保存ファイルパスを作成しシリアライズ化する。
            UIRoot rot = new UIRoot();
            rot = Util.CreateNewUISet(SetType);
            FilePath += FileName + ".xml";
            XmlUtil.Seialize<UIRoot>(FilePath, rot, FileMode.Create);

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
            UIRoot result = new UIRoot();
            result = XmlUtil.Deserialize<UIRoot>(FilePath);
            //新たなデータを追加してシリアライズ化
            Util.AddUISet(SetType, ref result);
            XmlUtil.Seialize<UIRoot>(FilePath, result);
        }
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("通知", "UIMapの書き出しに成功しました", "閉じる");
    }
}
