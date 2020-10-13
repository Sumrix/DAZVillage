using UnityEngine;

namespace ActiveObjects
{
    public class SlowDestroyer :
        MonoBehaviour
    {
        public float Delay;
        public float Duration;
        public Vector3 Speed;

        private float _startTime;
        private float _endTime;

        private void Start()
        {
            _startTime = Time.time + Delay;
            _endTime = _startTime + Duration;
        }
        private void Update()
        {
            if (_startTime < Time.time)
            {
                if (_endTime < Time.time)
                {
                    this.Destroy();
                }
                else
                {
                    transform.Translate(Time.deltaTime * Speed);
                }
            }
        }
    }
}