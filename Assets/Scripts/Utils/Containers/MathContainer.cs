using System;
using System.Collections.Generic;

public class MathContainer :
    SortedDictionary<int, float>
{
    public MathContainer() : base() { }
    public MathContainer(MathContainer o) : base(o) { }
    public static MathContainer operator +(MathContainer a, MathContainer b)
    {
        MathContainer result = new MathContainer();
        IEnumerator<KeyValuePair<int, float>> ieA, ieB;
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
                result.Add(ieA.Current.Key, ieA.Current.Value + ieB.Current.Value);
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
    public MathContainer Copy()
    {
        var newContainer = new MathContainer();
        foreach (var item in this)
        {
            newContainer.Add(item.Key, item.Value);
        }
        return newContainer;
    }
}