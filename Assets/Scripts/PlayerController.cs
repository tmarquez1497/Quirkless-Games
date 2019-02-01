using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private bool isWalking;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float stickToGroundForce = 10f;
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private MouseLook mouseLook;

    private Camera mCamera;
    private bool jump;
    private float yRotation;
    private Vector2 mInput;
    private Vector3 moveDir = Vector3.zero;
    private CharacterController characterController;
    private CollisionFlags collisionFlags;
    private bool previouslyGrounded;
    private bool jumping;

	// Use this for initialization
	void Start () {
        characterController = GetComponent<CharacterController>();
        mCamera = Camera.main;
        jumping = false;
        mouseLook.Init(transform, mCamera.transform);
	}
	
	// Update is called once per frame
	void Update () {
        RotateView();

        if (!jump && !jumping)
            jump = Input.GetButtonDown("Jump");

        if(!previouslyGrounded && characterController.isGrounded)
        {
            moveDir.y = 0f;
            jumping = false;
        }
        if (!characterController.isGrounded && !jumping && previouslyGrounded)
            moveDir.y = 0f;

        previouslyGrounded = characterController.isGrounded;
	}

    private void FixedUpdate()
    {
        float speed;
        GetInput(out speed);

        Vector3 desiredMove = transform.forward * mInput.y + transform.right * mInput.x;

        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo, characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        moveDir.x = desiredMove.x * speed;
        moveDir.z = desiredMove.z * speed;

        if (characterController.isGrounded)
        {
            moveDir.y = -stickToGroundForce;

            if (jump)
            {
                moveDir.y = jumpSpeed;
                //Add setup to play jump sound if needed
                jump = false;
                jumping = true;
            }
        }
        else
        {
            moveDir += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
        }

        collisionFlags = characterController.Move(moveDir * Time.fixedDeltaTime);

        mouseLook.UpdateCursorLock();
    }

    private void GetInput(out float speed)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        isWalking = !Input.GetKey(KeyCode.LeftShift);

        speed = isWalking ? walkSpeed : runSpeed;
        mInput = new Vector2(horizontal, vertical);

        if (mInput.sqrMagnitude > 1)
            mInput.Normalize();
    }

    private void RotateView()
    {
        mouseLook.LookRotation(transform, mCamera.transform);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (collisionFlags == CollisionFlags.Below)
            return;
        if (body == null || body.isKinematic)
            return;

        body.AddForceAtPosition(characterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }
}
