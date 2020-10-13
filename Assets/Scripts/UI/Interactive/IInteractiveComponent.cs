using UnityEngine;
using System.Collections;

namespace GameUI
{
    public interface IInteractiveComponent
    {
        bool IsActive { get; set; }
    }
}