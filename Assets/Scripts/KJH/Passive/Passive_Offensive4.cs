using EnumTypes;

public class Passive_Offensive4 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Offensive4);
    }
    public override void Active()//"Ư�� ���ݷ� {0}% ����, ��ų�� ���� ���� {1}% ����, ��ų ������ ȹ�淮 {2}% ����",
    {
        base._state.SkillPowerMulti += base._passiveData.VelueList[0] * 0.01f;
        _state.attackRangeMulti += base._passiveData.VelueList[1] * 0.01f;
        _state.SkillGaugeRecoveryMulti -= base._passiveData.VelueList[2] * 0.01f;
    }

    public override void DeActive()
    {

    }
}
