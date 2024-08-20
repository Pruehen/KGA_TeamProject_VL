using EnumTypes;

public class Passive_Defensive3 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Defensive3);
        ActiveCount = (int)base._passiveData.VelueList[2];
        ActiveTime = base._passiveData.VelueList[0];
        HpHoldValue = base._passiveData.VelueList[1];
    }

    public int ActiveCount { get; private set; }
    public float ActiveTime { get; private set; }
    public float HpHoldValue { get; private set; }
    public override void Active()//"ġ���� ���ظ� �Ծ��� �� {0}�ʵ��� ü���� {1}���Ϸ� �������� �ʴ´�. (���� �� {2}ȸ)",
    {
    }
    public void Active(out float holdTime)
    {
        ActiveCount--;
        holdTime = ActiveTime;
    }

    public override void DeActive()
    {

    }
}
