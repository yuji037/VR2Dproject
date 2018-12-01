using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LineLinker : SingletonMonoBehaviour<LineLinker>
{
    enum IgnoreAxis
    {
        X,
        Y,
        Z,
    }
    [SerializeField]
    //IgnoreAxis ignoreAxis = IgnoreAxis.Z;

    List<TransmissionLine> allLineList = new List<TransmissionLine>();

    List<TransmissionLine> powerOffLineList = new List<TransmissionLine>();

    [SerializeField]
    float sensingRange = 8.0f;


    Vector3 currentPos;
    public void AddLine(TransmissionLine transmissionLine)
    {
        allLineList.Add(transmissionLine);
    }

    private void Update()
    {
        LinkLine();
    }

    void LinkLine()
    {
        powerOffLineList = new List<TransmissionLine>(allLineList);
        var havePowerList = allLineList.FindAll(x => x.havePower);
        foreach (var line in havePowerList)
        {
            line.IsPowerOn = true;
            powerOffLineList.Remove(line);
            foreach (var p in line.IOPoints)
            {
                LinkNearPoints(p);
            }
        }

        foreach (var i in powerOffLineList)
        {
            i.IsPowerOn = false;
        }
    }

    void LinkNearPoints(IOPoint ioPoint)
    {
        foreach (var line in allLineList)
        {
            foreach (var p in line.IOPoints)
            {
                if (LinkPoint(ioPoint,p))
                {
                    //リンクが成功した点以外の点をリンクさせる。
                    var otherPoints = line.IOPoints.FindAll(x=>x!=p);
                    foreach (var op in otherPoints)
                    {
                        LinkNearPoints(op);
                    }
                }
            }
        }
    }
    bool LinkPoint(IOPoint powerOutPoint, IOPoint inPoint)
    {
        bool ownerIsEqual = (powerOutPoint.OwnerLine == inPoint.OwnerLine);
        bool canLink = CanLink(powerOutPoint.Position, inPoint.Position);
        bool isPowerOff = powerOffLineList.Contains(inPoint.OwnerLine);
        if (!ownerIsEqual && canLink&&isPowerOff)
        {
            inPoint.IsPowerOn = true;
            powerOffLineList.Remove(inPoint.OwnerLine);
            return true;
        }
        return false;
    }
    bool CanLink(Vector3 point1, Vector3 point2)
    {
        return Vector2.Distance(ConvertVector2(point1), ConvertVector2(point2)) < sensingRange;
    }
    //vector3→vector2変換をするためのラッパー
    Vector2 ConvertVector2(Vector3 vector)
    {
        var _2dcamera=ViewSwitchPerformer.GetInstance().Get2DCamera();
        return _2dcamera.WorldToScreenPoint(vector);

        //switch (ignoreAxis)
        //{
        //    case IgnoreAxis.X:
        //        return new Vector2(vector.y, vector.z);
        //    case IgnoreAxis.Y:
        //        return new Vector2(vector.x, vector.z);
        //    case IgnoreAxis.Z:
        //        return new Vector2(vector.x, vector.y);
        //}
        //return vector;
    }
}
