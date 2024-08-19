using EnumTypes;

public class Passive_Offensive1 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Offensive1);
        HpCheckRetio = _passiveData.VelueList[0] * 0.01f;
    }
    public float HpCheckRetio {  get; private set; }
    public override void Active()//"���� ü���� {0}% �̻��� ��, �Ϲ�&Ư�� ���ݷ��� {1}% ����",
    {
        base._state.AttackPowerMulti += base._passiveData.VelueList[1] * 0.01f;
        base._state.SkillPowerMulti += base._passiveData.VelueList[2] * 0.01f;
    }

    public override void DeActive()
    {
        base._state.AttackPowerMulti -= base._passiveData.VelueList[1] * 0.01f;
        base._state.SkillPowerMulti -= base._passiveData.VelueList[2] * 0.01f;
    }
}
