using System;
using UnityEngine;

[Serializable]
public class Description
{
    public string Name;
    [TextArea(2, 8)]
    public string Common;
}