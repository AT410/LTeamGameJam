using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;      //!< デプロイ時にEditorスクリプトが入るとエラーになるので UNITY_EDITOR で括ってね！
using UnityEditor.SceneManagement;
#endif

public enum ObjectType
{
    Test,
    Floor,
    Wall,
    Goal,
    Switch,
    Player,
    FireOn,
    FireLine,
    Water,
    WaterDrop,
    Ladder,
    Match,
    Door,
    FloatBox,
    WaterJet,
    WaterLV,
    PullBox
}

public class ParamBase : MonoBehaviour
{
    public ObjectType Type;
    public string MeshKey="DEFAULT_CUBE";
    public string TexKey="TEST_TX";
    public List<string> Tags;

    public bool SharedActive = false;
    public string SharedKey;

    public bool EventActive = false;
    public string EventReceiveKey;

    public string EventSendKey;
    public string EventMsgStr;

    /* ---- ここから拡張コード ---- */
#if UNITY_EDITOR
    /**
     * Inspector拡張クラス
     */
    [CustomEditor(typeof(ParamBase))]               //!< 拡張するときのお決まりとして書いてね
    public class ParamEditor : Editor           //!< Editorを継承するよ！
    {
        bool folding = true;

        public override void OnInspectorGUI()
        {
            // target は処理コードのインスタンスだよ！ 処理コードの型でキャストして使ってね！
            ParamBase param = target as ParamBase;

            /* -- カスタム表示 -- */

            // -- クラスタイプ --
            Color defaultColor = GUI.backgroundColor;
            Color defaultContentColor = GUI.contentColor;
            EditorGUI.BeginChangeCheck();
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUI.backgroundColor = new Color(0.75f, 0.75f, 0.75f, 1.0f);
                using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    //GUI.contentColor = Color.white;
                    EditorGUILayout.LabelField("基本設定値");
                    //GUI.contentColor = defaultContentColor;
                }
                GUI.backgroundColor = defaultColor;
                param.Type = (ObjectType)EditorGUILayout.EnumPopup("クラスタイプ", param.Type);
                param.MeshKey = EditorGUILayout.TextField("メッシュキー", param.MeshKey);
                param.TexKey = EditorGUILayout.TextField("テクスチャキー", param.TexKey);

                // -- タグ情報 --
                List<string> list = param.Tags;
                int i, len = list.Count;

                // 折りたたみ表示
                if (folding = EditorGUILayout.Foldout(folding, "タグ情報"))
                {
                    // リスト表示
                    for (i = 0; i < len; ++i)
                    {
                        list[i] = EditorGUILayout.TextField("タグ" + i.ToString(), list[i]);
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("追加"))
                    {
                        list.Add("");
                    }
                    if(GUILayout.Button("削除"))
                    {
                        if (list.Count != 0)
                        {
                            list.RemoveAt(list.Count-1);
                        }
                    }
                }

                GUI.backgroundColor = new Color(0.75f, 0.75f, 0.75f, 1.0f);
                using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    EditorGUILayout.LabelField("共有設定");
                    GUI.backgroundColor = defaultColor;
                }
                GUI.backgroundColor = defaultColor;

                param.SharedActive = EditorGUILayout.ToggleLeft("書き出し有効",param.SharedActive);

                if(param.SharedActive)
                {
                    param.SharedKey = EditorGUILayout.TextField("共有キー", param.SharedKey);
                }


                GUI.backgroundColor = new Color(0.75f, 0.75f, 0.75f, 1.0f);
                using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    EditorGUILayout.LabelField("イベント設定");
                    GUI.backgroundColor = defaultColor;
                }
                GUI.backgroundColor = defaultColor;

                param.EventActive = EditorGUILayout.ToggleLeft("イベントを受信可能", param.EventActive);

                if (param.EventActive)
                {
                    param.EventReceiveKey = EditorGUILayout.TextField("イベント受信キー", param.EventReceiveKey);
                }

                // --　スイッチオブジェクト時の追加設定 --
                if (param.Type == ObjectType.Switch||param.Type == ObjectType.FireLine)
                {
                    using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                    {

                        GUI.backgroundColor = new Color(0.75f, 0.75f, 0.75f, 1.0f);
                        using (new GUILayout.VerticalScope(EditorStyles.toolbar))
                        {
                            EditorGUILayout.LabelField("イベントメッセージ設定");
                        }
                        GUI.backgroundColor = defaultColor;

                        param.EventSendKey = EditorGUILayout.TextField("イベント送信キー", param.EventSendKey);
                        param.EventMsgStr = EditorGUILayout.TextField("イベントメッセージ", param.EventMsgStr);
                    }
                }

            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Change Inspector");
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            }
        }
    }
#endif
}
