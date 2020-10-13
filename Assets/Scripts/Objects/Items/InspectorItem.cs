using UnityEngine;
using System;

/// <summary>
/// Представление предмета в инспекторе. Может хранить как префаб, так и инстанс предмета.
/// </summary>
[Serializable]
public class InspectorItem
{
    public Item Prefab;
    public int Count;
    
    /// <summary>
    /// Возвращяет инстанс предмета
    /// </summary>
    /// <returns>Инстантиированный предмет</returns>
    public Item Instantiate()
    {
        if (Prefab)
        {
            var instance = GameObject.Instantiate(Prefab);
            if (instance.IsStackable)
            {
                instance.Stack.Current = Count;
            }
            return instance;
        }
        return null;
    }
    /// <summary>
    /// Инстантиирует предмет с инициализацией количества и transform
    /// </summary>
    /// <param name="parent">Родительский объект</param>
    /// <param name="worldPositionStays">Конвертирование систем координат</param>
    /// <returns>Инстанциированный предмет</returns>
    private Item Instantiate(Transform parent, bool worldPositionStays)
    {
        var instance = Instantiate();
        if (instance)
        {
            instance.transform.SetParent(parent, worldPositionStays);
        }
        return instance;
    }
    /// <summary>
    /// Краткая информация о предмете
    /// </summary>
    /// <returns>Строка с краткой информацией о предмете</returns>
    public override string ToString()
    {
        return string.Format("{0}:{1}", Count, Prefab.ID);
    }
}
