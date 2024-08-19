using EnumTypes;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassiveUIManager : MonoBehaviour
{
    [SerializeField] Button FalstButton;

    private void Awake()
    {
        EventSystem.current.SetSelectedGameObject(FalstButton.gameObject);
        InputManager.Instance.PropertyChanged += OnInputPropertyChanged;
    }


    void OnInputPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(InputManager.Instance.IsInteractiveBtnClick):
                if (InputManager.Instance.IsInteractiveBtnClick == true)
                {
                    if (gameObject.activeSelf == true)
                    {
                        Button selectedButton = EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
                        selectedButton.onClick.Invoke();
                        Debug.Log(selectedButton);

                    }
                    else
                    {
                        return;
                    }


                }
                break;
        }
    }



}
