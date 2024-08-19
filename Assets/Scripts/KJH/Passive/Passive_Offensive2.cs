using EnumTypes;

public class Passive_Offensive2 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Offensive2);
        CheckDistance_ToEnemy = _passiveData.VelueList[0];
        DmgGain = 1 + (_passiveData.VelueList[1] * 0.01f);
    }
    public float CheckDistance_ToEnemy { get; private set; }
    public float DmgGain { get; private set; }
    public override void Active()//"{0}m���� ������ �ִ� ������ ���ϴ� �������� {1}%��ŭ ����",
    {

    }

    public override void DeActive()
    {

    }
}
