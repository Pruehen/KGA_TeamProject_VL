using EnumTypes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PassiveUIManager : SceneSingleton<PassiveUIManager>
{
    [SerializeField] Button FalstButton;

    [SerializeField] List<PassiveUI> PassiveUI = new List<PassiveUI>();

    public Dictionary<PassiveID, PassiveUI> ID_PassiveUI_Dic = new Dictionary<PassiveID, PassiveUI>();

    public List<PassiveUI> PassiveUIGroup_EquipList = new List<PassiveUI>();


    [SerializeField] Text functionText;
    [SerializeField] Text costText;
    [SerializeField] Text effectText;

    [SerializeField] Text outgameGold;

   

    private void Awake()
    {
        PassiveUI.AddRange(FindObjectsOfType<PassiveUI>());
    }
    public void Init(UserData userData)
    {
        EventSystem.current.SetSelectedGameObject(FalstButton.gameObject);

        foreach (var item in PassiveUI)
        {
            item.SetUI();
        }
        InfoText(PassiveID.Offensive1);
        GetSlotData_OnInit(userData);

        SetOutGamGoldText_OnInit();
    }


    void GetSlotData_OnInit(UserData userData)
    {
        foreach (var item in ID_PassiveUI_Dic)
        {
            item.Value.SetPassiveId(item.Key);
        }

        foreach (var item in PassiveUIGroup_EquipList)
        {
            item.SetPassiveId(PassiveID.None);
        }

        Debug.Log($"{userData.SaveDataIndex}번째 데이터 로드");
        foreach (var item in userData.UsePassiveHashSet)
        {
            ID_PassiveUI_Dic[item].OnClick_TryEquip();
        }
    }

    public void InfoText(PassiveID passiveID)
    {
        if (passiveID == PassiveID.None)
        {
            functionText.text = "";
            costText.gameObject.SetActive(false);
            costText.text = "";
            effectText.text = "";
        }
        else
        {
            PassiveData data = JsonDataManager.GetPassive(passiveID);//패시브의 정보를 받는 부분

            functionText.text = data.PrintName();
            costText.gameObject.SetActive(true);
            costText.text = data.Cost.ToString();
            effectText.text = data.PrintInfo();
        }
    }



    public bool Try_EquipPassive(PassiveUI targetUI)
    {
        switch (targetUI.PassiveID)
        {
            case PassiveID.Offensive1:
            case PassiveID.Offensive2:
            case PassiveID.Offensive3:
            case PassiveID.Offensive4:
            case PassiveID.Offensive5:
                if (PassiveUIGroup_EquipList[0].AvailableSlot())
                {
                    PassiveUIGroup_EquipList[0].PassiveID = targetUI.PassiveID;
                    return true;
                }
                else if (PassiveUIGroup_EquipList[1].AvailableSlot())
                {
                    PassiveUIGroup_EquipList[1].PassiveID = targetUI.PassiveID;
                    return true;
                }
                break;
            case PassiveID.Defensive1:
            case PassiveID.Defensive2:
            case PassiveID.Defensive3:
            case PassiveID.Defensive4:
            case PassiveID.Defensive5:
                if (PassiveUIGroup_EquipList[2].AvailableSlot())
                {
                    PassiveUIGroup_EquipList[2].PassiveID = targetUI.PassiveID;
                    return true;
                }
                else if (PassiveUIGroup_EquipList[3].AvailableSlot())
                {
                    PassiveUIGroup_EquipList[3].PassiveID = targetUI.PassiveID;
                    return true;
                }
                break;
            case PassiveID.Utility1:
            case PassiveID.Utility2:
            case PassiveID.Utility3:
            case PassiveID.Utility4:
            case PassiveID.Utility5:
                if (PassiveUIGroup_EquipList[4].AvailableSlot())
                {
                    PassiveUIGroup_EquipList[4].PassiveID = targetUI.PassiveID;
                    return true;
                }
                else if (PassiveUIGroup_EquipList[5].AvailableSlot())
                {
                    PassiveUIGroup_EquipList[5].PassiveID = targetUI.PassiveID;
                    return true;
                }
                break;
            case PassiveID.None:
                Debug.LogWarning("패시브 ID가 None입니다. 아무 작업도 수행하지 않습니다.");
                return false;
        }
        return false;
    }
    public void Try_EquipUnPassive(PassiveUI targetUI)
    {
        PassiveID targetUiPassiveID = targetUI.PassiveID;

        switch (targetUiPassiveID)
        {
            case PassiveID.Offensive1:
            case PassiveID.Offensive2:
            case PassiveID.Offensive3:
            case PassiveID.Offensive4:
            case PassiveID.Offensive5:                
                ID_PassiveUI_Dic[targetUiPassiveID].PassiveID = targetUiPassiveID;
                ID_PassiveUI_Dic[targetUiPassiveID].SetUI();
                break;
            case PassiveID.Defensive1:
            case PassiveID.Defensive2:
            case PassiveID.Defensive3:
            case PassiveID.Defensive4:
            case PassiveID.Defensive5:                
                ID_PassiveUI_Dic[targetUiPassiveID].PassiveID = targetUiPassiveID;
                ID_PassiveUI_Dic[targetUiPassiveID].SetUI();
                break;
            case PassiveID.Utility1:
            case PassiveID.Utility2:
            case PassiveID.Utility3:
            case PassiveID.Utility4:
            case PassiveID.Utility5:                
                ID_PassiveUI_Dic[targetUiPassiveID].PassiveID = targetUiPassiveID;
                ID_PassiveUI_Dic[targetUiPassiveID].SetUI();
                break;
            case PassiveID.None:
                Debug.LogWarning("패시브 ID가 None입니다. 아무 작업도 수행하지 않습니다.");
                break;
        }
    }


    public void OnClickStartButton()
    {
        GameManager.Instance.StartGame();
    }

    //에메랄드 UI를 갱신해주는 메서드
    public void SetOutGamGoldText_OnInit()
    {
        outgameGold.text = JsonDataManager.GetUserData().Gold.ToString();
    }


    //에메랄드 사용 가능(보유량이 충분)할 경우 true 반환, 불가능할 경우 false 반환 목적
    public bool TryUseEmerald(int costInt)
    {

        //int costint;
        //int.TryParse(costText.text.ToString(), out costint);

        bool suceeded = JsonDataManager.GetUserData().TryUseGold(costInt);

        if (suceeded)
        {
            // UI 갱신
            outgameGold.text = JsonDataManager.GetUserData().Gold.ToString();
            return true;
        }
        else
        {
            Debug.Log("에메랄드 부족.");
            return false;
        }
    }
}