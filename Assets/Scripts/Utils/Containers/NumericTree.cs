using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

public class NumericTree :
    IEnumerable<KeyValuePair<string, float>>,
    IEnumerable
{
    public float this[string key]
    {
        get { return GetNumber(key); }
        set { SetNumber(key, value); }
    }
    public IEnumerable<KeyValuePair<string, float>> Numbers
    {
        get
        {
            return GetType()
                .GetFields()
                .Where(x => x.FieldType == typeof(float))
                .Select(x => new KeyValuePair<string, float>(x.Name, (float)x.GetValue(this)));
        }
    }
    public IEnumerable<KeyValuePair<string, NumericTree>> Branches
    {
        get
        {
            return GetType()
                .GetFields()
                .Where(x => x.FieldType.IsSubclassOf(typeof(NumericTree)))
                .Select(x => new KeyValuePair<string, NumericTree>(x.Name, (NumericTree)x.GetValue(this)));
        }
    }
    public IEnumerable<KeyValuePair<string, object>> Items
    {
        get
        {
            return GetType()
                .GetFields()
                .Where(x => (x.FieldType.IsSubclassOf(typeof(NumericTree))
                    || x.FieldType == typeof(float)) && !x.IsSpecialName)
                .Select(x => new KeyValuePair<string, object>(x.Name, x.GetValue(this)));
        }
    }
    public int Count
    {
        get { return this.Count(); }
    }
    public float GetNumber(string key)
    {
        return (float)GetItem(key);
    }
    public void SetNumber(string key, float value)
    {
        int endIndex = key.IndexOf('.');
        if (endIndex > 0)
        {
            string name = key.Substring(0, endIndex);
            var obj = GetType().GetField(name).GetValue(this);
            ((NumericTree)obj).SetNumber(key.Substring(endIndex + 1), value);
        }
        else
        {
            GetType().GetField(key).SetValue(this, value);
        }
    }
    public NumericTree GetBranch(string key)
    {
        return (NumericTree)GetItem(key);
    }
    public object GetItem(string key)
    {
        int endIndex = key.IndexOf('.');
        string name = endIndex > 0 ? key.Substring(0, endIndex) : key;
        var obj = GetType().GetField(name).GetValue(this);
        if (endIndex > 0)
        {
            return ((NumericTree)obj).GetItem(key.Substring(endIndex + 1));
        }
        else
        {
            return obj;
        }
    }
    public bool ContainsKey(string key)
    {
        int endIndex = key.IndexOf('.');
        string name = endIndex < 0 ? key : key.Substring(0, endIndex);
        var obj = GetType().GetField(name).GetValue(this);
        if (obj == null)
        {
            return false;
        }
        else
        {
            if (endIndex > 0)
            {
                return ((NumericTree)obj).ContainsKey(key.Substring(endIndex + 1));
            }
            else
            {
                return true;
            }
        }
    }
    public IEnumerator<KeyValuePair<string, float>> GetEnumerator()
    {
        foreach (var arr in Branches)
        {
            foreach (var prop in arr.Value)
            {
                yield return new KeyValuePair<string, float>(
                    string.Format("{0}.{1}", arr.Key, prop.Key),
                    prop.Value);
            }
        }
        foreach (var prop in Numbers)
        {
            yield return prop;
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public override string ToString()
    {
        StringBuilder text = new StringBuilder();
        text.Append("{");
        foreach (var arr in Branches)
        {
            text.Append(arr.Key);
            text.Append(":");
            text.Append(arr.Value);
            text.Append(",");
        }
        foreach (var num in Numbers)
        {
            text.Append(num.Key);
            text.Append(":");
            text.Append(num.Value);
            text.Append(",");
        }
        if (text[text.Length - 1] == ',')
            text.Remove(text.Length - 1, 1);
        text.Append("}");
        return text.ToString();
    }
    public static T Operate<T>(
        T operand1,
        T operand2,
        Func<float, float, float> func)
        where T : NumericTree
    {
        T result = (T)Activator.CreateInstance(operand1.GetType());

        var e1 = operand1.GetType().GetFields().GetEnumerator();
        var t2 = operand2.GetType();
        var t3 = result.GetType();

        while (e1.MoveNext())
        {
            var item1 = (FieldInfo)e1.Current;
            var item2 = t2.GetField(item1.Name);
            var item3 = t3.GetField(item1.Name);

            if (item2 != null && item3 != null)
            {
                if (item1.FieldType == typeof(float))
                {
                    item3.SetValue(result, func(
                        (float)item1.GetValue(operand1),
                        (float)item2.GetValue(operand2)));
                }
                else if (item1.FieldType.IsSubclassOf(typeof(NumericTree)))
                {
                    item3.SetValue(result, Operate(
                        (NumericTree)item1.GetValue(operand1),
                        (NumericTree)item2.GetValue(operand2),
                        func));
                }
            }
        }
        return result;
    }
    public static T Operate<T>(
        T operand1,
        Func<float, float> func)
        where T : NumericTree
    {
        T result = (T)Activator.CreateInstance(operand1.GetType());

        var e1 = operand1.GetType().GetFields().GetEnumerator();
        var t3 = result.GetType();

        while (e1.MoveNext())
        {
            var item1 = (FieldInfo)e1.Current;
            var item3 = t3.GetField(item1.Name);

            if (item3 != null)
            {
                if (item1.FieldType == typeof(float))
                {
                    item3.SetValue(result, func(
                        (float)item1.GetValue(operand1)));
                }
                else if (item1.FieldType.IsSubclassOf(typeof(NumericTree)))
                {
                    item3.SetValue(result, Operate(
                        (NumericTree)item1.GetValue(operand1),
                        func));
                }
            }
        }
        return result;
    }
}