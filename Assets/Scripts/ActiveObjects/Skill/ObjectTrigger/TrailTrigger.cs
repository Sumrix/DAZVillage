using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace ActiveObjects
{
    namespace Triggers
    {
        public class TrailTrigger :
            ObjectTrigger
        {
            private Vector3 _lastPosition;
            // Чтобы не активировать на одних и тех же повторно
            private List<Collider> _lastTargets = new List<Collider>();

            protected void Start()
            {
                _lastPosition = transform.position;
            }

            private void Update()
            {
                List<Collider> targets = new List<Collider>();
                var direction = transform.position - _lastPosition;

                foreach (var hit in Physics.RaycastAll(_lastPosition, direction, direction.magnitude))
                {
                    targets.Add(hit.collider);
                }
                _lastPosition = transform.position;

                foreach (var target in targets.Except(_lastTargets))
                {
                    OnSelect(target);
                    OnDeselect(target);
                }
                _lastTargets = targets;
            }
        }
    }
}