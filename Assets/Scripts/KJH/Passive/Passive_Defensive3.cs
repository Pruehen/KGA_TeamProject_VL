using EnumTypes;

public class Passive_Defensive3 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Defensive3);
    }

    public bool IsActive = false;
    public override void Active()//"ġ���� ���ظ� �Ծ��� �� {0}�ʵ��� ü���� {1}���Ϸ� �������� �ʴ´�. (���� �� {2}ȸ)",
    {
        
    }

    public override void DeActive()
    {

    }
}
