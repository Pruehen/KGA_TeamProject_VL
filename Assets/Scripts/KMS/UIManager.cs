using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class UIManager : SceneSingleton<UIManager>
{
    public Image stamina;
    public Image healthPoint;
    public Image skillPoint;
    public TextMeshProUGUI bullet;
    public GameObject inGameUI;
    public GameObject tabUI;
    public GameObject EscUI;

    #region InGameUI
    public void UpdateStamina(float currntStamina, float maxStaminae)
    {
        if(stamina!=null)
        stamina.fillAmount = (float)currntStamina / maxStaminae;
    }
    public void UpdatehealthPoint(int currntHealtPoint, int maxhealthPoint)
    {
        if(healthPoint!=null)
        healthPoint.fillAmount = (float)currntHealtPoint / maxhealthPoint;
    }
    public void UpdateskillPoint(int currntskillPoint, int maxskillPoint)
    {
        if (skillPoint!=null)
        skillPoint.fillAmount = (float)currntskillPoint / maxskillPoint;
    }
    public void UpdateBullet(int currntbullet, int maxbullet)
    {
        if(bullet!=null)
        bullet.text = currntbullet.ToString() + " / " + maxbullet.ToString();
    }
    
    #endregion

    #region TabUI

    #endregion
    public void UpdatePlayerState(int HP, int Stamina,int Atk,int moveSpeed)
    {

    }
    #region EscUI

    #endregion
    public void PopUpUI(GameObject UI)
    {

    }
    public void CloseUI(GameObject UI)
    {

    }
}
