using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polyslash.Winda
{
    public enum ElevatorState
    {
        E_Unknown,
        E_Idle,
        E_Opened,
        E_Opening,
        E_Closing,
        E_Moving,
        E_DoorsInterrupted
    }

    public class Elevator : MonoBehaviour
    {

        ElevatorState currentElevState = ElevatorState.E_Idle;
        Animator elevatorAnimator;
        private Vector3 startPoint;
        // [SerializeField]
        private Vector3 endPoint;

        // Chyba nie potrafie w traceing, na poczatku trace normalnie zwracal przyciski na konsoli i winda jechala, potem zaczal zwracac caly prefab windy :/
        [SerializeField]
        GameObject konsola;
        [SerializeField]
        Transform miejsceNaKonsole;
        //Maybe will be used later
        [SerializeField]
        BoxCollider noGravZone;
        [SerializeField]
        float speed = 2;
        [SerializeField]
        float stateTime = 5.0f;
        [SerializeField]
        float averageTravelDistance = 5.5f;

        GameObject carriedPlayer;
        float elevatorStartTime = 0.0f;
        float journeyLenght;
        bool isIdle = true;

        // Start is called before the first frame update
        void Start()
        {
            
            ConsoleInit();
            elevatorAnimator = GetComponent<Animator>();
   

        }

        private void ConsoleInit()
        {
            konsola = Instantiate(konsola);
            var Buttony = konsola.GetComponentsInChildren<CallButton>();
            konsola.transform.position = miejsceNaKonsole.position;
            foreach (CallButton button in Buttony)
            {
                button.CalledElevator = this;
            }
        }

        // Update is called once per frame
        void Update()
        {
            ProcessCurrentState();
            
        }

        private void MoveElevator()
        {
            float distCovered = (Time.time - elevatorStartTime) * speed;
            konsola.transform.position = miejsceNaKonsole.position;
            float fracJourney = distCovered / journeyLenght;
            transform.position = Vector3.Lerp(startPoint, endPoint, fracJourney);
            if (fracJourney >= 1.0f) SwitchState(ElevatorState.E_Opening);
        }

        private void ProcessCurrentState()
        {
            switch (currentElevState)
            {

                case ElevatorState.E_Moving:
                    MoveElevator();
                    break;
                case ElevatorState.E_Opening:
                    if (this.elevatorAnimator.GetCurrentAnimatorStateInfo(0).IsName("WindaDrzwi"))
                    {
                        elevatorAnimator.SetBool("isOpening", false);
                        StartCoroutine(timedStateSwitcher(ElevatorState.E_Closing));
                    }
                    break;
                case ElevatorState.E_Closing:
                     if (this.elevatorAnimator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
                    {
                        elevatorAnimator.SetFloat("closeValue", 0.1f);
                    }
                    else if (this.elevatorAnimator.GetCurrentAnimatorStateInfo(0).length <= elevatorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime )
                    {

                        elevatorAnimator.SetFloat("closeValue", 0.0f);
                        SwitchState(ElevatorState.E_Idle);
                    }
                   
                    break;
                        
                default:
              
                    break;
            }
            print("Unrecognized state" + currentElevState);
        }

        public void StartElevator(int floorLevel)
        {
            // It should have queue or smth to process multiple inputs like normal elevs do, but well thats close enough 
            if (currentElevState == ElevatorState.E_Moving)
            {
                print("Elevator is already moving" + currentElevState);
                return;
            }

            int currentFloorLevel = (int)(transform.position.y / averageTravelDistance);
            if (floorLevel == currentFloorLevel)
            {
                SwitchState(ElevatorState.E_Opening);
                return;
            }

            if (floorLevel > currentFloorLevel)
            {
                print("Called Up");
                startPoint = transform.position;
                endPoint = new Vector3(transform.position.x, transform.position.y + averageTravelDistance * Mathf.Abs(floorLevel - currentFloorLevel), transform.position.z);
                journeyLenght = Vector3.Distance(startPoint, endPoint);
                elevatorStartTime = Time.time;
            }
            else if (floorLevel < currentFloorLevel)
            {
                print("Called Down");
                startPoint = transform.position;
                endPoint = new Vector3(transform.position.x, transform.position.y - averageTravelDistance * Mathf.Abs(floorLevel - currentFloorLevel), transform.position.z);
                journeyLenght = Vector3.Distance(startPoint, endPoint);
                elevatorStartTime = Time.time;
            }
            print("Ride Parameters: startpoint:" + startPoint + " endpoint:" + endPoint + elevatorStartTime);
            SwitchState(ElevatorState.E_Moving);
            
            
        }

        IEnumerator timedStateSwitcher(ElevatorState newElevatorState)
        {
            yield return new WaitForSeconds(stateTime);
            SwitchState(newElevatorState);
        }

        public void SwitchState(ElevatorState newElevatorState)
        {
            currentElevState = newElevatorState;
            switch(currentElevState)
            {
                case ElevatorState.E_Opening:
                    elevatorAnimator.SetBool("isOpening", true);
                    break;
                case ElevatorState.E_Closing:
                    elevatorAnimator.SetFloat("closeValue", 0.1f);

                    break;
                default:
                    break;
            }
        }

        // Obsluga fotokomorki
        
        private void OnTriggerStay(Collider other)
        {
            print("triggered!");
            if (currentElevState == ElevatorState.E_Closing)
            {

                elevatorAnimator.SetFloat("closeValue", -0.1f);
            }
           
        }

        
    }
}