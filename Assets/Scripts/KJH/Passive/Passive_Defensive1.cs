using EnumTypes;

public class Passive_Defensive1 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Defensive1);
    }
    public override void Active()//"�ִ� ü���� {0}% �����ϴ� ��� �Ϲ�&Ư�� ���ݷ� {1}% ����",
    {
        base._state.MaxHpMulti += base._passiveData.VelueList[0] * 0.01f;
        base._state.AttackPowerMulti -= base._passiveData.VelueList[1] * 0.01f;
        base._state.SkillPowerMulti -= base._passiveData.VelueList[2] * 0.01f;
    }

    public override void DeActive()
    {

    }
}
