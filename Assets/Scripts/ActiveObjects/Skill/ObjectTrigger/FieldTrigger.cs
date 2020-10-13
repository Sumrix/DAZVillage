using UnityEngine;

namespace ActiveObjects
{
    namespace Triggers
    {
        public class FieldTrigger :
            ObjectTrigger
        {
            private void OnTriggerEnter(Collider other)
            {
                OnSelect(other);
            }
            private void OnTriggerExit(Collider other)
            {
                OnDeselect(other);
            }
        }
    }
}