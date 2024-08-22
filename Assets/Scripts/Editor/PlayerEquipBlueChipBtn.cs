using UnityEngine;
using UnityEditor;
using EnumTypes;

[CustomEditor(typeof(PlayerEquipBlueChip))]
[CanEditMultipleObjects]
public class CubeGenerateButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PlayerEquipBlueChip playerEquipBlueChip = (PlayerEquipBlueChip)target;
        if (GUILayout.Button("�ٰŸ�1 Ĩ �߰� (��¡ ü�½��� ��� �߰���, ���� ���)"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Melee1);
        }
        if (GUILayout.Button("�ٰŸ�2 Ĩ �߰� (������� ����, Ÿ�� �� ���� ȹ��)"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Melee2);
        }
        if (GUILayout.Button("���Ÿ�1 Ĩ �߰� (�Ϲݰ��� Ÿ�� ����)"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Range1);
        }
        if (GUILayout.Button("���Ÿ�2 Ĩ �߰� (���Ÿ� ��Ÿ Ȯ�� ������ ��ų �ߵ�)"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Range2);
        }
        if (GUILayout.Button("���̺긮��1 Ĩ �߰� (��� ��ȯ�� ��ȸ Ÿ�� ������ ����)"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Hybrid1);
        }
        if (GUILayout.Button("���̺긮��2 Ĩ �߰� (�� �ʸ��� �ڵ����� ��� ��ȯ, ���� �ڿ� �Ҹ� ����)"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Hybrid2);
        }
        if (GUILayout.Button("����1 Ĩ �߰� (��� ���� �ٰŸ� ȿ�� �߰�)"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Generic1);
        }
        if (GUILayout.Button("����2 Ĩ �߰� (�Ҹ� �� ���ط� ����)"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Generic2);
        }
        if (GUILayout.Button("����3 Ĩ �߰� (��ų �������� ��� ���� ����, ���� ���� �ð� �ʰ��� ��ų������ 0)"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Generic3);
        }
        if (GUILayout.Button("��� Ĩ ����"))
        {
            playerEquipBlueChip.GetBlueChipDic().Clear();
        }
    }
}
