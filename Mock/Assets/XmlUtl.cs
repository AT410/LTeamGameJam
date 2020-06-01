using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;

[Serializable]
public class root
{
    //エリア
    [XmlElement("Area")]
    public List<Area> Areas;
}

[Serializable]
public class Area
{
    /// <summary>
    /// ステージナンバー
    /// </summary>
    /// <info>
    /// チュートリアルは0番
    /// デフォルト値は-1
    /// </info>
    [XmlAttribute("AreaNumber")]
    [DefaultValue(-1)]
    public int AreaNumber = -1;

    //ステージデータ
    [XmlElement("StageData")]
    public List<Stage> StageData;
}

[Serializable]
public class Stage
{
    //各種パラメータ(隠し要素数など)
    [XmlAttribute("StageNumber")]
    [DefaultValue(-1)]
    public int StageNumber = -1;
    [XmlAttribute("CameraEye")]
    public string EyeStr;
    [XmlAttribute("CameraAt")]
    public string AtStr;
    [XmlAttribute("CameraNear")]
    public string NearStr;
    [XmlAttribute("CameraFar")]
    public string FarStr;
    //配置データ
    [XmlElement("Objects")]
    public List<Object> StageObjects = new List<Object>();
}

[Serializable]
public class Object
{
    [System.Xml.Serialization.XmlAttribute("Type")]
    public string Type;
    [System.Xml.Serialization.XmlAttribute("Tag")]
    public string GameTag;
    [System.Xml.Serialization.XmlAttribute("Pos")]
    public string Pos;
    [System.Xml.Serialization.XmlAttribute("Scale")]
    public string Scale;
    [System.Xml.Serialization.XmlAttribute("Rot")]
    public string Rot;
    [System.Xml.Serialization.XmlAttribute("MeshKey")]
    public string MeshKey;
    [System.Xml.Serialization.XmlAttribute("TexKey")]
    public string TexKey;
    [System.Xml.Serialization.XmlAttribute("Tags")]
    public string TagStr;

    [System.Xml.Serialization.XmlAttribute("CollisionActive")]
    public string CollActiveStr;
    [System.Xml.Serialization.XmlAttribute("CollisionFixedActive")]
    public string CollFixedStr;

    [System.Xml.Serialization.XmlAttribute("SharedActive")]
    public string ActiveStr;
    [System.Xml.Serialization.XmlAttribute("SharedKey")]
    public string SharedStr;

    [System.Xml.Serialization.XmlAttribute("EventActive")] //受信可能
    public string EventActiveStr;
    [System.Xml.Serialization.XmlAttribute("EventReceiverKey")]//受信設定キー
    public string EventReceiverKeyStr;
    [System.Xml.Serialization.XmlAttribute("EventRecipientKey")]//送信先キー（対象の受信設定キー）
    public string EventRecipientKeyStr;
    [System.Xml.Serialization.XmlAttribute("EventMsgStr")]//送信メッセージ
    public string EventMsgStr;

    // -- FireLineのみ適用 --
    [XmlAttribute("FireLineDirection")]
    [DefaultValue(0)]
    public int DelDir = 0;

    // -- アニメーション設定 --
    [XmlAttribute("StartAnimetionActive")]
    public string StartAnimetionActive;
    [XmlAttribute("EventAnimetionActive")]
    public string EventAnimetionActive;
    [XmlAttribute("EndAnimetionActive")]
    public string EndAnimetionActive;

    [XmlElement("Animetions")]
    public List<AnimSetData> animSets;
}

[Serializable]
public class AnimSetData
{
    [XmlAttribute("CallPoint")]
    public PlayBackType playBack;

    [XmlAttribute("MaxFlameCount")]//最大フレーム数
    public string MaxFlameCount;

    [XmlElement("AnimationData")]
    public List<AnimetionData> Datas;
}

[Serializable]
public class AnimetionData
{
    [XmlAttribute("FlameCount")]//フレーム数
    public string FlameCount;

    [XmlAttribute("Type")]
    public AnimetionType Type;

    [XmlAttribute("Value")]//移動量
    public string Value;
}

[Serializable]
public class UIRoot
{
    [XmlElement("UISet")]
    public List<UISet> UISets;
}

[Serializable]
public class UISet
{
    [System.Xml.Serialization.XmlAttribute("UIType")]
    public string UIType;

    [XmlElement("UIData")]
    public List<UIData> UIDatas;
}

[Serializable]
public class UIData
{
    [System.Xml.Serialization.XmlAttribute("Type")]
    public string Type;
    [System.Xml.Serialization.XmlAttribute("Pos")]
    public string Pos;
    [System.Xml.Serialization.XmlAttribute("Width")]
    public string Width;
    [System.Xml.Serialization.XmlAttribute("Height")]
    public string Height;
    [System.Xml.Serialization.XmlAttribute("TexKey")]
    public string TexKey;
    [System.Xml.Serialization.XmlAttribute("Layer")]
    [DefaultValue(-1)]
    public int DrawLayer = -1;
    [System.Xml.Serialization.XmlAttribute("MyIndexKey")]
    public string MyIndexKey;
    [System.Xml.Serialization.XmlAttribute("EventKey")]
    public string EventKey;
    [System.Xml.Serialization.XmlAttribute("UpKey")]
    public string UpKey;
    [System.Xml.Serialization.XmlAttribute("DownKey")]
    public string DownKey;
    [System.Xml.Serialization.XmlAttribute("LeftKey")]
    public string LeftKey;
    [System.Xml.Serialization.XmlAttribute("RightKey")]
    public string RightKey;

    [XmlAttribute("AreaNumber")]
    [DefaultValue(-1)]
    public int AreaNumber = -1;
    [XmlAttribute("StageNumber")]
    [DefaultValue(-1)]
    public int StageNumber = -1;
}


public class Util
{
    //データ新規
    public static root DataSet(int AreaNumber,int StageNumber,out bool Success)
    {
        root result = new root();
        result.Areas = new List<Area>();
        Area area = new Area();
        area.AreaNumber = AreaNumber;
        area.StageData = new List<Stage>();

        var CamObj = GameObject.FindGameObjectWithTag("CameraEye");
        var Eye = CamObj.GetComponent<Transform>().position;
        var camera = CamObj.GetComponent<Camera>();
        var near = camera.nearClipPlane;
        var far = camera.farClipPlane;
        
        var AtObj = GameObject.FindGameObjectWithTag("CameraAt");
        var At = AtObj.GetComponent<Transform>().position;

        Stage Stage = new Stage();
        Stage.StageNumber = StageNumber;
        Stage.EyeStr = VecToStr(Eye);
        Stage.AtStr = VecToStr(At);
        Stage.NearStr = near.ToString();
        Stage.FarStr = far.ToString();
        Stage.StageObjects = new List<Object>();

        Success = AddStageData(ref Stage);

        area.StageData.Add(Stage);
        result.Areas.Add(area);
        
        return result;
    }

    //データ追加
    public static bool AddData(int AreaNumber,int StageNumber, ref root data)
    {
        bool success = false;
        var area = data.Areas.Where(x => x.AreaNumber == AreaNumber).FirstOrDefault();
        if(area ==null)
        {
            Area areaptr = new Area();
            areaptr.AreaNumber = AreaNumber;
            areaptr.StageData = new List<Stage>();

            var CamObj = GameObject.FindGameObjectWithTag("CameraEye");
            var Eye = CamObj.GetComponent<Transform>().position;
            var camera = CamObj.GetComponent<Camera>();
            var near = camera.nearClipPlane;
            var far = camera.farClipPlane;

            var AtObj = GameObject.FindGameObjectWithTag("CameraAt");
            var At = AtObj.GetComponent<Transform>().position;

            Stage Stage = new Stage();
            Stage.StageNumber = StageNumber;
            Stage.EyeStr = VecToStr(Eye);
            Stage.AtStr = VecToStr(At);
            Stage.NearStr = near.ToString();
            Stage.FarStr = far.ToString();
            Stage.StageObjects = new List<Object>();

            success = AddStageData(ref Stage);

            areaptr.StageData.Add(Stage);
            data.Areas.Add(areaptr);
            return success;
        }
        else
        {
            var StagePtr = area.StageData.Where(x => x.StageNumber == StageNumber).FirstOrDefault();
            if (StagePtr == null)
            {
                var CamObj = GameObject.FindGameObjectWithTag("CameraEye");
                var Eye = CamObj.GetComponent<Transform>().position;
                var camera = CamObj.GetComponent<Camera>();
                var near = camera.nearClipPlane;
                var far = camera.farClipPlane;

                var AtObj = GameObject.FindGameObjectWithTag("CameraAt");
                var At = AtObj.GetComponent<Transform>().position;


                Stage Stage = new Stage();
                Stage.StageNumber = StageNumber;
                Stage.EyeStr = VecToStr(Eye);
                Stage.AtStr = VecToStr(At);
                Stage.NearStr = near.ToString();
                Stage.FarStr = far.ToString();
                Stage.StageObjects = new List<Object>();

                success = AddStageData(ref Stage);

                area.StageData.Add(Stage);
                return success;
            }
            else
            {
                StagePtr.StageObjects.Clear();
                area.StageData.Remove(StagePtr);

                Stage NewStage = new Stage();

                var CamObj = GameObject.FindGameObjectWithTag("CameraEye");
                var Eye = CamObj.GetComponent<Transform>().position;
                var camera = CamObj.GetComponent<Camera>();
                var near = camera.nearClipPlane;
                var far = camera.farClipPlane;

                var AtObj = GameObject.FindGameObjectWithTag("CameraAt");
                var At = AtObj.GetComponent<Transform>().position;
                NewStage.EyeStr = VecToStr(Eye);
                NewStage.AtStr = VecToStr(At);
                NewStage.NearStr = near.ToString();
                NewStage.FarStr = far.ToString();
                NewStage.StageNumber = StageNumber;

                success = AddStageData(ref NewStage);


                area.StageData.Add(NewStage);

                //sort
                area.StageData.Sort((a, b) => a.StageNumber - b.StageNumber);
                return success;
            }

        }

    }

    private static bool AddStageData(ref Stage stage)
    {
        foreach (var Obj in GameObject.FindGameObjectsWithTag("Editor"))
        {
            var TransComp = Obj.GetComponent<Transform>();
            var Param = Obj.GetComponent<ParamBase>();
            if (!Param)
            {
                return false;
            }
            Object Ob = new Object();
            Ob.Type = Param.Type.ToString();
            Ob.Pos = VecToStr(TransComp.position);
            Ob.Rot = RotToStr(TransComp.rotation);
            Ob.Scale = VecToStr(TransComp.localScale);
            Ob.MeshKey = Param.MeshKey;
            Ob.TexKey = Param.TexKey;
            for(int index=0 ;index<Param.Tags.Count();index++)
            {
                Ob.TagStr += Param.Tags[index];
                if(Param.Tags.Count!=index+1)
                {
                    Ob.TagStr += ",";
                }
            }

            Ob.CollActiveStr = Convert.ToInt32(Param.CollisionActive).ToString();
            if(Param.CollisionActive)
            {
                Ob.CollFixedStr = Convert.ToInt32(Param.SetFixed).ToString();
            }

            //共有設定
            Ob.ActiveStr = Convert.ToInt32(Param.SharedActive).ToString();
            if(Param.SharedActive)
            {
                Ob.SharedStr = Param.SharedKey;
            }

            //イベント設定
            Ob.EventActiveStr = Convert.ToInt32(Param.EventActive).ToString();
            if(Param.EventActive)
            {
                Ob.EventReceiverKeyStr = Param.EventReceiveKey;
            }

            //スイッチオブジェクトの時追加情報入力
            if(Param.Type == ObjectType.Switch || Param.Type == ObjectType.FireLine)
            {
                Ob.EventRecipientKeyStr = Param.EventSendKey!=null ? Param.EventSendKey:"";
                Ob.EventMsgStr = Param.EventMsgStr != null ? Param.EventMsgStr : "";
                if(Ob.EventRecipientKeyStr==""||Ob.EventMsgStr =="")
                {
                    EditorUtility.DisplayDialog("Warning", "イベント設定が不十分です。", "Close");
                    return false;
                }
            }

            // -- FireLineの追加設定 --
            if(Param.Type ==ObjectType.FireLine)
            {
                Ob.DelDir = Convert.ToInt32(Param.configu);
            }

            // -- アニメーション設定 --
            Ob.animSets = new List<AnimSetData>();

            Ob.StartAnimetionActive = Convert.ToInt32(Param.StartAnimetionActive).ToString();

            if (Param.StartAnimetionActive)
            {
                SetAnimetionData(ref Ob, PlayBackType.Start, Param.MaxStartAnimCount, Param.m_MixStartPosFlame, Param.m_StartAnimPos,
                    Param.m_MixStartRotFlame, Param.m_StartAnimRotate,
                    Param.m_MixStartScalFlame, Param.m_StartAnimScale);
            }

            Ob.EventAnimetionActive = Convert.ToInt32(Param.EventAnimetionActive).ToString();
            if (Param.EventAnimetionActive)
            {
                SetAnimetionData(ref Ob, PlayBackType.OnEvent, Param.MaxEventAnimCount, Param.m_MixEventPosFlame, Param.m_EventAnimPos,
                    Param.m_MixEventRotFlame, Param.m_EventAnimRotate,
                    Param.m_MixEventScalFlame, Param.m_EventAnimScale);
            }

            Ob.EndAnimetionActive = Convert.ToInt32(Param.EndAnimetionActive).ToString();
            if (Param.EndAnimetionActive)
            {
                SetAnimetionData(ref Ob, PlayBackType.End, Param.MaxEndAnimCount, Param.m_MixEndPosFlame, Param.m_EndAnimPos,
                    Param.m_MixEndRotFlame, Param.m_EndAnimRotate,
                    Param.m_MixEndScalFlame, Param.m_EndAnimScale);
            }

            stage.StageObjects.Add(Ob);
        }

        return true;
    }

    public static void SetAnimetionData(ref Object obj,PlayBackType play ,float MaxFlame, 
            List<float> FlamePosTimes, List<Vector4> PosVec4,
            List<float> FlameRotTimes, List<Vector4> RotVec4,
            List<float> FlameScalTimes, List<Vector4> ScalVec4)
    {
        AnimSetData setData = new AnimSetData();
        setData.playBack = play;
        setData.MaxFlameCount = MaxFlame.ToString();

        setData.Datas = new List<AnimetionData>();

        AddAnimetionData(AnimetionType.Postion,ref setData.Datas, FlamePosTimes, PosVec4);
        AddAnimetionData(AnimetionType.Rotation,ref setData.Datas, FlameRotTimes, RotVec4);
        AddAnimetionData(AnimetionType.Scale,ref setData.Datas, FlameScalTimes, ScalVec4);

        obj.animSets.Add(setData);
    }

    public static void AddAnimetionData(AnimetionType Type,ref List<AnimetionData> animetions, List<float> FlameTimes, List<Vector4> Vec4)
    {
        int LoopCont = FlameTimes.Count;
        for (int i = 0; i < LoopCont; i++)
        {
            AnimetionData data = new AnimetionData();
            data.FlameCount = FlameTimes[i].ToString();
            data.Type = Type;
            data.Value = VecToStr(Vec4[i]);

            animetions.Add(data);
        }

    }

    //UISET
    public static UIRoot CreateNewUISet(UISetType type)
    {
        UIRoot root = new UIRoot();
        root.UISets = new List<UISet>();
        UISet iSet = new UISet();
        iSet.UIType = type.ToString();
        iSet.UIDatas = new List<UIData>();
        AddUIData(ref iSet); 
        root.UISets.Add(iSet);
        return root;
    }

    public static void AddUISet(UISetType type,ref UIRoot result)
    {
        var UISet = result.UISets.Where(x => x.UIType == type.ToString()).FirstOrDefault();
        if(UISet ==null)
        {
            UISet iSet = new UISet();
            iSet.UIType = type.ToString();
            iSet.UIDatas = new List<UIData>();
            AddUIData(ref iSet);
            result.UISets.Add(iSet);
            return;
        }
        else
        {
            result.UISets.Remove(UISet);
            UISet iSet = new UISet();
            iSet.UIType = type.ToString();
            iSet.UIDatas = new List<UIData>();
            AddUIData(ref iSet);
            result.UISets.Add(iSet);

            result.UISets.Sort((a, b) => a.UIType.CompareTo(b.UIType));
            return;
        }
    }

    private static void AddUIData(ref UISet uISet)
    {
        foreach (var Test in GameObject.FindGameObjectsWithTag("UI"))
        {
            //メッシュの取得
            UIData data = new UIData();
            var RectT = Test.GetComponent<RectTransform>();
            var Pos = RectT.localPosition;
            var Width = RectT.rect.width;
            var Heght = RectT.rect.height;

            var param = Test.GetComponent<UIParam>();
            data.Type = param.type.ToString();
            data.Pos = VecToStr(new Vector2(Pos.x,Pos.y));
            data.Height = Heght.ToString();
            data.Width = Width.ToString();

            data.TexKey = param.TexKey;
            data.DrawLayer = param.DrawLayer;

            if (param.type == UIType.Flashing)
            {
                data.EventKey = param.Event.ToString();
                data.MyIndexKey = param.MyIndexKey;
                data.UpKey = param.Upkey;
                data.DownKey = param.DownKey;
                data.LeftKey = param.LeftKey;
                data.RightKey = param.RightKey;
            }

            if (param.Event == GameEvent.ToGameStage &&param.type == UIType.Flashing)
            {
                data.AreaNumber = param.AreaNum;
                data.StageNumber = param.StageNum;
            }
            uISet.UIDatas.Add(data);
        }

    }

    private static string RotToStr(Quaternion quat)
    {
        string result;
        result = quat.x.ToString() + "," + quat.y.ToString() + "," + quat.z.ToString() + "," + quat.w.ToString();
        return result;
    }

    private static string VecToStr(Vector4 vec)
    {
        string result;
        result = vec.x.ToString() + "," + vec.y.ToString() + "," + vec.z.ToString()+","+vec.w.ToString() ;
        return result;
    }
    private static string VecToStr(Vector3 vec)
    {
        string result;
        result = vec.x.ToString() + "," + vec.y.ToString() + "," + vec.z.ToString();
        return result;
    }

    private static string VecToStr(Vector2 vec)
    {
        string result;
        result = vec.x.ToString() + "," + vec.y.ToString();
        return result;
    }
}

[Serializable]
public class XmlUtil
{
#if UNITY_EDITOR
    // シリアライズ
    public static void Seialize<T>(string filename, T data,FileMode mode =FileMode.Create)
    {
        try
        {
            using (var stream = new FileStream(filename, mode))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, data);
            }
        }
        catch
        {
            //Debug.Log("同名ファイルが存在している");
        }
    }

    // デシリアライズ
    public static T Deserialize<T>(string filename)
    {
        using (var stream = new FileStream(filename, FileMode.Open))
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }
    }
#endif
}

