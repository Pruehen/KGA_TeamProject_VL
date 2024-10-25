using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

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
    }
    private void Start()
    {
        if(EventSystem.current.currentInputModule == null)
        {
            return;
        }
        EventSystem.current.currentInputModule.enabled = false;
        EventSystem.current.currentInputModule.enabled = true;
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

        if (InputManager.Instance.IsEscapeBtnClick)
        {
            if (passiveUI.activeSelf == true)
            {
                EnterMainScene_OnEscClick();
            }
        }

    }

    int _slotIndexTemp = 0;
    public void OnClick_NewGameButton(int slotIndex)//���� ���� �� ȣ��
    {
        if (overwriteStartPanel.activeSelf == true)
        {
            _slotIndexTemp = slotIndex;
            CheckUIManager.Instance.CheckUiActive_OnClick(SelectSlot, "���� ���� �����͸� �ε��Ͻðڽ��ϱ�?");
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
        Debug.Log("�ڷ�");
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        startPanel.SetActive(true);
        passiveUI.SetActive(false);
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void SlotSelect()//���� ���� ��ư Ŭ�� �� ȣ��
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
