using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Объект главного потока.
/// Позволяет вызывать делегаты в главном потоке.
/// </summary>
public class MainThread :
    MonoBehaviour
{
    /// <summary>
    /// Очередь делегатов на вызов.
    /// </summary>
    private static  Queue<Action> _actions;

    static MainThread()
    {
        _actions = new Queue<Action>();
    }
    /// <summary>
    /// Вызов делегата в главном потоке.
    /// Добавляет делегат в очередь вызова.
    /// </summary>
    /// <param name="action">Делегат для вызова</param>
    public static void Invoke(Action action)
    {
        _actions.Enqueue(action);
    }
    /// <summary>
    /// Метод главного потока. Здесь вызываются все делегаты и удаляются из очереди.
    /// </summary>
	private void Update ()
    {
        for (int i = 0; i < _actions.Count; i++)
        {
            _actions.Dequeue()();
        }
	}
}
