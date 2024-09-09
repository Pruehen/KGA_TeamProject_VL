using EnumTypes;

public class Passive_Offensive3 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Offensive3);
    }
    public override void Active()//"�Ϲ�&Ư�� ���ݷ��� {0, 1}% ����, �뽬 �ð� {2}% ���� �� ���׹̳� �Ҹ� {3}% ����",
    {
        base._state.AttackPowerMulti += base._passiveData.VelueList[0] * 0.01f;
        base._state.SkillPowerMulti += base._passiveData.VelueList[1] * 0.01f;
        base._state.DashTimeMulti -= base._passiveData.VelueList[2] * 0.01f;
        base._state.DashCostMulti += base._passiveData.VelueList[3] * 0.01f;
    }

    public override void DeActive()
    {

    }
}
