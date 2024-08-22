using EnumTypes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassiveUI : MonoBehaviour, ISelectHandler
{
    public PassiveID passiveID;

    [SerializeField] bool OnlyViewMode = false;
    [SerializeField] Image Image_Icon;//PassiveId 가 변경되면 해당 이미지를 변경함.

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
        ImageChange();

        if (JsonDataManager.TryGetUserData(0, out UserData data))
        {
            Debug.Log(data.SaveTime);
        }
        else
        {

        }
    }
    public void OnSelect(BaseEventData eventData)
    {
        PassiveUIManager.Instance.InfoText(passiveID);
    }

    public void ImageChange()
    {
        if (passiveID == PassiveID.None)
        {
            Image_Icon.gameObject.SetActive(false);
        }
        else
        {
            Image_Icon.gameObject.SetActive(true);
            Image_Icon.sprite = Resources.Load<Sprite>(JsonDataManager.GetPassive(passiveID).IconPath);
        }
    }

    public void SetPassiveId(PassiveID newPassiveID)
    {
        passiveID = newPassiveID;
        ImageChange();
    }

    public void OnClick_TryEquip()
    {
        if (PassiveUIManager.Instance.Try_EquipPassive(this))
        {
            JsonDataManager.GetUserData().TryAddPassive(passiveID);            
            passiveID = PassiveID.None;
            ImageChange();
        }
        else
        {
            Debug.Log("추가 실패");            
        }
    }

    public void OnClick_TryUnEquip()
    {
        JsonDataManager.GetUserData().TryRemovePassive(passiveID);
        PassiveUIManager.Instance.Try_EquipUnPassive(this);
        passiveID = PassiveID.None;
        ImageChange();
    }

  

}
