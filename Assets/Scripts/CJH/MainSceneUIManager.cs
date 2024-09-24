using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MainSceneUIManager : SceneSingleton<MainSceneUIManager>
{
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject passiveUI;
    [SerializeField] GameObject overwriteStartPanel;
    [SerializeField] Button startButton;
    [SerializeField] GameObject[] selectSlot;
    Text[] slotText;

    SaveFillSlot _selectSlotTemp;
    
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

        InputManager.Instance.IsInteractiveBtnClick = false;
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

    int _slotIndexTemp = 0;
    public void OnClick_NewGameButton(int slotIndex)//슬롯 선택 시 호출
    {
        if (overwriteStartPanel.activeSelf == true)
        {
            _slotIndexTemp = slotIndex;
            CheckUIManager.Instance.CheckUiActive_OnClick(SelectSlot, "현재 슬롯 데이터를 로드하시겠습니까?");
        }
    }

    void SelectSlot()
    {
        Debug.Log(_slotIndexTemp);
        JsonDataManager.SetUserDataIndex(_slotIndexTemp);

        if (JsonDataManager.TryGetUserData(_slotIndexTemp, out UserData userData))
        {
            if (userData.TryGetPlayData(out PlayData playData))
            {
                if (userData.PlayData.InGame_StageStarted)
                {
                    GameManager.Instance.StartGame();
                    return;
                }
            }

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
                if(InputManager.Instance.IsInteractiveBtnClick)
                    break;
                Button selectedButton = EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
                if (selectedButton != null && selectedButton.interactable)
                {
                    selectedButton.onClick.Invoke();
                    Debug.Log("Button clicked: " + selectedButton.name);

                    // 버튼 클릭 이후 IsInteractiveBtnClick을 false로 설정하여 중복 호출 방지
                    InputManager.Instance.IsInteractiveBtnClick = false;
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
            EventSystem.current.SetSelectedGameObject(selectSlot[0].gameObject);

            startPanel.SetActive(false);
            overwriteStartPanel.SetActive(true);

            for (int i = 0; i < selectSlot.Length; i++)
            {
                selectSlot[i].GetComponent<SaveFillSlot>().SetData(i);
            }

        }
    }
}
