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

    int openCount = 1;


    //활성화
    public void CheckUiActive_OnClick(Action callBack, string msg)
    {
        if (openCount > 2)
        {
            openCount = 0;
        }
        openCount++;

        this.gameObject.SetActive(true);
        excutionEvent = callBack;

        Text_Msg.text = msg;

        curserTemp = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(Btn_Yes);
    }

    //비활성화 
    void CheckUIDeActive()
    {
        
        openCount--;

        if (openCount != 0)
        {
            return;
        }
        this.gameObject.SetActive(false);
        excutionEvent = null;
    }


    public void Yes_OnClick()
    {
        EventSystem.current.SetSelectedGameObject(curserTemp);
        excutionEvent?.Invoke();
        CheckUIDeActive();
        InputManager.Instance.IsInteractiveBtnClick = false;
        Debug.Log("Yes");        
    }

    public void No_OnClick()
    {
        EventSystem.current.SetSelectedGameObject(curserTemp);
        CheckUIDeActive();
    }
}
