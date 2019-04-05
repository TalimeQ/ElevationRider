using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polyslash.Player {
    public class PlayerMovement : MonoBehaviour
    {

       
        [SerializeField] private float movementSpeed;



        [SerializeField]
        [Tooltip("Reference to the body we will be moving")]
        CharacterController playerMovementController;


        

        public  void ProcessPlayerMovement(float rawVerticalInput, float rawHorizontalInput)
        {
            float verticalInput = rawVerticalInput * movementSpeed * Time.deltaTime;
            float horizontalInput = rawHorizontalInput* movementSpeed * Time.deltaTime;

            Vector3 forwardMovement = transform.forward * verticalInput;
            Vector3 rightMovement = transform.right * horizontalInput;

            playerMovementController.SimpleMove(forwardMovement + rightMovement);

        }


    }
}