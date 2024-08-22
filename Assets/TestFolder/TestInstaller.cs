using UnityEngine;
using Zenject;

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //Greeter을 바인딩
        Container.Bind<Greeter>().AsSingle();

       
      
    }

}

public class Greeter
{
    public Greeter()
    {
        Debug.Log("Greeter함수 이다.");
    }
    public void LogMsg(string masage)
    { 
      Debug.Log("LogMsg : " + masage);
    }

}
