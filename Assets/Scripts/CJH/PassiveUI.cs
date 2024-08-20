using EnumTypes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveUI : MonoBehaviour
{
    public PassiveID passiveID;

    [SerializeField] Image Image_Icon;//PassiveId �� ����Ǹ� �ش� �̹����� ������.

    public void ImageChange(List<Sprite> iconImage)
    {
        int index = (int)passiveID;
        if (index >= 0 && index < iconImage.Count)
        {
            if (passiveID == PassiveID.None)
            {
                Image_Icon.gameObject.SetActive(false);
            }
            else
            {
                Image_Icon.gameObject.SetActive(true);
                Image_Icon.sprite = iconImage[index];
            }
        }
        else
        {
            Debug.Log("�̹��� ����");
            return;
        }
    }

    public void OnClick()
    {        
        passiveID = PassiveID.None;
        PassiveUIManager.Instance.Command_IconImage(this);
    }
}
