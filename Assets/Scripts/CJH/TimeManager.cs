using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    //�̱���

    public static TimeManager instance;

    private void Awake()
    {
        instance = this;
    }

    //�ð� ����
    public void TimeStop()
    {
        Time.timeScale = 0;
    }
    public void TimeStart()
    {
        Time.timeScale = 1;
    }


  


}
