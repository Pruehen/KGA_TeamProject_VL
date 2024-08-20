using EnumTypes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassiveUI : MonoBehaviour, ISelectHandler
{
    public PassiveID passiveID;

    [SerializeField] Image Image_Icon;//PassiveId �� ����Ǹ� �ش� �̹����� ������.

    private void Awake()
    {
        if(passiveID != PassiveID.None)
        {
            PassiveUIManager.Instance.ID_PassiveUI_Dic.Add(passiveID, this);            
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

    public void OnClick_TryEquip()
    {
        if (PassiveUIManager.Instance.Try_EquipPassive(this))
        {
            passiveID = PassiveID.None;
            ImageChange();
        }
        else
        {
            Debug.Log("�߰� ����");
        }
    }

    public void OnClick_TryUnEquip()
    {
        PassiveUIManager.Instance.Try_EquipUnPassive(this);
        passiveID = PassiveID.None;
        ImageChange();
    }

  

}
