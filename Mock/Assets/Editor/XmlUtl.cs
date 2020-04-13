using UnityEditor;
using UnityEngine;
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
}

public class Util
{
    //データ新規
    public static root DataSet(int AreaNumber,int StageNumber)
    {
        root result = new root();
        result.Areas = new List<Area>();
        Area area = new Area();
        area.AreaNumber = AreaNumber;
        area.StageData = new List<Stage>();

        Stage Stage = new Stage();
        Stage.StageNumber = StageNumber;
        Stage.StageObjects = new List<Object>();

        AddStageData(ref Stage);

        area.StageData.Add(Stage);
        result.Areas.Add(area);
        
        return result;
    }

    //データ追加
    public static void AddData(int AreaNumber,int StageNumber, ref root data)
    {
        var area = data.Areas.Where(x => x.AreaNumber == AreaNumber).FirstOrDefault();
        if(area ==null)
        {
            Area areaptr = new Area();
            areaptr.AreaNumber = AreaNumber;
            areaptr.StageData = new List<Stage>();

            Stage Stage = new Stage();
            Stage.StageNumber = StageNumber;
            Stage.StageObjects = new List<Object>();

            AddStageData(ref Stage);

            areaptr.StageData.Add(Stage);
            data.Areas.Add(areaptr);
            return;
        }
        else
        {
            var StagePtr = area.StageData.Where(x => x.StageNumber == StageNumber).FirstOrDefault();
            if (StagePtr == null)
            {
                Stage Stage = new Stage();
                Stage.StageNumber = StageNumber;
                Stage.StageObjects = new List<Object>();

                AddStageData(ref Stage);

                area.StageData.Add(Stage);
                return;
            }
            else
            {
                StagePtr.StageObjects.Clear();

                AddStageData(ref StagePtr);

                area.StageData.RemoveAt(StageNumber);

                area.StageData.Add(StagePtr);
                return;
            }

        }

    }

    private static void AddStageData(ref Stage stage)
    {
        foreach (var Obj in GameObject.FindGameObjectsWithTag("Editor"))
        {
            var TransComp = Obj.GetComponent<Transform>();
            var Param = Obj.GetComponent<ParamBase>();
            if (!Param)
                return;
            Object Ob = new Object();
            Ob.Type = Param.Type;
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
            stage.StageObjects.Add(Ob);
        }

    }

    private static string RotToStr(Quaternion quat)
    {
        string result;
        result = quat.x.ToString() + "," + quat.y.ToString() + "," + quat.z.ToString() + "," + quat.w.ToString();
        return result;
    }

    private static string VecToStr(Vector3 vec)
    {
        string result;
        result = vec.x.ToString() + "," + vec.y.ToString() + "," + vec.z.ToString();
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

