using UnityEngine;
using ActiveObjects.Triggers;
using System.Linq;
using System.Collections.Generic;

namespace ActiveObjects
{
    [RequireComponent(typeof(Character))]
    public class AutoAiming :
        MonoBehaviour
    {
        [RequiredField]
        public FieldTrigger FieldTrigger;
        public TimerTrigger Timer;
        public float RotationSpeed;
        public LayerMask IgnoreLayerMask = -1;

        private Character _target;
        private Character _character;

        private void Awake()
        {
            _character = GetComponent<Character>();

            FieldTrigger.Active += FieldTrigger_Active;
            Timer.Active += Timer_Active;

            FieldTrigger.Enable();
            Timer.Enable();
        }
        private void FieldTrigger_Active(object sender, System.EventArgs e)
        {
            SelectTarget();
        }
        private void Timer_Active(object sender, System.EventArgs e)
        {
            MainThread.Invoke(SelectTarget);
        }
        private void SelectTarget()
        {
            var characters = new List<Character>(FieldTrigger.SelectList.OfType<Character>());
            _target = characters
                .Where(x => x.Team != _character.Team)
                .OrderBy(x => Vector3.Distance(x.transform.position, transform.position))
                .FirstOrDefault(x => !Physics.Linecast(x.transform.position, transform.position));
        }
        private void Update()
        {
            if (_target && Managers.Player && Managers.Player.AutoAiming)
            {
                Vector3 direction = _target.transform.position - transform.position;
                direction.y = 0;
                Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z);
                float step = RotationSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(forward, direction, step, 0.0F);
                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
    }
}