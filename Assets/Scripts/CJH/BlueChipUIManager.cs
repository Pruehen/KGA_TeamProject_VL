using EnumTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueChipUIManager : MonoBehaviour
{
    [Header("현재 착용 중인 블루칩 UI")]
    [SerializeField] List<BlueChipIcon> Icon_EquipChipList;

    [Header("블루칩 선택 UI")]
    [SerializeField] List<BlueChipIcon> Icon_SelectChipList;

 

    public void Init()
    {
        //Debug.Log("블루칩 창 초기화");

        //1. 왼쪽에 있는 UI에 현재 착용 중인 블루칩을 표시.    
        List<BlueChipSlot> equipedSlotList = new List<BlueChipSlot>();
        foreach (var item in PlayerMaster.Instance._PlayerEquipBlueChip.GetBlueChipDic())
        {
            equipedSlotList.Add(item.Value);
        }                
        for (int i = 0; i < equipedSlotList.Count; i++)
        {
            Icon_EquipChipList[i].PrintChipData(equipedSlotList[i]);
        }

        //2. 중앙에 있는 UI에 임의의 3개의 블루칩 정보를 표시
        List<BlueChipSlot> selectSlotList = new List<BlueChipSlot>();
        List<int> usedIndexes = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            int randomIndex;

            // 중복되지 않는 숫자를 뽑을 때까지 반복
            do
            {
                randomIndex = Random.Range(0, 9);
            }
            while (usedIndexes.Contains(randomIndex));

            // 뽑은 숫자를 사용된 리스트에 추가
            usedIndexes.Add(randomIndex);

            selectSlotList.Add(new BlueChipSlot((BlueChipID)randomIndex, 1));


        }
        for (int i = 0; i < selectSlotList.Count; i++)
        {
            Icon_SelectChipList[i].PrintChipData(selectSlotList[i]);
        }


    }
}
