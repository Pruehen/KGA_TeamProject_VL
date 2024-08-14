using UnityEngine;
using UnityEngine.UI;
using EnumTypes;
using System.Collections.Generic;

public class BlueChipIcon : MonoBehaviour
{


    [SerializeField] Image iconImage;
    [SerializeField] Text iconName;
    [SerializeField] Text iconInfo;
    [SerializeField] Text iconLevel;

    private HashSet<BlueChipID> iconID = new HashSet<BlueChipID>();

    public void PrintChipData(int chipLevel, BlueChipID chipID)
    {
        if (chipLevel == 0)
        {
            iconName.text = "����";
            iconLevel.text = "";
            iconInfo.text = "";
        }
        else 
        {
            if (iconID.Contains(chipID))//��ġ�� �ʴ� ID
            {
                Debug.Log("��ħ");
            }
            else
            {

                Debug.Log(123);
                BlueChip data = JsonDataManager.GetBlueChipData(chipID);
                iconName.text = data.PrintName();
                iconLevel.text = data.PrintLevel(chipLevel);
                iconInfo.text = data.PrintInfo(chipLevel);
            }



        }
    } 
    
      
}
