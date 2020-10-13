using System;
using UnityEngine;

[Serializable]
public class Points :
    NumericTree
{
    [SerializeField]
    [Range(0,1)]
    public float CurrentNormalized = 0f;

    public bool IsFull
    {
        get { return Mathf.Approximately(Current, Maximum); }
    }
    public bool IsEmpty
    {
        get { return Mathf.Approximately(Current, 0); }
    }
    public float Maximum;
    public float Current
    {
        get { return Maximum * CurrentNormalized; }
        set
        {
            if (Maximum > Mathf.Epsilon)
            {
                var percentageNew = value / Maximum;
                CurrentNormalized = Mathf.Clamp(percentageNew, 0, 1);
            }
        }
    }
}