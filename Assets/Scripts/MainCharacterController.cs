using CnControls;
using UnityEngine;
using System.Linq;
using ActiveObjects.GameSkill;

[RequireComponent(typeof(CharacterController))]
public class MainCharacterController :
    MonoBehaviour
{
    public float moveSpeed = 20.0f;
    public float paceSpeed;
    public float rotSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -20.0f;
    public float minFall = -1.5f;

    private float _vertSpeed;

    private CharacterController _charController;
    private float _camRayLength = 100f;
    //private Animator _animator;
    [HideInInspector]
    // Когда управление на себя берёт другой модуль
    public bool IsBusy;
    private Character _character;

    // Use this for initialization
    void Start()
    {
        _vertSpeed = minFall;
        _charController = GetComponent<CharacterController>();
        _character = GetComponent<Character>();
        //_animator = GetComponent<Animator>();
        paceSpeed = moveSpeed / 2;
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsBusy)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
            }

            // start with zero and add movement components progressively
            Vector3 movement = Vector3.zero;

            // x z movement transformed relative to input
            float horInput = CnInputManager.GetAxis("Horizontal");
            float vertInput = CnInputManager.GetAxis("Vertical");


            float diffMagnitude = new Vector2(horInput, vertInput).magnitude;
            // face movement direction
            if (horInput != 0 || vertInput != 0)
            {
                if (diffMagnitude > 0.8f) //run
                {
                    movement.x = horInput * moveSpeed;
                    movement.z = vertInput * moveSpeed;
                }
                else
                {
                    movement.x = horInput * paceSpeed;
                    movement.z = vertInput * paceSpeed;
                }

                movement = Vector3.ClampMagnitude(movement, moveSpeed);
            }

            if (GameSettings.TouchInput)
            {
                TurningTouchscreen();
            }
            else
            {
                Turning();
            }

            //_animator.SetFloat("Speed", movement.sqrMagnitude);

            // raycast down to address steep slopes and dropoff edge
            bool hitGround = false;
            RaycastHit hit;
            if (_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                float check = (_charController.height + _charController.radius) / 1.9f;
                hitGround = hit.distance <= check;  // to be sure check slightly beyond bottom of capsule
            }

            // y movement: possibly jump impulse up, always accel down
            // could _charController.isGrounded instead, but then cannot workaround dropoff edge
            if (hitGround)
            {
                _vertSpeed = minFall;
                //_animator.SetBool("Jumping", false);
            }
            else
            {
                _vertSpeed += gravity * 5 * Time.deltaTime;
                if (_vertSpeed < terminalVelocity)
                {
                    _vertSpeed = terminalVelocity;
                }
            }
            movement.y = _vertSpeed;

            movement *= Time.deltaTime;
            _charController.Move(movement);
        }
    }
    private Vector3 TurningTouchscreen()
    {
        Vector3 lookAt = Vector3.zero;

        float horInput1 = CnInputManager.GetAxis("Mouse X Joystick");
        float vertInput1 = CnInputManager.GetAxis("Mouse Y Joystick");

        float diffMagnitude = new Vector2(horInput1, vertInput1).magnitude;
        // face movement direction
        if (horInput1 != 0 || vertInput1 != 0)
        {
            lookAt.x = horInput1;
            lookAt.z = vertInput1;
            Quaternion direction = Quaternion.LookRotation(lookAt);
            transform.rotation = Quaternion.Lerp(transform.rotation,
                                                 direction, rotSpeed * Time.deltaTime);
        }
        return lookAt;
    }
    private void Turning()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, _camRayLength))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // Set the player's rotation to this new rotation.
            transform.rotation = newRotation;
        }
    }
}
