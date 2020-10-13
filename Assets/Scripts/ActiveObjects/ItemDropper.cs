using UnityEngine;
using System;

namespace ActiveObjects
{
    [RequireComponent(typeof(Character))]
    public class ItemDropper :
        MonoBehaviour
    {
        public DropItem[] DropItems;

        private Transform _droppedItems;
        private Character _character;

        private void Awake()
        {
            _character = GetComponent<Character>();
            _character.Dead += ItemDropper_Dead;
            _droppedItems = GameObject.Find("DroppedItems").transform;
        }

        private void ItemDropper_Dead(object sender, DeadEventArgs e)
        {
            // Чтобы не было повторных вызовов
            _character.Dead -= ItemDropper_Dead;

            foreach (var dropItem in DropItems)
            {
                if (UnityEngine.Random.value < dropItem.Probability)
                {
                    var item = dropItem.Instantiate(transform, false);
                    item.transform.SetParent(_droppedItems);
                }
            }
        }
    }
}

[Serializable]
public class DropItem
{
    public Item Prefab;
    [Range(0.0f, 1.0f)]
    public float Probability = 1;
    [IntRange(1, 30)]
    public IntRange CountRange;

    public Item Instantiate()
    {
        if (Prefab != null)
        {
            var instance = GameObject.Instantiate(Prefab);
            if (instance.IsStackable)
            {
                instance.Stack.Current = CountRange.RandomValue;
            }
            return instance;
        }
        return null;
    }
    public Item Instantiate(Transform parent, bool worldPositionStays)
    {
        var instance = Instantiate();
        if (instance)
        {
            instance.transform.SetParent(parent, worldPositionStays);
        }
        return instance;
    }
}