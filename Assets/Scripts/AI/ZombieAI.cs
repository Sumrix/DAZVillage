using UnityEngine;
using System.Collections;

public class ZombieAI :
    MonoBehaviour,
    IManagerWaiter
{
    private Transform _target;
    public UnityEngine.AI.NavMeshAgent Agent;
    // Когда управление на себя берёт другой модуль
    public bool IsBusy
    {
        set
        {
            if (_target != null)
            {
                if (value)
                {
                    Agent.Stop();
                }
                else
                {
                    Agent.Resume();
                }
            }
        }
    }

	private void Start ()
    {
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Managers.AddWaiter(this);
    }
    public void Startup()
    {
        if (Managers.Player.Player != null)
        {
            _target = Managers.Player.Player.transform;
        }
    }
    private void Update()
    {
        if (_target != null)
        {
            Agent.SetDestination(_target.position);
        }
    }
}
