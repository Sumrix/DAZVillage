using UnityEngine;

public class FloatRangeAttribute :
    PropertyAttribute
{
    public float MinLimit, MaxLimit;

    public FloatRangeAttribute(float minLimit, float maxLimit)
    {
        this.MinLimit = minLimit;
        this.MaxLimit = maxLimit;
    }
}

[System.Serializable]
public class FloatRange
{
    public float RangeStart, RangeEnd;

    public FloatRange(float start = 0, float end = 1)
    {
        RangeStart = start;
        RangeEnd = end;
    }
    public float RandomValue
    {
        get { return UnityEngine.Random.Range(RangeStart, RangeEnd); }
    }
    public bool InRange(float value)
    {
        return value > RangeStart && value < RangeEnd;
    }
    public float Size
    {
        get { return RangeEnd - RangeStart; }
    }
}