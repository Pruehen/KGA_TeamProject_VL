using UnityEngine;
using UnityEngine.UI;
using EnumTypes;
using System.Collections.Generic;
using System.Linq;

public class BlueChipIcon : MonoBehaviour
{


    [SerializeField] Image iconImage;
    [SerializeField] Text iconName;
    [SerializeField] Text iconInfo;
    [SerializeField] Text iconLevel;

    private HashSet<BlueChipID> iconID = new HashSet<BlueChipID>();

    public void PrintChipData(BlueChipSlot slot)
    {
        if (slot.Level == 0)
        {
            iconName.text = "¾øÀ½";
            iconLevel.text = "";
            iconInfo.text = "";
        }
        else 
        {
            if (iconID.Contains(slot.Id))//°ãÄ¡Áö ¾Ê´Â ID
            {
                Debug.Log("°ãÄ§");
            }
            else
            {

                Debug.Log(123);
                BlueChip data = JsonDataManager.GetBlueChipData(slot.Id);
                iconName.text = data.PrintName();
                iconLevel.text = data.PrintLevel(slot.Level);
                iconInfo.text = data.PrintInfo(slot.Level);
            }
        }
    } 
    
    public void PickBtn_OnClick()
    {
        Debug.Log(this.gameObject.name);
        UIManager.Instance.PickBUtton();
    }
}
