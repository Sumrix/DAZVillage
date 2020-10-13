using UnityEngine;

// Адаптер между MonoBehaviour и ProjectileBehavior
public class Projectile :
    MonoBehaviour
{
    [HideInInspector]
    // Цель
    public Vector3 Target;
    [HideInInspector]
    // Компонент работает
    public bool IsActive = true;
    // Задержка перед исчезновением
    public float DestroyingDelay;

    protected void Start()
    {
        transform.LookAt(Target);
    }
    protected void Stop()
    {
        if (IsActive)
        {
            IsActive = false;
            DelayHelper.DelayedAction(() =>
            {
                Destroy(gameObject);
            }, DestroyingDelay);
        }
    }
}