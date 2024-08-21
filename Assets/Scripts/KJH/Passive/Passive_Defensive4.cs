using EnumTypes;

public class Passive_Defensive4 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Defensive4);
    }

    public float Value_DamageReductionPercentage { get; private set; }
    public override void Active()//"���� óġ �� ������ ���� �������� �Դ� ���ذ� {0}%�� �����Ѵ� (�ִ� {1}%)",
    {
        Value_DamageReductionPercentage += base._passiveData.VelueList[0];
        if(Value_DamageReductionPercentage > base._passiveData.VelueList[1])
        {
            Value_DamageReductionPercentage = base._passiveData.VelueList[1];
        }
    }

    public override void DeActive()
    {
        Value_DamageReductionPercentage = 0;
    }
}
