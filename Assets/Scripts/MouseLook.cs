using System;
using UnityEngine;

[Serializable]
public class MouseLook {
    public float YSensitivity = 2f;
    public float XSensitivity = 2f;

    [Tooltip("Should looking up and down be restricted?")]
    public bool clampVerticalRotaion = true;
    public float MinimumX = -90f;
    public float MaximumX = 90f;
    public bool smooth;
    public float smoothTime = 5f;
    public bool lockCursor = true;

    private Quaternion mCharacterTargetRot;
    private Quaternion mCameraTargetRot;
    private bool mCursorIsLocked = true;

    public void Init(Transform character, Transform camera)
    {
        mCharacterTargetRot = character.localRotation;
        mCameraTargetRot = camera.localRotation;
    }

    public void LookRotation(Transform character, Transform camera)
    {
        float xRot = Input.GetAxis("Mouse Y") * XSensitivity;
        float yRot = Input.GetAxis("Mouse X") * YSensitivity;

        mCharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
        mCameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

        if (clampVerticalRotaion)
            mCameraTargetRot = ClampXRotation(mCameraTargetRot);

        if (smooth)
        {
            character.localRotation = Quaternion.Slerp(character.localRotation, mCharacterTargetRot, smoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp(camera.localRotation, mCameraTargetRot, smoothTime * Time.deltaTime);
        }
        else
        {
            character.localRotation = mCharacterTargetRot;
            camera.localRotation = mCameraTargetRot;
        }

        UpdateCursorLock();
    }

    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
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
