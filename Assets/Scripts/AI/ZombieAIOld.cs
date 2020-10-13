using UnityEngine;
using System.Collections;

public class ZombieAIOld : MonoBehaviour
{/*
    private GameObject[] _players; //все игроки сцены

    private Transform _target;
    private GameObject player;
    private Transform defaultTarget; //когда не выбран игрок

    private NavMeshAgent _agent;

    bool cd = false;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        //Установить центр карты(!!!), добавить случайное смещение
        //defaultTarget = GameObject.Find("Arrows").transform;

        _players = GameObject.FindGameObjectsWithTag("Player"); //инициализация массива игроков

        StartCoroutine(SelectTarget()); 
    }

    IEnumerator SelectTarget()
    {
        //поиск ближайшего игрока
        float minDistance = float.MaxValue, distance;
        foreach (GameObject _player in _players)
        {
            distance = Vector3.Distance(_agent.transform.position, _player.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance; player = _player;
            }
        }

        //если самый близкий игрок на расстоянии менее 100 юнитов
        if (minDistance < 100)
        {
            _target = player.transform;
        }
        else
        {
            _target = player.transform; 
        }

        _agent.SetDestination(_target.position);

        yield return new WaitForSeconds(0.1f); //ожидание для экономии ресурсов
        StartCoroutine(SelectTarget()); //поиск игрока
    }

    IEnumerator Attack()
    {
        //test
        
        yield return new WaitForSeconds(0.8f);
        cd = false;
    }

    void Update()
    {
        if (Vector3.Distance(_agent.transform.position, player.transform.position) < 3)
        {
            if (!cd)
            //надо включить анимацию атаки
            {
                StartCoroutine(Attack());
                cd = true;
            }
        }
    }*/
}