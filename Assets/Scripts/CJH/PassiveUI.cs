using EnumTypes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassiveUI : MonoBehaviour, ISelectHandler
{
    [SerializeField] private PassiveID _passiveID;
    public PassiveID PassiveID
    {
        get { return _passiveID; }
        set
        {
            _passiveID = value;
            SetUI();
        }
    }
    public bool AvailableSlot()
    {
        return JsonDataManager.GetUserData().AvailableSlot(equipSlotIndex) && PassiveID == PassiveID.None;
    }

    [SerializeField] bool OnlyViewMode = false;
    [SerializeField] bool locked = false;
    [SerializeField] bool lockedSlot = false;
    [SerializeField] int equipSlotIndex = -1;

    [SerializeField] GameObject Icon_Lock;

    [SerializeField] Image Image_Icon;//PassiveId 가 변경되면 해당 이미지를 변경함.


    private void Awake()
    {
        if (PassiveID != PassiveID.None && OnlyViewMode == false)
        {
            if (!PassiveUIManager.Instance.ID_PassiveUI_Dic.ContainsKey(PassiveID))
            {
                PassiveUIManager.Instance.ID_PassiveUI_Dic.Add(PassiveID, this);
            }
            else
            {
                return;
            }
        }
        //SetUI();
    }
    public void OnSelect(BaseEventData eventData)
    {
        PassiveUIManager.Instance.InfoText(PassiveID);
    }

    public void SetUI()
    {        
        if(equipSlotIndex >= 0)
        {
            lockedSlot = !JsonDataManager.GetUserData().AvailableSlot(equipSlotIndex);
            Icon_Lock.SetActive(lockedSlot);
        }
        else
        {
            locked = !JsonDataManager.GetUserData().UnlockPassiveHashSet.Contains(PassiveID);
            Icon_Lock.SetActive(locked && PassiveID != PassiveID.None);
        }

        if (PassiveID == PassiveID.None)
        {
            Image_Icon.gameObject.SetActive(false);
        }
        else
        {
            Image_Icon.gameObject.SetActive(true);            
            if(locked || lockedSlot)
            {
                Image_Icon.sprite = Resources.Load<Sprite>(JsonDataManager.GetPassive(PassiveID).IconPath_Dis);
            }
            else
            {
                Image_Icon.sprite = Resources.Load<Sprite>(JsonDataManager.GetPassive(PassiveID).IconPath);
            }
        }
    }

    public void SetPassiveId(PassiveID newPassiveID)
    {
        PassiveID = newPassiveID;        
    }

    public void OnClick_TryEquip()
    {
        if (PassiveID == PassiveID.None)
            return;

        if(locked)
        {
            CheckUIManager.Instance.CheckUiActive_OnClick(TryUnLock, "해금하시겠습니까?");
            return;
        }


        if (PassiveUIManager.Instance.Try_EquipPassive(this))
        {
            JsonDataManager.GetUserData().TryAddPassive(PassiveID);            
            PassiveID = PassiveID.None;            
        }
        else
        {
            Debug.Log("추가 실패");            
        }
    }

    void TryUnLock()
    {
        if (PassiveUIManager.Instance.TryUseEmerald(JsonDataManager.GetPassive(PassiveID).Cost))
        {            
            JsonDataManager.GetUserData().TryUnLockPassive(PassiveID);
            SetUI();
        }
        else
        {
            CheckUIManager.Instance.CheckUiActive_OnClick(NotMony, "돈이 부족합니다.");
        }
    }
    void TryUnLockSlot()
    {
        if (PassiveUIManager.Instance.TryUseEmerald(2000))//추후에 가격 수정할 것
        {
            JsonDataManager.GetUserData().TryUnLockPassiveSlot(equipSlotIndex);
            SetUI();
        }
        else
        {
            CheckUIManager.Instance.CheckUiActive_OnClick(NotMony, "돈이 부족합니다.");
        }
    }

    void NotMony()
    { 
    }

    public void OnClick_TryUnEquip()
    {
        if (lockedSlot)
        {
            CheckUIManager.Instance.CheckUiActive_OnClick(TryUnLockSlot, "해금하시겠습니까?");
            return;
        }

        JsonDataManager.GetUserData().TryRemovePassive(PassiveID);
        PassiveUIManager.Instance.Try_EquipUnPassive(this);
        PassiveID = PassiveID.None;        
    }
}
