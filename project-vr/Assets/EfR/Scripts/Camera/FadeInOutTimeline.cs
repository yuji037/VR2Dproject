using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class FadeInOutTimeline : MonoBehaviour, UnityEngine.Timeline.ITimeControl{

    [SerializeField]
    float fadeOutDuration;

    [SerializeField]
    float fadeInDuration;

    public void OnControlTimeStart()
    {
        FadeInOutController._2DFadePanel.StartBlackFadeOut(fadeOutDuration);
    }

    public void OnControlTimeStop()
    {
        FadeInOutController._2DFadePanel.StartBlackFadeIn(fadeInDuration);
    }

    public void SetTime(double time)
    {
        
    }
}
