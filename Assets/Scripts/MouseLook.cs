using System;
using UnityEngine;

[Serializable]
public class MouseLook {
    public float YSensitivity = 2f;             // Sensitivity scale of the mouse in the vertical direction
    public float XSensitivity = 2f;             // Sensitivity scale of the mouse in the horizontal direction

    public bool clampVerticalRotaion = true;    // Should the camera act as if it has a neck when looking up and down.
    public float MinimumX = -90f;               // Lowest angle the player can look down (if restricted)
    public float MaximumX = 90f;                // Highest angle the player can look up (if restricted)
    public bool smooth;                         // Should smoothing be applied to camera movement (use if movement is jittery)
    public float smoothTime = 5f;               // Length of the smooth rotation curve
    public bool lockCursor = true;              // Should the mouse pointer be hidden or not

    private Quaternion mCharacterTargetRot;     // Stored rotation of the player
    private Quaternion mCameraTargetRot;        // Stored rotation of the camera
    private bool mCursorIsLocked = true;        // Is the mouse pointer currently hidden

    public void Init(Transform character, Transform camera)
    {
        // Grab and store the rotation of the player and camera
        mCharacterTargetRot = character.localRotation;
        mCameraTargetRot = camera.localRotation;
    }

    public void LookRotation(Transform character, Transform camera)
    {
        // Grab and store the mouse postion and scale by sensitivity
        float xRot = Input.GetAxis("Mouse Y") * XSensitivity;
        float yRot = Input.GetAxis("Mouse X") * YSensitivity;

        // Rotate the player and the camera by an angle equal to mouse movement
        mCharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
        mCameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

        // If the camera has a neck, restrict its angle to the limits set earlier.
        if (clampVerticalRotaion)
            mCameraTargetRot = ClampXRotation(mCameraTargetRot);

        // If smoothing is applied, call Unity to rotate camera and player by a curve instead of linearly
        if (smooth)
        {
            character.localRotation = Quaternion.Slerp(character.localRotation, mCharacterTargetRot, smoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp(camera.localRotation, mCameraTargetRot, smoothTime * Time.deltaTime);
        }
        // Otherwise, set the rotations directly
        else
        {
            character.localRotation = mCharacterTargetRot;
            camera.localRotation = mCameraTargetRot;
        }

        // Check to see if the mouse pointer should be hidden
        UpdateCursorLock();
    }

    public void SetCursorLock(bool value)
    {
        /**
         * This changes the mouse to visible if the player changes while the game is running 
         */
        lockCursor = value;
        if (!lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        // If it's set to lock, call the function to handle the mouse
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
            /**
            * If L is pressed, show the mouse pointer
            * If the player clicks the game, hide the mouse pointer
            */
            if (Input.GetKeyUp(KeyCode.L))
            {
                mCursorIsLocked = false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                mCursorIsLocked = true;
            }
            if (mCursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!mCursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
    }

    Quaternion ClampXRotation(Quaternion q)
    {
        // This function restricts the up and down rotation.
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
