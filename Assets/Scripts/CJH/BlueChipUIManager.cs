using EnumTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueChipUIManager : MonoBehaviour
{
    [Header("���� ���� ���� ���Ĩ UI")]
    [SerializeField] List<BlueChipIcon> Icon_EquipChipList;

    [Header("���Ĩ ���� UI")]
    [SerializeField] List<BlueChipIcon> Icon_SelectChipList;

 

    public void Init()
    {
        //Debug.Log("���Ĩ â �ʱ�ȭ");

        //1. ���ʿ� �ִ� UI�� ���� ���� ���� ���Ĩ�� ǥ��.    
        List<BlueChipSlot> equipedSlotList = new List<BlueChipSlot>();
        foreach (var item in PlayerMaster.Instance._PlayerEquipBlueChip.GetBlueChipDic())
        {
            equipedSlotList.Add(item.Value);
        }                
        for (int i = 0; i < equipedSlotList.Count; i++)
        {
            Icon_EquipChipList[i].PrintChipData(equipedSlotList[i]);
        }

        //2. �߾ӿ� �ִ� UI�� ������ 3���� ���Ĩ ������ ǥ��
        List<BlueChipSlot> selectSlotList = new List<BlueChipSlot>();
        List<int> usedIndexes = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            int randomIndex;

            // �ߺ����� �ʴ� ���ڸ� ���� ������ �ݺ�
            do
            {
                randomIndex = Random.Range(0, 9);
            }
            while (usedIndexes.Contains(randomIndex));

            // ���� ���ڸ� ���� ����Ʈ�� �߰�
            usedIndexes.Add(randomIndex);

            selectSlotList.Add(new BlueChipSlot((BlueChipID)randomIndex, 1));


        }
        for (int i = 0; i < selectSlotList.Count; i++)
        {
            Icon_SelectChipList[i].PrintChipData(selectSlotList[i]);
        }


    }
}
