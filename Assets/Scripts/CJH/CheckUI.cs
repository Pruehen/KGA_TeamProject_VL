using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheckUI : MonoBehaviour
{
    Action excutionEvent;
    GameObject curserTemp;

    [SerializeField] Text Text_Msg;
    [SerializeField] GameObject Btn_Yes;
    [SerializeField] GameObject Btn_No;

    //활성화
    public void CheckUiActive_OnClick(Action callBack, string msg)
    {
        this.gameObject.SetActive(true);
        excutionEvent = callBack;

        Text_Msg.text = msg;

        curserTemp = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(Btn_Yes);        
    }

    //비활성화 
    void CheckUIDeActive()
    {
        this.gameObject.SetActive(false);
        excutionEvent = null;
    }

    public void Yes_OnClick()
    {
        EventSystem.current.SetSelectedGameObject(curserTemp);
        
        excutionEvent?.Invoke();        
        CheckUIDeActive();
    }

    public void No_OnClick()
    {
        EventSystem.current.SetSelectedGameObject(curserTemp);
        CheckUIDeActive();
    }
}
