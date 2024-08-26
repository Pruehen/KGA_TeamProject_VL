using EnumTypes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassiveUI : MonoBehaviour, ISelectHandler
{
    public PassiveID passiveID;
 
    
    [SerializeField] bool OnlyViewMode = false;
    [SerializeField] bool locked = false;
    [SerializeField] bool lockedSlot = false;

    [SerializeField] GameObject Icon_Lock;

    [SerializeField] Image Image_Icon;//PassiveId �� ����Ǹ� �ش� �̹����� ������.


    private void Awake()
    {
        if (passiveID != PassiveID.None && OnlyViewMode == false)
        {
            if (!PassiveUIManager.Instance.ID_PassiveUI_Dic.ContainsKey(passiveID))
            {
                PassiveUIManager.Instance.ID_PassiveUI_Dic.Add(passiveID, this);
            }
            else
            {
                return;
            }
        }
        SetUI();
    }
    public void OnSelect(BaseEventData eventData)
    {
        PassiveUIManager.Instance.InfoText(passiveID);
    }

    public void SetUI()
    {
        if (passiveID == PassiveID.None)
        {
            Image_Icon.gameObject.SetActive(false);
        }
        else
        {
            Image_Icon.gameObject.SetActive(true);            
            if(locked || lockedSlot)
            {
                Icon_Lock.SetActive(true);
                Image_Icon.sprite = Resources.Load<Sprite>(JsonDataManager.GetPassive(passiveID).IconPath_Dis);
            }
            else
            {
                Icon_Lock.SetActive(false);
                Image_Icon.sprite = Resources.Load<Sprite>(JsonDataManager.GetPassive(passiveID).IconPath);
            }
        }
    }

    public void SetPassiveId(PassiveID newPassiveID)
    {
        passiveID = newPassiveID;
        SetUI();
    }

    public void OnClick_TryEquip()
    {
        if (passiveID == PassiveID.None)
            return;

        if(locked)
        {
            CheckUIManager.Instance.CheckUiActive_OnClick(TryUnLock, "�ر��Ͻðڽ��ϱ�?");
            return;
        }


        if (PassiveUIManager.Instance.Try_EquipPassive(this))
        {
            JsonDataManager.GetUserData().TryAddPassive(passiveID);            
            passiveID = PassiveID.None;
            SetUI();
        }
        else
        {
            Debug.Log("�߰� ����");            
        }
    }

    void TryUnLock()
    {
        if (PassiveUIManager.Instance.TryUseEmerald())
        {
            locked = false;
            SetUI();
        }
        else
        {
            CheckUIManager.Instance.CheckUiActive_OnClick(NotMony, "�� ������");
        }
    }

    void NotMony()
    { 
    
    
    }

    public void OnClick_TryUnEquip()
    {
        JsonDataManager.GetUserData().TryRemovePassive(passiveID);
        PassiveUIManager.Instance.Try_EquipUnPassive(this);
        passiveID = PassiveID.None;
        SetUI();
    }





}
