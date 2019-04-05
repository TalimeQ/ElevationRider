using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polyslash.Player { 
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private string horizontalInputString;
        [SerializeField] private string verticalInputString;
        [SerializeField] private string fireString;
        [SerializeField] private string mouseXInputName, mouseYInputName;
        [SerializeField] private float mouseSens;


        [SerializeField][Tooltip("Reference to the script that handles moving")]
        PlayerMovement playerMovementComponentRef;
        [SerializeField][Tooltip("Reference to script moving camera")]
        PlayerLook playerLookComponent;
        [SerializeField][Tooltip("Reference to the script that handles interaction/raycasting")]
        PlayerRaycaster playerRaycastComponentRef;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            playerMovementComponentRef.ProcessPlayerMovement(Input.GetAxis(verticalInputString), Input.GetAxis(horizontalInputString));
            playerLookComponent.CameraRotation(Input.GetAxisRaw(mouseYInputName) ,Input.GetAxisRaw(mouseXInputName), mouseSens);
            if (Input.GetButtonDown(fireString)) playerRaycastComponentRef.Raycast();
        }
    }
}