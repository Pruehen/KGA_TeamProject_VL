using EnumTypes;

public class Passive_Offensive5 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Offensive5);
        ValueChangeRatio = _passiveData.VelueList[0] * 0.01f;
    }
    public float ValueChangeRatio { get; private set; }
    public override void Active()//"Ư�� ���ݷ��� {0}%��ŭ�� �Ϲ� ���ݷ����� ġȯ",
    {

    }

    public override void DeActive()
    {

    }
}
