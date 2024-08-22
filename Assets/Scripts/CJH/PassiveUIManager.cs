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

    public List<PassiveUI> PassiveUIGroup_Offensive = new List<PassiveUI>();
    public List<PassiveUI> PassiveUIGroup_Deffensive = new List<PassiveUI>();
    public List<PassiveUI> PassiveUIGroup_Utility = new List<PassiveUI>();

    int useOffensivePassiveCount = 0;
    int useDeffensivePassiveCount = 0;
    int useUtilityPassiveCount = 0;

    [SerializeField] Text functionText;
    [SerializeField] Text costText;
    [SerializeField] Text effectText;
    
    [SerializeField] int Max_Passive_Count = 2;

    private void Awake()
    {
        PassiveUI.AddRange(FindObjectsOfType<PassiveUI>());
    }
    public void Init(UserData userData)
    {
        EventSystem.current.SetSelectedGameObject(FalstButton.gameObject);

        foreach (var item in PassiveUI)
        {
            item.ImageChange();
        }
        InfoText(PassiveID.Offensive1);
        GetSlotData_OnInit(userData);
    }

    void GetSlotData_OnInit(UserData userData)
    {
        foreach (var item in ID_PassiveUI_Dic)
        {
            item.Value.SetPassiveId(item.Key);
        }

        useOffensivePassiveCount = 0;
        useDeffensivePassiveCount = 0;
        useUtilityPassiveCount = 0;

        foreach (var item in PassiveUIGroup_Offensive)
        {
            item.SetPassiveId(PassiveID.None);
        }
        foreach (var item in PassiveUIGroup_Deffensive)
        {
            item.SetPassiveId(PassiveID.None);
        }
        foreach (var item in PassiveUIGroup_Utility)
        {
            item.SetPassiveId(PassiveID.None);
        }

        Debug.Log($"{userData.SaveDataIndex}��° ������ �ε�");
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
            PassiveData data = JsonDataManager.GetPassive(passiveID);

            functionText.text = data.PrintName();
            costText.gameObject.SetActive(true);
            costText.text = data.Cost.ToString();
            effectText.text = data.PrintInfo();
        }
    }



    public bool Try_EquipPassive(PassiveUI targetUI)
    {
        switch (targetUI.passiveID)
        {
            case PassiveID.Offensive1:
            case PassiveID.Offensive2:
            case PassiveID.Offensive3:
            case PassiveID.Offensive4:
            case PassiveID.Offensive5:
                if (Max_Passive_Count > useOffensivePassiveCount)
                {
                    PassiveUIGroup_Offensive[useOffensivePassiveCount].passiveID = targetUI.passiveID;
                    PassiveUIGroup_Offensive[useOffensivePassiveCount].ImageChange();
                    useOffensivePassiveCount++;
                    return true;
                }
                break;
            case PassiveID.Defensive1:
            case PassiveID.Defensive2:
            case PassiveID.Defensive3:
            case PassiveID.Defensive4:
            case PassiveID.Defensive5:
                if (Max_Passive_Count > useDeffensivePassiveCount)
                {
                    PassiveUIGroup_Deffensive[useDeffensivePassiveCount].passiveID = targetUI.passiveID;
                    PassiveUIGroup_Deffensive[useDeffensivePassiveCount].ImageChange();
                    useDeffensivePassiveCount++;
                    return true;
                }
                break;
            case PassiveID.Utility1:
            case PassiveID.Utility2:
            case PassiveID.Utility3:
            case PassiveID.Utility4:
            case PassiveID.Utility5:
                if (Max_Passive_Count > useUtilityPassiveCount)
                {
                    PassiveUIGroup_Utility[useUtilityPassiveCount].passiveID = targetUI.passiveID;
                    PassiveUIGroup_Utility[useUtilityPassiveCount].ImageChange();
                    useUtilityPassiveCount++;
                    return true;
                }
                break;
            case PassiveID.None:
                Debug.LogWarning("�нú� ID�� None�Դϴ�. �ƹ� �۾��� �������� �ʽ��ϴ�.");
                return false;                
        }
        return false;
    }
    public void Try_EquipUnPassive(PassiveUI targetUI)
    {
        PassiveID targetUiPassiveID = targetUI.passiveID;

        switch (targetUiPassiveID)
        {
            case PassiveID.Offensive1:
            case PassiveID.Offensive2:
            case PassiveID.Offensive3:
            case PassiveID.Offensive4:
            case PassiveID.Offensive5:
                useOffensivePassiveCount--;
                ID_PassiveUI_Dic[targetUiPassiveID].passiveID = targetUiPassiveID;
                ID_PassiveUI_Dic[targetUiPassiveID].ImageChange();
                break;
            case PassiveID.Defensive1:
            case PassiveID.Defensive2:
            case PassiveID.Defensive3:
            case PassiveID.Defensive4:
            case PassiveID.Defensive5:
                useDeffensivePassiveCount--;
                ID_PassiveUI_Dic[targetUiPassiveID].passiveID = targetUiPassiveID;
                ID_PassiveUI_Dic[targetUiPassiveID].ImageChange();
                break;
            case PassiveID.Utility1:
            case PassiveID.Utility2:
            case PassiveID.Utility3:
            case PassiveID.Utility4:
            case PassiveID.Utility5:
                useUtilityPassiveCount--;
                ID_PassiveUI_Dic[targetUiPassiveID].passiveID = targetUiPassiveID;
                ID_PassiveUI_Dic[targetUiPassiveID].ImageChange();
                break;
            case PassiveID.None:
                Debug.LogWarning("�нú� ID�� None�Դϴ�. �ƹ� �۾��� �������� �ʽ��ϴ�.");
                break;
        }
    }


    public void OnClickStartButton()
    {
        SceneManager.LoadScene("Jihe");
    }

}