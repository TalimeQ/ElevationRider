using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polyslash.Player { 
    public class PlayerLook : MonoBehaviour
    {


        [SerializeField] private Transform playerTransform;


        private float xAxisClamp = 0.0f;

        // Start is called before the first frame update
        void Awake()
        {
            LockCursor();
        }

        // Locks the cursor
        void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void CameraRotation(float rawMouseY, float rawMouseX,float mouseSens)
        {
            float mouseX = rawMouseX * mouseSens;
            float mouseY = rawMouseY* mouseSens;

            xAxisClamp += mouseY;

            if(xAxisClamp > 90.0f)
            {
                xAxisClamp = 90.0f;
                mouseY = 0.0f;
                ClampXAxisRotationToValue(270.0f);
            }
            else if(xAxisClamp < - 90.0f)
            {
                xAxisClamp = -90.0f;
                mouseY = 0.0f;
                ClampXAxisRotationToValue(90.0f);
            }

            transform.Rotate(Vector3.left * mouseY);
            playerTransform.Rotate(Vector3.up * mouseX);
        }

        private void ClampXAxisRotationToValue(float value)
        {
            Vector3 eulerRotation = transform.eulerAngles;
            eulerRotation.x = value;
            transform.eulerAngles = eulerRotation;
        }
    }
}