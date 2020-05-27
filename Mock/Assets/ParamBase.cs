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
    PullBox,
    FrontWall
}

public enum PlayBackType
{
    Start,
    OnEvent,
    End
}

public enum AnimetionType
{
    Postion = 0,
    Rotation,
    Scale
}

public class AnimParam
{
    public float TargetFlame;

    public Vector3 Vec;

    public AnimParam(float Time, Vector3 vec)
    {
        TargetFlame = Time;
        Vec = vec;
    }
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

    //アニメーション設定
    public bool StartAnimetionActive;
    public bool EventAnimetionActive;
    public bool EndAnimetionActive;

    // -- 開始時アニメーション --
    public List<float> m_MixStartFlame = new List<float>();
    public List<AnimetionType> m_MixStartAnimType = new List<AnimetionType>();
    public List<Vector4> m_StartAnimValue = new List<Vector4>();

    // -- イベント時アニメーション --
    public List<float> m_MixEventFlame = new List<float>();
    public List<AnimetionType> m_MixEventAnimType = new List<AnimetionType>();
    public List<Vector4> m_EventAnimValue = new List<Vector4>();

    // -- イベント時アニメーション --
    public List<float> m_MixEndFlame = new List<float>();
    public List<AnimetionType> m_MixEndAnimType = new List<AnimetionType>();
    public List<Vector4> m_EndAnimValue = new List<Vector4>();

    // -- フレームカウント最大数 --
    public float MaxStartAnimCount;
    public float MaxEventAnimCount;
    public float MaxEndAnimCount;

    // -- アニメーションテスト変数 --
    bool PlayAnimetion;
    int Count = 0;
    public float TotalTime;
    PlayBackType PlayAnime;


    Vector3 StartPos;
    Vector4 StartRot;
    Vector3 StartScal;
    private void Awake()
    {
        return;
    }

    private void Start()
    {
        //m_AnimPos.Add(new AnimParam(0.0f,Vector3.zero));
        Debug.Log(Tags.Count);
        StartPos = this.transform.position;
        var rot = this.transform.rotation;
        StartRot = ConvertToVec4(rot);
        StartScal = this.transform.localScale;
    }

    private void Update()
    {
        if ((StartAnimetionActive||EventAnimetionActive||EndAnimetionActive)&& PlayAnimetion)
        {
            switch(PlayAnime)
            {
                case PlayBackType.Start:
                    AnimeBehavior(m_MixStartFlame, m_MixStartAnimType, m_StartAnimValue);
                    break;
                case PlayBackType.OnEvent:
                    AnimeBehavior(m_MixEventFlame, m_MixEventAnimType, m_EventAnimValue);
                    break;
                case PlayBackType.End:
                    AnimeBehavior(m_MixEndFlame, m_MixEndAnimType, m_EndAnimValue);
                    break;
            }
        }
    }

    void AnimeBehavior(List<float> FlameTimes, List<AnimetionType> Types, List<Vector4> Vec4)
    {
        if (AnimMove(FlameTimes, Types, Vec4))
        {
            if (FlameTimes.Count - 1 != Count)
            {
                Count++;
            }
            else
            {
                this.transform.position = StartPos;
                this.transform.rotation = ConvertToQuat(StartRot);
                this.transform.localScale = StartScal;
                Count = 0;
                PlayAnimetion = false;
            }
        }

    }

    bool AnimMove(List<float> FlameTimes,List<AnimetionType> Types,List<Vector4> Vec4)
    {
        TotalTime += Time.deltaTime;
        if (TotalTime >= FlameTimes[Count])
        {
            return true;
        }

        AnimetionType Type = Types[Count];

        switch (Type)
        {
            case AnimetionType.Postion:
                var pos = Lerp(TotalTime, transform.position, ConverToVec3(Vec4[Count]), FlameTimes[Count]);
                this.transform.position = pos;
                break;
            case AnimetionType.Rotation:
                var Rot = Lerp(TotalTime,ConvertToVec4(transform.rotation), Vec4[Count], FlameTimes[Count]);
                this.transform.rotation = ConvertToQuat(Rot);
                break;
            case AnimetionType.Scale:
                var scale = Lerp(TotalTime, transform.localScale, ConverToVec3(Vec4[Count]), FlameTimes[Count]);
                this.transform.localScale = scale;
                break;
        }
        return false;
    }

    Vector3 Lerp(float TotalTime, Vector3 StartVec, Vector3 EndVec, float AllTime)
    {
        var SpanVec = EndVec - StartVec;
        return SpanVec * TotalTime / AllTime + StartVec;
    }

    Vector4 Lerp(float TotalTime, Vector4 StartVec, Vector4 EndVec, float AllTime)
    {
        var SpanVec = EndVec - StartVec;
        return SpanVec * TotalTime / AllTime + StartVec;
    }

    Vector3 ConverToVec3(Vector4 Vec4)
    {
        return new Vector3(Vec4.x, Vec4.y, Vec4.z);
    }

    Vector4 ConvertToVec4(Quaternion Quat)
    {
        return new Vector4(Quat.x, Quat.y, Quat.z, Quat.w);
    }

    Quaternion ConvertToQuat(Vector4 Vec4)
    {
        return new Quaternion(Vec4.x, Vec4.y, Vec4.z, Vec4.w);
    }

    /* ---- ここから拡張コード ---- */
#if UNITY_EDITOR
    /**
     * Inspector拡張クラス
     */
    [CustomEditor(typeof(ParamBase))]               //!< 拡張するときのお決まりとして書いてね
    public class ParamEditor : Editor           //!< Editorを継承するよ！
    {
        bool folding = true;

        bool Animfold = true;

        int toolSelect = 0;

        private readonly string[] TabToggles = { "MainTab", "AnimetionTab"};

        int TabSelect = 0;

        public override void OnInspectorGUI()
        {
            // target は処理コードのインスタンスだよ！ 処理コードの型でキャストして使ってね！
            ParamBase param = target as ParamBase;

            /* -- カスタム表示 -- */
            EditorGUI.BeginChangeCheck();
            Color defaultColor = GUI.backgroundColor;

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUI.backgroundColor = new Color(1.0f, 0.75f, 0.75f, 1.0f);
                using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    TabSelect = GUILayout.Toolbar(TabSelect, TabToggles, new GUIStyle(EditorStyles.toolbarButton), GUI.ToolbarButtonSize.Fixed);
                }
                GUI.backgroundColor = defaultColor;

                switch (TabSelect)
                {
                    case 0:
                        MainTab(param, defaultColor);
                        break;
                    case 1:
                        AnimetionTab(param, defaultColor);
                        break;
                }
            }

            if (EditorGUI.EndChangeCheck()&&!EditorApplication.isPlaying)
            {
                Undo.RecordObject(target, "Change Inspector");
                EditorUtility.SetDirty(param);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            }
        }

        void MainTab(ParamBase param,Color defaultColor)
        {
            // -- クラスタイプ --
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
                if (GUILayout.Button("削除"))
                {
                    if (list.Count != 0)
                    {
                        list.RemoveAt(list.Count - 1);
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

            param.SharedActive = EditorGUILayout.ToggleLeft("書き出し有効", param.SharedActive);

            if (param.SharedActive)
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
            if (param.Type == ObjectType.Switch || param.Type == ObjectType.FireLine)
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

        void AnimetionTab(ParamBase param,Color defaultColor)
        {
            // --  --
            GUI.backgroundColor = new Color(0.75f, 0.75f, 0.75f, 1.0f);
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                EditorGUILayout.LabelField("アニメーション設定");
                GUI.backgroundColor = defaultColor;
            }
            GUI.backgroundColor = defaultColor;

            param.PlayAnime = (PlayBackType)EditorGUILayout.EnumPopup("再生タイミング", param.PlayAnime);

            switch(param.PlayAnime)
            {
                case PlayBackType.Start:
                    param.StartAnimetionActive = EditorGUILayout.ToggleLeft("書き出し対象に追加する", param.StartAnimetionActive);
                    AddAnimetion(param,ref param.MaxStartAnimCount, param.m_MixStartFlame, param.m_MixStartAnimType, param.m_StartAnimValue);
                    break;
                case PlayBackType.OnEvent:
                    param.EventAnimetionActive = EditorGUILayout.ToggleLeft("書き出し対象に追加する", param.EventAnimetionActive);
                    AddAnimetion(param, ref param.MaxEventAnimCount, param.m_MixEventFlame, param.m_MixEventAnimType, param.m_EventAnimValue);
                    break;
                case PlayBackType.End:
                    param.EndAnimetionActive = EditorGUILayout.ToggleLeft("書き出し対象に追加する", param.EndAnimetionActive);
                    AddAnimetion(param, ref param.MaxEndAnimCount, param.m_MixEndFlame, param.m_MixEndAnimType, param.m_EndAnimValue);
                    break;
            }

        }

        void AddAnimetion(ParamBase param,ref float MaxFlameCount,List<float> FlameTimes, List<AnimetionType> Types, List<Vector4> Vec4)
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                MaxFlameCount = MaxFlameCount < 1.0f ? 1.0f : MaxFlameCount;
                MaxFlameCount = EditorGUILayout.FloatField("フレーム最大値", MaxFlameCount);

                if (EditorApplication.isPlaying)
                {
                    EditorGUILayout.LabelField("現在の再生フレーム数：" + param.TotalTime.ToString());
                }

                toolSelect = GUILayout.Toolbar(toolSelect, new string[] { "Position", "Rotation", "Scale" });
                if (EditorApplication.isPlaying)
                {
                    if (GUILayout.Button("再生"))
                    {
                        param.TotalTime = 0.0f;
                        param.PlayAnimetion = true;
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("追加"))
                    {

                        FlameTimes.Add(0.0f);
                        Types.Add((AnimetionType)toolSelect);
                        switch (toolSelect)
                        {
                            case 0:
                                Vec4.Add(param.transform.position);
                                break;
                            case 1:
                                Vec4.Add(ConvertToVec4(param.transform.rotation));
                                break;
                            case 2:
                                Vec4.Add(param.transform.localScale);
                                break;
                        }
                    }

                    if (GUILayout.Button("削除"))
                    {
                        if (FlameTimes.Count != 0)
                        {
                            FlameTimes.RemoveAt(FlameTimes.Count - 1);
                            Types.RemoveAt(Types.Count - 1);
                            Vec4.RemoveAt(Vec4.Count - 1);
                        }
                    }
                }

                if (GUILayout.Button("全削除"))
                {
                    FlameTimes.Clear();
                    Types.Clear();
                    Vec4.Clear();
                }


                // -- タグ情報 --
                int j, FlameCount = FlameTimes.Count;

                // 折りたたみ表示
                if (Animfold = EditorGUILayout.Foldout(Animfold, "Data"))
                {
                    // リスト表示
                    for (j = 0; j < FlameCount; ++j)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        FlameTimes[j] = EditorGUILayout.Slider("フレーム数", FlameTimes[j], 0.0f, MaxFlameCount);
                        Types[j] = (AnimetionType)EditorGUILayout.EnumPopup("Type", Types[j]);
                        if (Types[j] == AnimetionType.Rotation)
                        {
                            Vec4[j] = EditorGUILayout.Vector4Field("Value", Vec4[j]);
                        }
                        else
                            Vec4[j] = EditorGUILayout.Vector3Field("Value", Vec4[j]);
                        EditorGUILayout.EndVertical();
                    }
                }
            }

        }

        Vector4 ConvertToVec4(Quaternion Quat)
        {
            return new Vector4(Quat.x, Quat.y, Quat.z, Quat.w);
        }

        Quaternion ConvertToQuat(Vector4 Vec4)
        {
            return new Quaternion(Vec4.x, Vec4.y, Vec4.z, Vec4.w);
        }
    }
#endif
}
