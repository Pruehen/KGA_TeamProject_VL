using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    //½Ì±ÛÅæ

    public static TimeManager instance;

    private void Awake()
    {
        instance = this;
    }

    //½Ã°£ Á¤Áö
    public void TimeStop()
    {
        Time.timeScale = 0;
    }
    public void TimeStart()
    {
        Time.timeScale = 1;
    }


  


}
