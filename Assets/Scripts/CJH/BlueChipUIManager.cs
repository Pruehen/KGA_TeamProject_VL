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
    bool _canReRoll = false;
    int _randomChipCount = 3;
    int _chipAddLevel = 1;

    public void Init()
    {
        //Debug.Log("���Ĩ â �ʱ�ȭ");

        PlayerPassiveCheck();
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

        if (_canReRoll)
        {
            Btn_ReRoll.SetActive(true);
        }
    }

    void PlayerPassiveCheck()
    {
        if (PlayerMaster.Instance._PlayerPassive.ContainPassiveId(EnumTypes.PassiveID.Utility1))//���Ĩ �������� ���ΰ�ħ �� �� �ִ�. (���� �� 1ȸ)
        {
            _canReRoll = true;
        }
        else
        {
            _canReRoll = false;
        }

        if (PlayerMaster.Instance._PlayerPassive.ContainPassiveId(EnumTypes.PassiveID.Utility3))//���Ĩ �������� 1���� ���������� ���� ������ ���Ĩ�� �����Ѵ�.
        {
            _randomChipCount = 1;
            _chipAddLevel = 2;
        }
        else
        {
            _randomChipCount = 3;
            _chipAddLevel = 1;
        }
    }

    public void SetRandomBlueChip()
    {
        List<BlueChipSlot> equipedSlotList = new List<BlueChipSlot>();
        //2. �߾ӿ� �ִ� UI�� ������ 3���� ���Ĩ ������ ǥ��

        foreach (var item in PlayerMaster.Instance._PlayerEquipBlueChip.GetRandomBlueChip(_randomChipCount, _chipAddLevel))
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
