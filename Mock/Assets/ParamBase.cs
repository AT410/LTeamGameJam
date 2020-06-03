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
    FrontWall,
    MoveFloor,
    Slope
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

public enum FireLineConfigu
{
    None =0,
    LeftToRight,
    RightToLeft,
    UpToDown,
    DownToUp,
    FrontToBack,
    BackToFront
}

public enum CreateOrder
{
    Early,
    Default,
    Late
}


public class ParamBase : MonoBehaviour
{
    public CreateOrder order = CreateOrder.Default;
    public ObjectType Type;
    public string MeshKey="DEFAULT_CUBE";
    public string TexKey="TEST_TX";
    public List<string> Tags;

    public bool CollisionActive = true;
    public bool SetFixed = false;

    public bool SharedActive = false;
    public string SharedKey;

    public bool EventActive = false;
    public string EventReceiveKey;

    public string EventSendKey;
    public string EventMsgStr;

    public FireLineConfigu configu = FireLineConfigu.None;

    //アニメーション設定
    public bool StartAnimetionActive;
    public bool EventAnimetionActive;
    public bool EndAnimetionActive;

    // -- 開始時アニメーション --
    public List<float> m_MixStartPosFlame = new List<float>();
    public List<Vector4> m_StartAnimPos = new List<Vector4>();

    public List<float> m_MixStartRotFlame = new List<float>();
    public List<Vector4> m_StartAnimRotate = new List<Vector4>();

    public List<float> m_MixStartScalFlame = new List<float>();
    public List<Vector4> m_StartAnimScale = new List<Vector4>();

    // -- イベント時アニメーション --
    public List<float> m_MixEventPosFlame = new List<float>();
    public List<Vector4> m_EventAnimPos = new List<Vector4>();

    public List<float> m_MixEventRotFlame = new List<float>();
    public List<Vector4> m_EventAnimRotate = new List<Vector4>();

    public List<float> m_MixEventScalFlame = new List<float>();
    public List<Vector4> m_EventAnimScale = new List<Vector4>();

    // -- イベント時アニメーション --
    public List<float> m_MixEndPosFlame = new List<float>();
    public List<Vector4> m_EndAnimPos = new List<Vector4>();

    public List<float> m_MixEndRotFlame = new List<float>();
    public List<Vector4> m_EndAnimRotate = new List<Vector4>();

    public List<float> m_MixEndScalFlame = new List<float>();
    public List<Vector4> m_EndAnimScale = new List<Vector4>();

    // -- フレームカウント最大数 --
    public float MaxStartAnimCount;
    public float MaxEventAnimCount;
    public float MaxEndAnimCount;

    // -- テスト変数 --
    bool PlayFireLineActive = false;
    bool PlayPosAnimetion = false;
    bool PlayRotAnimetion = false;
    bool PlayScaleAnimetion = false;
    int PosCount = 0;
    int RotCount = 0;
    int ScaleCount = 0;
    public float PosTotalTime;
    public float RotTotalTime;
    public float ScaleTotalTime;
    PlayBackType PlayAnime;


    Vector3 StartPos;
    Vector4 StartRot;
    Vector3 StartScal;
    private void Awake()
    {
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
        // -- アニメーションの再生 --
        PlayAnimation();

        // -- 導火線のテスト --
        PlayFireLine();
    }

    void PlayFireLine()
    {
        if(PlayFireLineActive)
        {
            var Postion = this.transform.position;
            var Scale = this.transform.localScale;

            switch (configu)
            {
                case FireLineConfigu.None:
                    break;
                case FireLineConfigu.LeftToRight:
                    Postion.x = FireLineBehaviorPos(-1, Postion.x);
                    Scale.x = FireLineBehaviorScale(Scale.x);
                    break;
                case FireLineConfigu.RightToLeft:
                    Postion.x = FireLineBehaviorPos(+1, Postion.x);
                    Scale.x = FireLineBehaviorScale(Scale.x);
                    break;
                case FireLineConfigu.UpToDown:
                    Postion.y = FireLineBehaviorPos(-1, Postion.y);
                    Scale.y = FireLineBehaviorScale(Scale.y);
                    break;
                case FireLineConfigu.DownToUp:
                    Postion.y = FireLineBehaviorPos(+1, Postion.y);
                    Scale.y = FireLineBehaviorScale(Scale.y);
                    break;
                case FireLineConfigu.FrontToBack:
                    Postion.z = FireLineBehaviorPos(+1, Postion.z);
                    Scale.z = FireLineBehaviorScale(Scale.z);
                    break;
                case FireLineConfigu.BackToFront:
                    Postion.z = FireLineBehaviorPos(-1, Postion.z);
                    Scale.z = FireLineBehaviorScale(Scale.z);
                    break;
                default:
                    break;
            }

            // -- 座標系更新 --
            this.transform.position = Postion;
            this.transform.localScale = Scale;

            if(this.transform.localScale.x <= 0||this.transform.localScale.y <=0||this.transform.localScale.z <=0)
            {
                this.gameObject.SetActive(false);
                PlayFireLineActive = false;
            }
        }
    }

    float FireLineBehaviorPos(float Key,float Pos)
    {
        return Pos += Key * 0.025f;
    }

    float FireLineBehaviorScale(float Scale)
    {
        return Scale += -0.05f;
    }

    void PlayAnimation()
    {
        if ((StartAnimetionActive || EventAnimetionActive || EndAnimetionActive) && (PlayPosAnimetion || PlayRotAnimetion || PlayScaleAnimetion))
        {
            switch (PlayAnime)
            {
                case PlayBackType.Start:
                    SelectBehavior(m_MixStartPosFlame, m_StartAnimPos,
                        m_MixStartRotFlame, m_StartAnimRotate,
                        m_MixStartScalFlame, m_StartAnimScale);
                    break;
                case PlayBackType.OnEvent:
                    SelectBehavior(m_MixEventPosFlame, m_EventAnimPos,
                        m_MixEventRotFlame, m_EventAnimRotate,
                        m_MixEventScalFlame, m_EventAnimScale);
                    break;
                case PlayBackType.End:
                    SelectBehavior(m_MixEndPosFlame, m_EndAnimPos,
                        m_MixEndRotFlame, m_EndAnimRotate,
                        m_MixEndScalFlame, m_EndAnimScale);
                    break;
            }

            // -- フレーム更新 --
            PosTotalTime += Time.deltaTime;
            RotTotalTime += Time.deltaTime;
            ScaleTotalTime += Time.deltaTime;
        }
    }

    void SelectBehavior(List<float> FlamePosTimes, List<Vector4> PosVec4,
            List<float> FlameRotTimes, List<Vector4> RotVec4,
            List<float> FlameScalTimes, List<Vector4> ScalVec4)
    {
        if (PlayPosAnimetion)
        {
            if (!AnimeBehavior(ref PosTotalTime,FlamePosTimes, AnimetionType.Postion, ref PosCount, PosVec4))
            {
                PlayPosAnimetion = false;
            }
        }

        if (PlayRotAnimetion)
        {
            if (!AnimeBehavior(ref RotTotalTime,FlameRotTimes, AnimetionType.Rotation, ref RotCount, RotVec4))
            {
                PlayRotAnimetion = false;
            }
        }

        if (PlayScaleAnimetion)
        {
            if (!AnimeBehavior(ref ScaleTotalTime,FlameScalTimes, AnimetionType.Scale, ref ScaleCount, ScalVec4))
            {
                PlayScaleAnimetion = false;
            }
        }
    }

    bool AnimeBehavior(ref float TotalTime,List<float> FlameTimes, AnimetionType Type,ref int Count, List<Vector4> Vec4)
    {
        if(FlameTimes.Count == Count)
        {
            return false;
        }
        if (AnimMove(ref TotalTime, FlameTimes, Type,Count, Vec4))
        {
            if (FlameTimes.Count - 1 != Count)
            {
                Count++;
                TotalTime = 0;
            }
            else
            {
                Count = 0;
                return false;
            }
        }
        return true;
    }

    bool AnimMove(ref float TotalTime,List<float> FlameTimes,AnimetionType Type,int Count,List<Vector4> Vec4)
    {
        if (TotalTime > FlameTimes[Count])
        {
            return true;
        }

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

        private readonly string[] TabToggles = { "MainTab", "ActionTab"};

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
            param.order = (CreateOrder)EditorGUILayout.EnumPopup("生成順序", param.order);
            param.Type = (ObjectType)EditorGUILayout.EnumPopup("クラスタイプ", param.Type);
            param.MeshKey = EditorGUILayout.TextField("メッシュキー", param.MeshKey);
            param.TexKey = EditorGUILayout.TextField("テクスチャキー", param.TexKey);

            if(param.Type == ObjectType.FireLine)
            {
                param.configu = (FireLineConfigu)EditorGUILayout.EnumPopup("減少方向", param.configu);
                if(EditorApplication.isPlaying)
                {
                    if(GUILayout.Button("テスト"))
                    {
                        param.transform.position = param.StartPos;
                        param.transform.rotation = ConvertToQuat(param.StartRot);
                        param.transform.localScale = param.StartScal;

                        param.gameObject.SetActive(true);
                        param.PlayFireLineActive = true;
                    }
                }
            }

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
                EditorGUILayout.LabelField("物理設定");
                GUI.backgroundColor = defaultColor;
            }
            GUI.backgroundColor = defaultColor;

            param.CollisionActive = EditorGUILayout.ToggleLeft("物理判定を有効にする", param.CollisionActive);

            if (param.CollisionActive)
            {
                param.SetFixed = EditorGUILayout.ToggleLeft("固定オブジェクト化", param.SetFixed);
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
            toolSelect = GUILayout.Toolbar(toolSelect, new string[] { "Position", "Rotation", "Scale" });

            switch (param.PlayAnime)
            {
                case PlayBackType.Start:
                    param.StartAnimetionActive = EditorGUILayout.ToggleLeft("書き出し対象に追加する", param.StartAnimetionActive);
                    SelectAnimationType(param,ref param.MaxStartAnimCount, param.m_MixStartPosFlame, param.m_StartAnimPos,
                        param.m_MixStartRotFlame,param.m_StartAnimRotate,
                        param.m_MixStartScalFlame,param.m_StartAnimScale);
                    break;
                case PlayBackType.OnEvent:
                    param.EventAnimetionActive = EditorGUILayout.ToggleLeft("書き出し対象に追加する", param.EventAnimetionActive);
                    SelectAnimationType(param, ref param.MaxEventAnimCount, param.m_MixEventPosFlame, param.m_EventAnimPos,
                        param.m_MixEventRotFlame, param.m_EventAnimRotate,
                        param.m_MixEventScalFlame, param.m_EventAnimScale);
                    break;
                case PlayBackType.End:
                    param.EndAnimetionActive = EditorGUILayout.ToggleLeft("書き出し対象に追加する", param.EndAnimetionActive);
                    SelectAnimationType(param, ref param.MaxEndAnimCount, param.m_MixEndPosFlame, param.m_EndAnimPos,
                        param.m_MixEndRotFlame, param.m_EndAnimRotate,
                        param.m_MixEndScalFlame, param.m_EndAnimScale);
                    break;
            }

        }

        void SelectAnimationType(ParamBase param, ref float MaxFlameCount, List<float> FlamePosTimes, List<Vector4> PosVec4, 
            List<float> FlameRotTimes, List<Vector4> RotVec4, 
            List<float> FlameScalTimes, List<Vector4> ScalVec4)
        {
            switch (toolSelect)
            {
                case 0:
                    AddAnimetion(param, ref MaxFlameCount, FlamePosTimes, PosVec4);
                    break;
                case 1:
                    AddAnimetion(param, ref MaxFlameCount, FlameRotTimes, RotVec4);
                    break;
                case 2:
                    AddAnimetion(param, ref MaxFlameCount, FlameScalTimes, ScalVec4);
                    break;
            }
        }

        void AddAnimetion(ParamBase param,ref float MaxFlameCount,List<float> FlameTimes,List<Vector4> Vec4)
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                MaxFlameCount = MaxFlameCount < 1.0f ? 1.0f : MaxFlameCount;
                MaxFlameCount = EditorGUILayout.FloatField("フレーム最大値", MaxFlameCount);

                if (EditorApplication.isPlaying)
                {
                    switch (toolSelect)
                    {
                        case 0:
                            EditorGUILayout.LabelField("現在の再生フレーム数：" + param.PosTotalTime.ToString());
                            break;
                        case 1:
                            EditorGUILayout.LabelField("現在の再生フレーム数：" + param.RotTotalTime.ToString());
                            break;
                        case 2:
                            EditorGUILayout.LabelField("現在の再生フレーム数：" + param.ScaleTotalTime.ToString());
                            break;
                    }
                }

                if (EditorApplication.isPlaying)
                {
                    using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
                    {
                        if (GUILayout.Button("再生"))
                        {
                            switch (toolSelect)
                            {
                                case 0:
                                    param.transform.position = param.StartPos;
                                    param.PlayPosAnimetion = true;
                                    param.PosTotalTime = 0.0f;
                                    break;
                                case 1:
                                    param.transform.rotation = ConvertToQuat(param.StartRot);
                                    param.PlayRotAnimetion = true;
                                    param.RotTotalTime = 0.0f;
                                    break;
                                case 2:
                                    param.transform.localScale = param.StartScal;
                                    param.PlayScaleAnimetion = true;
                                    param.ScaleTotalTime = 0.0f;
                                    break;
                            }

                        }
                        if (GUILayout.Button("全再生"))
                        {
                            param.PosTotalTime = 0.0f;
                            param.RotTotalTime = 0.0f;
                            param.ScaleTotalTime = 0.0f;

                            param.transform.position = param.StartPos;
                            param.transform.rotation = ConvertToQuat(param.StartRot);
                            param.transform.localScale = param.StartScal;

                            param.PlayPosAnimetion = true;
                            param.PlayRotAnimetion = true;
                            param.PlayScaleAnimetion = true;
                        }
                    }

                    if(GUILayout.Button("ReSet"))
                    {
                        param.PosTotalTime = 0.0f;
                        param.RotTotalTime = 0.0f;
                        param.ScaleTotalTime = 0.0f;

                        param.transform.position = param.StartPos;
                        param.transform.rotation = ConvertToQuat(param.StartRot);
                        param.transform.localScale = param.StartScal;

                        param.PlayPosAnimetion = false;
                        param.PlayRotAnimetion = false;
                        param.PlayScaleAnimetion = false;
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("追加"))
                    {

                        FlameTimes.Add(0.0f);
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
                            Vec4.RemoveAt(Vec4.Count - 1);
                        }
                    }
                }

                if (GUILayout.Button("全削除"))
                {
                    FlameTimes.Clear();
                    Vec4.Clear();
                }

                // -- リスト表示 --
                ViewAnimeVec(MaxFlameCount,FlameTimes, (AnimetionType)toolSelect, Vec4);
            }

        }

        void ViewAnimeVec(float MaxFlameCount,List<float> FlameTimes, AnimetionType Type, List<Vector4> Vec4)
        {
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
                    if (Type == AnimetionType.Rotation)
                    {
                        Vec4[j] = EditorGUILayout.Vector4Field("Value", Vec4[j]);
                    }
                    else
                        Vec4[j] = EditorGUILayout.Vector3Field("Value", Vec4[j]);
                    EditorGUILayout.EndVertical();
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
