using System;
using System.Collections.Generic;

public class MathContainerT<T> :
    SortedDictionary<int, T>
    where T : ISummable<T>
{
    public MathContainerT() : base() { }
    public MathContainerT(MathContainerT<T> o) : base(o) { }
    public static MathContainerT<T> operator +(MathContainerT<T> a, MathContainerT<T> b)
    {
        MathContainerT<T> result = new MathContainerT<T>();
        IEnumerator<KeyValuePair<int, T>> ieA, ieB;
        ieA = a.GetEnumerator();
        ieB = b.GetEnumerator();
        bool isEndA, isEndB;
        isEndA = !ieA.MoveNext();
        isEndB = !ieB.MoveNext();

        while (!(isEndA || isEndB))
        {
            while (ieA.Current.Key < ieB.Current.Key)
            {
                result.Add(ieA.Current.Key, ieA.Current.Value);
                if (!ieA.MoveNext()) { isEndA = true; break; }
            }
            while (ieA.Current.Key > ieB.Current.Key)
            {
                result.Add(ieB.Current.Key, ieB.Current.Value);
                if (!ieB.MoveNext()) { isEndB = true; break; }
            }
            while (ieA.Current.Key == ieB.Current.Key && !(isEndA || isEndB))
            {
                result.Add(ieA.Current.Key, ieA.Current.Value.Sum(ieB.Current.Value));
                isEndA = !ieA.MoveNext();
                isEndB = !ieB.MoveNext();
            }
        }
        if (!isEndA)
            do {
                result.Add(ieA.Current.Key, ieA.Current.Value);
            } while (ieA.MoveNext());
        if (!isEndB)
            do {
                result.Add(ieB.Current.Key, ieB.Current.Value);
            } while (ieB.MoveNext());

        return result;
    }
}