using UnityEngine;

public class PlayerTrackObject : MonoBehaviour
{
    Transform trackTargetTrf;

    PlayerCameraMove Cam;

    private void Awake()
    {
        Cam = GetComponentInChildren<PlayerCameraMove>();
        trackTargetTrf = transform.parent;
        this.transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (trackTargetTrf != null)
            this.transform.position = trackTargetTrf.position;
        else
        {
            Destroy(Cam);
            return;
        }
           



    }
}
