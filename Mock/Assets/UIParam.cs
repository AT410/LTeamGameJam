using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;      //!< デプロイ時にEditorスクリプトが入るとエラーになるので UNITY_EDITOR で括ってね！
#endif

public enum UIType
{
    Normal,
    Flashing,
    Fade,
}

public enum GameEvent
{
    ToTitleStage,
    ToDataSelectStage,
    ToAreaSelectStage,
    ToLoadStage,
    ToGameStage,
    ToEndingStage
}

public class UIParam : MonoBehaviour
{
    public UIType type;

    public string TexKey;

    public GameEvent Event;

    public string MyIndexKey;

    public string Upkey="", DownKey="", LeftKey="", RightKey="";

    //追加
    public int StageNum = -1, AreaNum = -1;

    /* ---- ここから拡張コード ---- */
#if UNITY_EDITOR
    /**
     * Inspector拡張クラス
     */
    [CustomEditor(typeof(UIParam))]               //!< 拡張するときのお決まりとして書いてね
    public class CharacterEditor : Editor           //!< Editorを継承するよ！
    {
        bool folding = false;

        public override void OnInspectorGUI()
        {
            // target は処理コードのインスタンスだよ！ 処理コードの型でキャストして使ってね！
            UIParam param = target as UIParam;

            /* -- カスタム表示 -- */

            // -- クラスタイプ --
            Color defaultColor = GUI.backgroundColor;
            Color defaultContentColor = GUI.contentColor;
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUI.backgroundColor = new Color(0.75f, 0.75f, 0.75f, 1.0f);
                using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    //GUI.contentColor = Color.white;
                    EditorGUILayout.LabelField("UI基本設定値");
                    //GUI.contentColor = defaultContentColor;
                }
                GUI.backgroundColor = defaultColor;
                param.type = (UIType)EditorGUILayout.EnumPopup("クラスタイプ", param.type);
                param.TexKey = EditorGUILayout.TextField("テクスチャキー", param.TexKey);
                param.Event = (GameEvent)EditorGUILayout.EnumPopup("イベント遷移先", param.Event);

                // 折りたたみ表示
                folding = EditorGUILayout.Foldout(folding, "インデックス");

                if (folding)
                {
                    // リスト表示
                    param.MyIndexKey = EditorGUILayout.TextField("Myインデックスキー", param.MyIndexKey);
                    param.Upkey = EditorGUILayout.TextField("上方向UIインデックス", param.Upkey);
                    param.DownKey = EditorGUILayout.TextField("下方向UIインデックス", param.DownKey);
                    param.LeftKey = EditorGUILayout.TextField("左方向UIインデックス", param.LeftKey);
                    param.RightKey = EditorGUILayout.TextField("右方向UIインデックス", param.RightKey);
                }

            }

            // -- ゲームイベント時の追加設定 --
            if (param.Event == GameEvent.ToGameStage)
            {
                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                {

                    GUI.backgroundColor = new Color(0.75f, 0.75f, 0.75f, 1.0f);
                    using (new GUILayout.VerticalScope(EditorStyles.toolbar))
                    {
                        EditorGUILayout.LabelField("遷移先追加情報");
                    }
                    GUI.backgroundColor = defaultColor;

                    AreaEnum temp = AreaEnum.Area1;
                    param.AreaNum = (int)(AreaEnum)EditorGUILayout.EnumPopup("エリアセレクト", temp);
                    StageEnum temps = StageEnum.Stage1;
                    param.StageNum = (int)(StageEnum)EditorGUILayout.EnumPopup("ステージセレクト", temps);
                }
            }
        }
    }
#endif
}
