using UnityEngine;
using System.Collections;
using Collections;

namespace GameUI
{
    public class ItemCollectionView :
        CollectionView
    {
        protected override bool Move(IObservableCollection dstList, int dstIndex, object srcValue,
            IObservableCollection srcList, int srcIndex, object dstValue)
        {
            if (srcValue != null && dstValue != null)
            {
                var srcItem = (Item)srcValue;
                var dstItem = (Item)dstValue;
                srcItem.FillFrom(dstItem);
                if (dstItem.Stack.IsEmpty)
                {
                    dstItem.Destroy();
                    dstValue = null;
                }
            }
            return base.Move(dstList, dstIndex, srcValue, srcList, srcIndex, dstValue);
        }
    }
}