using UnityEngine;
using Zenject;

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //Greeter�� ���ε�
        Container.Bind<Greeter>().AsSingle();

       
      
    }

}

public class Greeter
{
    public Greeter()
    {
        Debug.Log("Greeter�Լ� �̴�.");
    }
    public void LogMsg(string masage)
    { 
      Debug.Log("LogMsg : " + masage);
    }

}
