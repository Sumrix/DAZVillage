using UnityEngine;

public class CameraFacingBillboard :
    MonoBehaviour
{
    void Update()
    {
        transform.LookAt(transform.position + Managers.Player.Camera.transform.rotation * Vector3.forward,
            Managers.Player.Camera.transform.rotation * Vector3.up);
    }
}