using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MainSceneUIManager : MonoBehaviour
{
    public static MainSceneUIManager Instance;

    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject passiveUI;
    [SerializeField] GameObject overwriteStartPanel;
    [SerializeField] Button startButton;
    [SerializeField] GameObject[] selectSlot;
    Text[] slotText;




    private void Awake()
    {
        startPanel.SetActive(true);
        passiveUI.SetActive(false);
        overwriteStartPanel.SetActive(false);

        slotText = new Text[selectSlot.Length];
        for (int i = 0; i < selectSlot.Length; i++)
        {
            slotText[i] = selectSlot[i].GetComponentInChildren<Text>();
        }

        Instance = this;
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        InputManager.Instance.PropertyChanged += OnInputPropertyChanged;
    }

    private void Update()
    {
        for (int i = 0; i < selectSlot.Length; i++)
        {
            if (EventSystem.current.currentSelectedGameObject == selectSlot[i].gameObject)
            {
                slotText[i].color = new Color(1f, 0.5f, 0f);
            }
            else if (EventSystem.current.currentSelectedGameObject != selectSlot[i].gameObject)
            {
                slotText[i].color = Color.white;
            }
            else
            {
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (passiveUI.activeSelf == true)
            {
                Debug.Log(123);
                EnterMainScene_OnEscClick();
            }             
        }
    }

    public void OnClick_NewGameButton(int slotIndex)//슬롯 선택 시 호출
    {        
        if (overwriteStartPanel.activeSelf == true)
        {
            Debug.Log(slotIndex);
            JsonDataManager.SetUserDataIndex(slotIndex);
            overwriteStartPanel.SetActive(false);
            passiveUI.SetActive(true);
            PassiveUIManager.Instance.Init(JsonDataManager.GetUserData());
        }
    }
    void EnterMainScene_OnEscClick()
    {
        Debug.Log("뒤로");
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        startPanel.SetActive(true);
        passiveUI.SetActive(false);
    }

    //인풋 F키
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

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void SlotSelect()//게임 시작 버튼 클릭 시 호출
    {
        if (startPanel.activeSelf == true)
        {
            startPanel.SetActive(false);
            overwriteStartPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(selectSlot[0].gameObject);
        }      
    }
}
