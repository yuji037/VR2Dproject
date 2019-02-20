using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class StageInfoTimeline : MonoBehaviour, ITimeControl
{
    public void OnControlTimeStart()
    {
        StageInformationViewer.GetInstance().Play();
    }

    public void OnControlTimeStop()
    {
        StageInformationViewer.GetInstance().Stop();
    }

    public void SetTime(double time)
    {
    }
}
