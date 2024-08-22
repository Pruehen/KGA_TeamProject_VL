using EnumTypes;
using UnityEngine;

public class Passive_Offensive1 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Offensive1);
        HpCheckRetio = _passiveData.VelueList[0] * 0.01f;
    }
    public float HpCheckRetio {  get; private set; }
    public bool IsActive { get; set; } = false;
    public override void Active()//"���� ü���� {0}% �̻��� ��, �Ϲ�&Ư�� ���ݷ��� {1}% ����",
    {
        if(IsActive == false)
        {
            base._state.AttackPowerMulti += base._passiveData.VelueList[1] * 0.01f;
            base._state.SkillPowerMulti += base._passiveData.VelueList[2] * 0.01f;
            IsActive = true;
            Debug.Log("����1 �нú� Ȱ��ȭ");
        }        
    }

    public override void DeActive()
    {
        if (IsActive == true)
        {
            base._state.AttackPowerMulti -= base._passiveData.VelueList[1] * 0.01f;
            base._state.SkillPowerMulti -= base._passiveData.VelueList[2] * 0.01f;
            IsActive = false;
            Debug.Log("����1 �нú� ��Ȱ��ȭ");
        }
    }
}
