using UnityEngine;
using BSJ;
public class FaderSceneMainUi : MonoBehaviour
{
    [SerializeField] private CanvasGroup _target;
    private void OnEnable()
    {
        Fader.FadeIn(_target, this, 2f);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public void OnSceneUnloaded(float unloadTime)
    {
        _target.GetComponent<Canvas>().sortingOrder = 100;
        Fader.FadeOut(_target, this, unloadTime);
    }
}