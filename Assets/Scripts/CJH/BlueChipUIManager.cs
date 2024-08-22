using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlueChipUIManager : MonoBehaviour
{
    [Header("���� ���� ���� ���Ĩ UI")]
    [SerializeField] List<BlueChipIcon> Icon_EquipChipList;

    [Header("���Ĩ ���� UI")]
    [SerializeField] List<BlueChipIcon> Icon_SelectChipList;

    [SerializeField] GameObject Btn_ReRoll;
    [SerializeField] bool CanReRoll = false;

    public void Init()
    {
        //Debug.Log("���Ĩ â �ʱ�ȭ");

        //1. ���ʿ� �ִ� UI�� ���� ���� ���� ���Ĩ�� ǥ��.    
        List<BlueChipSlot> equipedSlotList = new List<BlueChipSlot>();
        foreach (var item in PlayerMaster.Instance._PlayerEquipBlueChip.GetBlueChipDic())
        {
            equipedSlotList.Add(item.Value);
        }
        for (int i = 0; i < Icon_EquipChipList.Count; i++)
        {
            if (equipedSlotList.Count > i)
            {
                Icon_EquipChipList[i].SetChipData(equipedSlotList[i]);
            }
            else
            {
                Icon_EquipChipList[i].SetChipData(null);
            }
        }

        SetRandomBlueChip();

        if (CanReRoll)
        {
            Btn_ReRoll.SetActive(true);
        }
    }

    public void SetRandomBlueChip()
    {
        List<BlueChipSlot> equipedSlotList = new List<BlueChipSlot>();
        //2. �߾ӿ� �ִ� UI�� ������ 3���� ���Ĩ ������ ǥ��

        foreach (var item in PlayerMaster.Instance._PlayerEquipBlueChip.GetRandomBlueChip())
        {
            equipedSlotList.Add(item.Value);
        }
        for (int i = 0; i < Icon_SelectChipList.Count; i++)
        {
            if (equipedSlotList.Count > i)
            {
                Icon_SelectChipList[i].SetChipData(equipedSlotList[i]);
            }
            else
            {
                Icon_SelectChipList[i].SetChipData(null);
            }
        }
        Btn_ReRoll.SetActive(false);
        EventSystem.current.SetSelectedGameObject(Icon_SelectChipList[0].gameObject);
    }
}
