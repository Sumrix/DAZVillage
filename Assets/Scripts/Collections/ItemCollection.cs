using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using GameUI;

namespace Collections
{
    /// <summary>
    /// Статический класс реализующий методы работы с коллекциями предметов
    /// </summary>
    public static class ItemCollection
    {
        /// <summary>
        /// Посчитать количество Item в коллекции
        /// </summary>
        /// <param name="items">Коллекция где подсчитывается количество предметов</param>
        /// <param name="item">Тип предмета для посчёта</param>
        /// <returns>Количество предметов</returns>
        public static int Count(IEnumerable<Item> items, Item item)
        {
            var targetItems = items
                .OfType<Item>()
                .Where(x => x.ID == item.ID);

            return item.IsStackable
                ? Mathf.RoundToInt(targetItems.Sum(x => x.Stack.Current))
                : targetItems.Count();
        }

        /// <summary>
        /// Содержет ли коллекция достаточное количество определённых предметов
        /// </summary>
        /// <param name="items">Коллекция где производится операция</param>
        /// <param name="item">Тип продсчитываемого предмета</param>
        /// <param name="count">Количество предметов</param>
        /// <returns>true - если содержет достаточное количество определённых предметов, иначе - false</returns>
        public static bool Contains(IEnumerable<Item> items, Item item, int count)
        {
            return Count(items, item) >= count;
        }

        /// <summary>
        /// Содержет ли коллекция достаточное количество определённых предметов
        /// </summary>
        /// <param name="items">Коллекция где производится операция</param>
        /// <param name="item">Данные о посчитываемом предмете</param>
        /// <returns>true - если содержет достаточное количество определённых предметов, иначе - false</returns>
        public static bool Contains(IEnumerable<Item> items, InspectorItem item)
        {
            return Contains(items, item.Prefab, item.Count);
        }

        /// <summary>
        /// Содержет ли коллекция достаточное количество определённых предметов
        /// </summary>
        /// <param name="items">Коллекция где производится операция</param>
        /// <param name="item">Данные о посчитываемом предмете</param>
        /// <returns>true - если содержет достаточное количество определённых предметов, иначе - false</returns>
        public static bool Contains(IEnumerable<Item> items, IEnumerable<InspectorItem> checkItems)
        {
            return checkItems.All(item => Contains(items, item.Prefab, item.Count));
        }

        /// <summary>
        /// Удалить count предметов типа item из коллекции 
        /// </summary>
        /// <param name="items">Коллекция где будет производиться удаление</param>
        /// <param name="item">Тип предмета для удаления</param>
        /// <param name="count">Количество предметов для удаления</param>
        public static void Remove(IList<Item> items, Item item, int count)
        {
            int amount = 0;

            for (int i = items.Count - 1; i >= 0 && amount < count; i--)
            {
                var current = items[i];
                if (current != null && current.ID == item.ID)
                {
                    amount += Mathf.RoundToInt(current.Stack.Current);
                    if (amount <= count)
                    {
                        items[i] = null;
                        current.Destroy();
                    }
                    else
                    {
                        current.Stack.Current = amount - count;
                        // Если это был IObservableCollection, то надо вызвать событие обновления
                        var collection = items as IObservableCollection;
                        if (collection != null)
                        {
                            collection.ResetItem(i);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Удалить предмет из коллекции
        /// </summary>
        /// <param name="items">Коллекция где будет производиться удаление</param>
        /// <param name="item">Данные о предмете для удаления</param>
        public static void Remove(IList<Item> items, InspectorItem item)
        {
            Remove(items, item.Prefab, item.Count);
        }

        /// <summary>
        /// Удалить предметы из коллекции
        /// </summary>
        /// <param name="items">Коллекция где будет производиться удаление</param>
        /// <param name="removeItems">Данные о предметах для удаления</param>
        public static void Remove(IList<Item> items, IEnumerable<InspectorItem> removeItems)
        {
            foreach (var removeItem in removeItems)
            {
                Remove(items, removeItem);
            }
        }

        /// <summary>
        /// Заполняет свободные места в списке предметами. ПРЕДМЕТ КОПИРУЕТСЯ.
        /// </summary>
        /// <param name="items">Список предметов</param>
        /// <param name="newitem">Добавляемый предмет</param>
        /// <param name="count">Количество предметов для добавления</param>
        /// <returns>Не поместившиеся предметы</returns>
        public static Item Fill(IList<Item> items, Item newItem, int count)
        {
            var addingItem = GameObject.Instantiate(newItem);
            addingItem.Stack.Current = count;

            if (addingItem.IsStackable)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    if (item != null)
                    {
                        if (item.FillFrom(addingItem))
                        {
                            // Если это был IObservableCollection, то надо вызвать событие обновления
                            var collection = items as IObservableCollection;
                            if (collection != null)
                            {
                                collection.ResetItem(i);
                            }
                        }
                        if (addingItem.Stack.IsEmpty)
                        {
                            addingItem.Destroy();
                            return null;
                        }
                    }
                }
            }
            
            if (addingItem.Stack.Current > 0.8)
            {
                int index = items.IndexOf(null);
                if (index >= 0)
                {
                    items[index] = addingItem;
                }
                else
                {
                    return addingItem;
                }
            }
            else
            {
                addingItem.Destroy();
            }

            return null;
        }

        /// <summary>
        /// Заполняет свободные места в списке предметами. ПРЕДМЕТ КОПИРУЕТСЯ.
        /// </summary>
        /// <param name="items">Список предметов</param>
        /// <param name="newitem">Информация о добавляемом предмете</param>
        /// <returns>Не поместившиеся предметы</returns>
        public static Item Fill(IList<Item> items, InspectorItem newItem)
        {
            return Fill(items, newItem.Prefab, newItem.Count);
        }

        /// <summary>
        /// Заполняет свободные места в списке предметами. ПРЕДМЕТ КОПИРУЕТСЯ.
        /// </summary>
        /// <param name="items">Список предметов</param>
        /// <param name="newItems">Информация о добавляемых предметах</param>
        /// <returns>Не поместившиеся предметы</returns>
        public static IEnumerable<Item> Fill(IList<Item> items, IEnumerable<InspectorItem> newItems)
        {
            foreach (var newItem in newItems)
            {
                yield return Fill(items, newItem);
            }
        }
        /// <summary>
        /// Удалить с проверкой count предметов типа item из коллекции 
        /// </summary>
        /// <param name="items">Коллекция где будет производиться удаление</param>
        /// <param name="item">Тип предмета для удаления</param>
        /// <param name="count">Количество предметов для удаления</param>
        /// <returns>true - если удаление успешно, иначе - false</returns>
        public static bool TryRemove(IList<Item> items, Item item, int count)
        {
            if (Contains(items, item, count))
            {
                Remove(items, item, count);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Удалить с проверкой предмет из коллекции
        /// </summary>
        /// <param name="items">Коллекция где будет производиться удаление</param>
        /// <param name="item">Данные о предмете для удаления</param>
        /// <returns>true - если удаление прошло успешно, иначе - false</returns>
        public static bool TryRemove(IList<Item> items, InspectorItem item)
        {
            if (Contains(items, item))
            {
                Remove(items, item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Удалить с проверкой предметы из коллекции
        /// </summary>
        /// <param name="items">Коллекция где будет производиться удаление</param>
        /// <param name="removeItems">Данные о предметах для удаления</param>
        /// <returns>true - если удаление успешно, иначе - false</returns>
        public static bool TryRemove(IList<Item> items, IEnumerable<InspectorItem> removeItems)
        {
            if (Contains(items, removeItems))
            {
                Remove(items, removeItems);
                return true;
            }
            return false;
        }
    }
}