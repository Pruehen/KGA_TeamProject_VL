using System;
using UnityEngine;

public class CheckUIManager : GlobalSingleton<CheckUIManager>
{
    [SerializeField] CheckUI _checkUI;

    private void Awake()
    {
        Instance.Init();
        _checkUI.No_OnClick();
    }

    public void CheckUiActive_OnClick(Action callBack, string msg = "")
    {
        _checkUI.CheckUiActive_OnClick(callBack, msg);
    }

    void Init()
    {

    }
}
