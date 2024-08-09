using System;
using System.ComponentModel;
using UnityEngine;

public class PlayerModChangeManager : MonoBehaviour
{
    public bool IsAbsorptState { get; private set; }

    public void TrySetAbsorptState(bool value)
    {        
        if(value == true && IsAbsorptState == false)
        {
            IsAbsorptState = true;
            EnterAbsorptState();
        }
        if(value == false && IsAbsorptState == true)
        {
            IsAbsorptState = false;
            SucceseAbsorptState();
        }
    }

    private void Awake()
    {        
        InputManager.Instance.PropertyChanged += OnInputPropertyChanged;
    }
    void OnInputPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(InputManager.Instance.IsLControlBtnClick):
                TrySetAbsorptState(InputManager.Instance.IsLControlBtnClick);
                break;
        }
    }

    public Action OnEnterAbsorptState;
    public Func<int> OnSucceseAbsorptState;
    public Action OnEndAbsorptState;

    public void EnterAbsorptState()
    {
        OnEnterAbsorptState.Invoke();
    }
    public void SucceseAbsorptState()
    {
        int value = OnSucceseAbsorptState.Invoke();
        Debug.Log($"{value}°³ Èí¼ö");
    }
    public void EndAbsorptState()
    {

    }
}
