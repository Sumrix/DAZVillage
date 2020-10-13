using UnityEngine;

public class IntRangeAttribute :
    PropertyAttribute
{
    public int MinLimit, MaxLimit;

    public IntRangeAttribute(int minLimit, int maxLimit)
    {
        this.MinLimit = minLimit;
        this.MaxLimit = maxLimit;
    }
}

[System.Serializable]
public class IntRange
{
    public int RangeStart, RangeEnd;

    public int RandomValue
    {
        get { return UnityEngine.Random.Range(RangeStart, RangeEnd); }
    }
}