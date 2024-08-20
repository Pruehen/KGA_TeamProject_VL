using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.Arm;

public class MainSceneUIManager : MonoBehaviour
{
    public static MainSceneUIManager Instance;

    [SerializeField] GameObject StartPanel;
    [SerializeField] GameObject passiveUI;
    [SerializeField] Button startButton;

    private void Awake()
    {
        Instance = this;
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        InputManager.Instance.PropertyChanged += OnInputPropertyChanged;
    }

    public void OnClick_NewGameButton()
    {
        if (StartPanel.activeSelf == true)
        {
            StartPanel.SetActive(false);
            passiveUI.SetActive(true);
            PassiveUIManager.Instance.Reton();
        }
        else
        {
            StartPanel.SetActive(true);
            passiveUI.SetActive(false);
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        }
      

    }

    public void OnClick_ExitButton()
    {
        Application.Quit();
    }


    void OnInputPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(InputManager.Instance.IsInteractiveBtnClick):
                if (InputManager.Instance.IsInteractiveBtnClick == true)
                {
                       Button selectedButton = EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
                        selectedButton.onClick.Invoke();
                }
                break;
        }
    }

}
