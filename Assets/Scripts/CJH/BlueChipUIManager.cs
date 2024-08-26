using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlueChipUIManager : MonoBehaviour
{
    [Header("현재 착용 중인 블루칩 UI")]
    [SerializeField] List<BlueChipIcon> Icon_EquipChipList;

    [Header("블루칩 선택 UI")]
    [SerializeField] List<BlueChipIcon> Icon_SelectChipList;

    [SerializeField] GameObject Btn_ReRoll;
    bool _canReRoll = false;
    int _randomChipCount = 3;
    int _chipAddLevel = 1;

    public void Init()
    {
        //Debug.Log("블루칩 창 초기화");

        PlayerPassiveCheck();
        //1. 왼쪽에 있는 UI에 현재 착용 중인 블루칩을 표시.    
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
        if (PlayerMaster.Instance._PlayerPassive.ContainPassiveId(EnumTypes.PassiveID.Utility1))//블루칩 선택지를 새로고침 할 수 있다. (게임 당 1회)
        {
            _canReRoll = true;
        }
        else
        {
            _canReRoll = false;
        }

        if (PlayerMaster.Instance._PlayerPassive.ContainPassiveId(EnumTypes.PassiveID.Utility3))//블루칩 선택지가 1개로 감소하지만 높은 레벨의 블루칩이 등장한다.
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
        //2. 중앙에 있는 UI에 임의의 3개의 블루칩 정보를 표시

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
