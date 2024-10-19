using UnityEngine;
using System.Reflection;
using System.Threading.Tasks;
public class UniqueType : MonoBehaviour
{
    public MonoBehaviour component;

    void Start()
    {
        if(component == null)
        {
            return;
        }
        Debug.Log(component.GetType());

        var type = component.GetType();

        var instances = FindObjectsOfType(type);

        foreach(var instance in instances)
        {
            if(instance == component)
            {
                continue;
            }
            var tar = (MonoBehaviour)instance;
            Destroy(tar.gameObject);
        }
    }


}
