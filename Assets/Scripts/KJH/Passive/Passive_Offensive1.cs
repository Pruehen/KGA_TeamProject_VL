using EnumTypes;

public class Passive_Offensive1 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Offensive1);
    }
    public override void Active()//���� ü���� 80% �̻��� ��, �Ϲ�&Ư�� ���ݷ��� 15%��ŭ ����
    {
        base._state.AttackPowerMulti += base._passiveData.VelueList[1];
        base._state.SkillPowerMulti += base._passiveData.VelueList[2];
    }

    public override void DeActive()
    {
        base._state.AttackPowerMulti -= base._passiveData.VelueList[1];
        base._state.SkillPowerMulti -= base._passiveData.VelueList[2];
    }
}
