using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    public Transform target;
    [SerializeField]
    private float _distance = 15;

    //движение камеры за персонажем
    void LateUpdate()
    {
		if (target != null) {
			float input = Input.GetAxis ("Mouse ScrollWheel");
			if (input != 0) {
				_distance += -10 * input;
			}
			_distance = Mathf.Clamp (_distance, 10, 30);
			transform.position = transform.forward * (-_distance) + target.position;
		}
    }
}