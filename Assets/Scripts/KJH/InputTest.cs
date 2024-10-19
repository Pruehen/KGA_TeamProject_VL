using System.ComponentModel;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    InputManager inputManager;
    private void Awake()
    {
        inputManager = InputManager.Instance;
        inputManager.PropertyChanged += OnPropertyChanged;
    }
    private void OnDestroy()
    {
        inputManager.PropertyChanged -= OnPropertyChanged;
    }

    void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(inputManager.IsLMouseBtnClick):
                Debug.Log(inputManager.IsLMouseBtnClick);
                break;
        }
    }
}
