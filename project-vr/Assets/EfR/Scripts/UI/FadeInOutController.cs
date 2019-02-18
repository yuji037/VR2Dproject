using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeInOutController : SingletonMonoBehaviour<FadeInOutController>
{
   
    public class FadePanel
    {
        public FadePanel(MonoBehaviour _owner, Image fade_panel)
        {
            owner = _owner;
            fadePanel = fade_panel;
        }
        MonoBehaviour owner;
        Image fadePanel;
        Coroutine currentRoutine;

        public void StartBlackFadeIn(float duration)
        {
            StartFadeIn(Color.black, duration);
        }
        public void StartBlackFadeOut(float duration)
        {
            StartFadeOut(Color.black, duration);
        }
        public void StartWhiteFadeIn(float duration)
        {
            StartFadeIn(Color.white, duration);
        }
        public void StartWhiteFadeOut(float duration)
        {
            StartFadeOut(Color.white, duration);
        }

        //自分で色の値を設定できる
        public void StartFadeIn(Color _color, float duration)
        {
            if (currentRoutine != null) owner.StopCoroutine(currentRoutine);
            currentRoutine = owner.StartCoroutine(FadeInRoutine(_color, duration));
        }
        //自分で色の値を設定できる
        public void StartFadeOut(Color _color, float duration)
        {
            if (currentRoutine != null) owner.StopCoroutine(currentRoutine);
            currentRoutine = owner.StartCoroutine(FadeOutRoutine(_color, duration));
        }

        IEnumerator FadeInRoutine(Color _color, float duration)
        {
            fadePanel.color = _color;
            yield return owner.StartCoroutine(FadeRoutine(duration, false));
        }
        IEnumerator FadeOutRoutine(Color _color, float duration)
        {
            _color.a = 0;
            fadePanel.color = _color;
            yield return owner.StartCoroutine(FadeRoutine(duration, true));
        }
        IEnumerator FadeRoutine(float duration, bool is_plus)
        {
            int multipliValue = 1;
            if (!is_plus) multipliValue = -1;
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                fadePanel.color += multipliValue * new Color(0, 0, 0, (1.0f / duration) * Time.deltaTime);
                yield return null;
            }
            var endColor = fadePanel.color;
            endColor.a = (is_plus) ? 1.0f : 0f;
            fadePanel.color = endColor;
        }
    }

    public enum ViewType{
        _2D,VR
    }

    [SerializeField]
    Image _2dfadePanel;

    [SerializeField]
    Image vrFadePanel;

    FadePanel[] fadePanels;

    private void Start()
    {
        fadePanels = new FadePanel[2];
        fadePanels[(int)ViewType._2D] =new FadePanel(this,_2dfadePanel);
        fadePanels[(int)ViewType.VR] = new FadePanel(this,vrFadePanel);
        //DebugTools.RegisterDebugAction(KeyCode.N, () => { fadePanels[(int)ViewType._2D].StartBlackFadeIn(1.0f); }, "2dfadein");
        //DebugTools.RegisterDebugAction(KeyCode.M, () => { fadePanels[(int)ViewType._2D].StartBlackFadeOut(1.0f); }, "2dfadeout");
    }

    public FadePanel GetFadePanel(ViewType viewType)
    {
        return fadePanels[(int)viewType];
    }
}
