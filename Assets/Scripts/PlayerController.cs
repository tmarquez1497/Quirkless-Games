using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float walkSpeed = 5f;              // The walking speed of the player
    [SerializeField] private float runSpeed = 10f;              // The running speed of the player
    [SerializeField] private float jumpSpeed = 10f;             // The upward force applied when jumping
    [SerializeField] private float stickToGroundForce = 10f;    // A downward force applied to keep the player on the ground
    [SerializeField] private float gravityMultiplier = 2f;      // The scale the force of physics when in the air
    [SerializeField] private MouseLook mouseLook;               // Refer to MouseLook script

    private Camera mCamera;                                     // The game's camera (MUST have the "Main Camera" tag)
    private bool jump;                                          // Was the jump button (spacebar) just pressed
    //private float yRotation;                                  
    private Vector2 mInput;                                     // The input vector that reads keyboard input
    private Vector3 moveDir = Vector3.zero;                     // Target position to move to (updated every frame)
    private CharacterController characterController;            // The Character Controller component
    private CollisionFlags collisionFlags;                      // List of objects the player collides with while moving
    private bool previouslyGrounded;                            // Was the player on the ground during the last frame
    private bool jumping;                                       // Is the player in the air
    private bool isWalking;                                     // Is the shift key being held or not

	void Start () {
        // Grab the character controller component, find the camera, find the GameManager and startup the mouse script.
        characterController = GetComponent<CharacterController>();
        mCamera = Camera.main;
        jumping = false;
        mouseLook.Init(transform, mCamera.transform);
    }
	
	void Update () {
        // If the GameManager exists and the game isn't paused . . .
        if (GameManager.instance == null || !GameManager.instance.isPaused)
        {
            // Rotate the camera
            RotateView();

            //If the player didn't already jump and is not in the air then get the jump input.
            if (!jump && !jumping)
                jump = Input.GetButtonDown("Jump");

            // If the player was not on the ground last frame and is on the ground now . . .
            if (!previouslyGrounded && characterController.isGrounded)
            {
                // . . . Set the y axis movement to 0 and jumping to false
                moveDir.y = 0f;
                jumping = false;
            }

            // If the player is not on the ground and is not in the air but was grounded last frame, then clear the upward movement.
            if (!characterController.isGrounded && !jumping && previouslyGrounded)
                moveDir.y = 0f;

            // Check to see if the player is on the ground or not.
            previouslyGrounded = characterController.isGrounded;
        }
	}

    private void FixedUpdate()
    {
        // If the GameManager exists and the game isn't paused . . .
        if (GameManager.instance == null || !GameManager.instance.isPaused)
        {
            // Grab controller input (if any)
            float speed;
            GetInput(out speed);

            // Set the next movement position based on the input
            Vector3 desiredMove = transform.forward * mInput.y + transform.right * mInput.x;

            // Check to see if the player is above the ground, then set the movement position to the right axis.
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo, characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            // Then scale the movement vector by the the player's speed
            moveDir.x = desiredMove.x * speed;
            moveDir.z = desiredMove.z * speed;

            // If the player is on the ground . . .
            if (characterController.isGrounded)
            {
                // . . . apply the ground force.
                moveDir.y = -stickToGroundForce;

                // If jump was pressed while on the ground . . .
                if (jump)
                {
                    // . . . add the upward force to the vector, reset the jump variable, and set jumping to true.
                    moveDir.y = jumpSpeed;
                    jump = false;
                    jumping = true;
                    // [Note] Add setup to play jump sound if needed
                }
            }
            // If the player is in the air, apply the downward force of gravity.
            else
            {
                moveDir += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
            }

            // Move the player and save any collisions made.
            collisionFlags = characterController.Move(moveDir * Time.fixedDeltaTime);

            // Check to see if the mouse pointer should be hidden.
            mouseLook.UpdateCursorLock();
        }
    }

    public void SetCursor(bool val)
    {
        mouseLook.SetCursorLock(val);
    }

    private void GetInput(out float speed)
    {
        // Grab the horizontal and vertical movement values
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Is the run button being held down for running,
        isWalking = !Input.GetButton("Run");

        // if so the player is walking; if not then it's running
        speed = isWalking ? walkSpeed : runSpeed;
        
        // Save the combined direction of the controller input
        mInput = new Vector2(horizontal, vertical);

        // If the length of the vector is less than 1 Unity unit, shrink it to 1
        if (mInput.sqrMagnitude > 1)
            mInput.Normalize();
    }

    private void RotateView()
    {
        // Check the mouse location to update the camera.
        mouseLook.LookRotation(transform, mCamera.transform);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // If the player touched the ground or if the thing they hit does not have a RigidBody, do nothing.
        Rigidbody body = hit.collider.attachedRigidbody;
        if (collisionFlags == CollisionFlags.Below)
            return;
        if (body == null || body.isKinematic)
            return;
        if (body.tag.Equals("PickUp"))
        {
            body.gameObject.SetActive(false);
            GameManager.instance.YouWin();
        }

        // Otherwise, add a force to whatever was hit.
        body.AddForceAtPosition(characterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }
}
