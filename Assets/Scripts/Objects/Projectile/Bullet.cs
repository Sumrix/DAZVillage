using UnityEngine;

public class Bullet :
    Projectile
{
    [SerializeField]
    public float _speed = 30;
    protected void Update()
    {
        if (IsActive)
        {
            float offset = _speed * Time.deltaTime;
            float distance = Vector3.Distance(transform.position, Target);
            if (!Mathf.Approximately(distance, 0))
            {
                transform.position = Vector3.MoveTowards(transform.position, Target, offset);
            }
            else
            {
                Stop();
            }
        }
    }
}
