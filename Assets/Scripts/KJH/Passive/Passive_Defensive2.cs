using EnumTypes;

public class Passive_Defensive2 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Defensive2);
    }
    public override void Active()//"���� {0}���� óġ�� ������ ���� ü���� �ִ� ü���� {1}%��ŭ ȸ��.",
    {
        base._state.ChangeHp(_state.GetMaxHp() * 0.01f * base._passiveData.VelueList[1]);
    }

    public override void DeActive()
    {

    }
}
