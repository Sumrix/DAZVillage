using UnityEngine;
using System.Collections;

namespace GameUI
{
    public class DescriptionPanel :
        MonoBehaviour
    {
        public void Set(object info)
        {
            if (info == null)
            {
                SetDefault();
            }
            else
            {
                SetItem(info);
            }
        }
        public virtual void SetItem(object info)
        {

        }
        public virtual void SetDefault()
        {
        }
    }
}