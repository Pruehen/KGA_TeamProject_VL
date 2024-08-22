using System;
using UnityEngine;

public class CheckUI : MonoBehaviour
{
    Action excutionEvent;

    public void CheckUiActive_OnClick(Action callBack)
    {
        this.gameObject.SetActive(true);
        excutionEvent = callBack;
    }
    void CheckUIDeActive()
    {
        this.gameObject.SetActive(false);
        excutionEvent = null;
    }

    public void Yes_OnClick()
    {
        excutionEvent?.Invoke();
        CheckUIDeActive();
    }

    public void No_OnClick()
    {
        CheckUIDeActive();
    }
}
