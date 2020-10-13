using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameUI
{
    /// <summary>
    /// Элемент представления коллекции
    /// </summary>
    public class CollectionViewItem :
        MonoBehaviour
    {
        [HideInInspector]
        public CollectionView CollectionView;
        
        /// <summary>
        /// Обновить данные CollectionViewItem
        /// </summary>
        /// <param name="value">Новые данные</param>
        public virtual void Upd(object value)
        {
        }
    }
}