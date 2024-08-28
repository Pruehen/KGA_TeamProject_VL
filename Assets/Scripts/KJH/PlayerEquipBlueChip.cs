using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class PlayerEquipBlueChip : MonoBehaviour
{
    Dictionary<BlueChipID, BlueChipSlot> useBlueChipDic = new Dictionary<BlueChipID, BlueChipSlot>();
    PlayerMaster _playerMaster;
    [SerializeField] SO_SKillEvent BlueChipVFX;

    public void Awake()
    {
        _playerMaster = GetComponent<PlayerMaster>();
    }
    public int GetBlueChipLevel(BlueChipID targetBlueChip)
    {
        int level = 0;

        if(useBlueChipDic.ContainsKey(targetBlueChip))
        {
            level = useBlueChipDic[targetBlueChip].Level;
        }

        return level;
    }
    public Dictionary<BlueChipID, BlueChipSlot> GetBlueChipDic()
    {
        return useBlueChipDic;
    }
    public bool TryAddBlueChip(BlueChipID targetBlueChip)
    {
        int targetChipLevel = GetBlueChipLevel(targetBlueChip);

        if(targetChipLevel == 0)//Ĩ�� ���� ���
        {
            if (useBlueChipDic.Count >= 3)
            {
                Debug.LogWarning("�̹� 3���� Ĩ�� �����ϰ� �ֽ��ϴ�");
                return false;
            }
            else
            {
                Debug.Log("���ο� Ĩ�� �����մϴ�.");
                useBlueChipDic.Add(targetBlueChip, new BlueChipSlot(targetBlueChip, 1));
                return true;
            }
        }
        else if(targetChipLevel > 0 && targetChipLevel < JsonDataManager.GetBlueChipData(targetBlueChip).Level_VelueList.Count)//Ĩ�� ������, �ִ� ���� �̸��� ���
        {            
            useBlueChipDic[targetBlueChip].LevelUp(1);
            Debug.Log($"Ĩ ������ ��ȭ�մϴ�. : {useBlueChipDic[targetBlueChip].Level}����");
            return true;
        }
        else//Ĩ�� �ִ� ������ ���
        {
            Debug.LogWarning("Ĩ�� �ִ� �����Դϴ�");
            return false;
        }
    }
    public bool TryAddBlueChip(BlueChipID targetBlueChip, int targetLevel)
    {
        int targetChipLevel = GetBlueChipLevel(targetBlueChip);

        if (targetChipLevel == 0)//Ĩ�� ���� ���
        {
            if (useBlueChipDic.Count >= 3)
            {
                Debug.LogWarning("�̹� 3���� Ĩ�� �����ϰ� �ֽ��ϴ�");
                return false;
            }
            else
            {
                Debug.Log("���ο� Ĩ�� �����մϴ�.");
                useBlueChipDic.Add(targetBlueChip, new BlueChipSlot(targetBlueChip, targetLevel));
                _playerMaster._PlayerSkill.Effect2(BlueChipVFX);
                return true;
            }
        }
        else if (targetChipLevel > 0 && targetChipLevel < JsonDataManager.GetBlueChipData(targetBlueChip).Level_VelueList.Count)//Ĩ�� ������, �ִ� ���� �̸��� ���
        {
            useBlueChipDic[targetBlueChip].SetLevel(targetLevel);
            Debug.Log($"Ĩ ������ ��ȭ�մϴ�. : {useBlueChipDic[targetBlueChip].Level}����");
            _playerMaster._PlayerSkill.Effect2(BlueChipVFX);
            return true;
        }
        else//Ĩ�� �ִ� ������ ���
        {
            Debug.LogWarning("Ĩ�� �ִ� �����Դϴ�");
            return false;
        }
    }
    public void SwapBlueChip(BlueChipID targetId, BlueChipID newId, int level)
    {
        if(useBlueChipDic.ContainsKey(targetId))
        {
            useBlueChipDic.Remove(targetId);
            Debug.Log("���ο� Ĩ�� �����մϴ�.");
            useBlueChipDic.Add(newId, new BlueChipSlot(newId, level));
        }
        else
        {
            Debug.LogWarning($"�������� �ʴ� ��� Ĩ�Դϴ�. : {targetId}");
        }
    }

    public Dictionary<BlueChipID, BlueChipSlot> GetRandomBlueChip(int count, int addLevel)
    {
        Dictionary<BlueChipID, BlueChipSlot> selectSlotDic = new Dictionary<BlueChipID, BlueChipSlot>();

        while (selectSlotDic.Count < count)
        {
            BlueChipID id = (BlueChipID)Random.Range(0, 9);

            if (selectSlotDic.ContainsKey(id))
                continue;

            int targetChipLevel = GetBlueChipLevel(id);
            int targetChipMaxLevel = JsonDataManager.GetBlueChipData(id).Level_VelueList.Count;

            if (targetChipLevel < targetChipMaxLevel)//Ĩ�� �ִ� ���� �̸��� ���
            {
                targetChipLevel += addLevel;
                if(targetChipLevel > targetChipMaxLevel)
                {
                    targetChipLevel = targetChipMaxLevel;
                }
                selectSlotDic.Add(id, new BlueChipSlot(id, targetChipLevel));
            }
        }

        return selectSlotDic;
    }
}


public class BlueChipSlot
{
    public BlueChipID Id { get; private set; }
    public int Level { get; private set; }    

    public BlueChipSlot(BlueChipID id, int level)
    {
        Id = id;        
        Level = level;
    }

    public void LevelUp(int addLevel)
    {
        Level += addLevel;        
    }
    public void SetLevel(int level)
    {
        Level = level;
    }

}