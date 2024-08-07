using UnityEngine;

public class PlayerTrackObject : MonoBehaviour
{
    Transform trackTargetTrf;

    private void Awake()
    {
        trackTargetTrf = transform.parent;
        this.transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = trackTargetTrf.position;
    }
}
