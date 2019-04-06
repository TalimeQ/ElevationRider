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
    }

    public class Elevator : MonoBehaviour
    {



        /* Chyba nie potrafie w traceing, na poczatku trace normalnie zwracal przyciski na konsoli i winda jechala, potem zaczal zwracac caly prefab windy :/
         Efektem jest ten bandaidowe rozwiązanie.
             */
        [SerializeField]
        GameObject konsola;
        [SerializeField]
        Transform miejsceNaKonsole;
        //Maybe be used later
        [SerializeField]
        BoxCollider noGravZone;
        [SerializeField]
        AudioClip arrivedClip;
        [SerializeField]
        AudioClip loopingTheme;
        [SerializeField]
        float speed = 2;
        [SerializeField]
        float openedTime = 5.0f;
        [SerializeField]
        float averageTravelDistance = 5.5f;

        GameObject carriedPlayer;
        float elevatorStartTime = 0.0f;
        float journeyLenght;
        ElevatorState currentElevState = ElevatorState.E_Idle;
        Animator elevatorAnimator;
        private Vector3 startPoint;
        // [SerializeField]
        private Vector3 endPoint;
        AudioSource elevatorAudioSource;

        // Start is called before the first frame update
        void Start()
        {
            
            ConsoleInit();
            elevatorAnimator = GetComponent<Animator>();
            elevatorAudioSource = GetComponent<AudioSource>();

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
            if (fracJourney >= 1.0f)
            {
                PlaySound(arrivedClip, false);
                SwitchState(ElevatorState.E_Opening);
            }
        }

        private void ProcessCurrentState()
        {
            switch (currentElevState)
            {

                case ElevatorState.E_Moving:
                    MoveElevator();
                    break;
                case ElevatorState.E_Opening:
                    ProcessOpeningLogic();
                    break;
                case ElevatorState.E_Closing:
                    ProcessClosingLogic();
                    break;
                default:
                    break;
            }
        }

        private void ProcessClosingLogic()
        {
            if (this.elevatorAnimator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
            {
                elevatorAnimator.SetFloat("closeValue", 0.1f);
            }
            else if (this.elevatorAnimator.GetCurrentAnimatorStateInfo(0).length <= elevatorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {

                elevatorAnimator.SetFloat("closeValue", 0.0f);
                SwitchState(ElevatorState.E_Idle);
            }
        }

        private void ProcessOpeningLogic()
        {
            if (this.elevatorAnimator.GetCurrentAnimatorStateInfo(0).IsName("WindaDrzwi"))
            {
                elevatorAnimator.SetBool("isOpening", false);
                StartCoroutine(timedStateSwitcher(ElevatorState.E_Closing,openedTime));
            }
        }
       

        public void StartElevator(int floorLevel)
        {
            // It should have queue or smth to process multiple inputs like normal elevs do, but well thats close enough 
            if (currentElevState != ElevatorState.E_Idle)
            {
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
             
            }
            else if (floorLevel < currentFloorLevel)
            {
                print("Called Down");
                startPoint = transform.position;
                endPoint = new Vector3(transform.position.x, transform.position.y - averageTravelDistance * Mathf.Abs(floorLevel - currentFloorLevel), transform.position.z);
                journeyLenght = Vector3.Distance(startPoint, endPoint);
               
            }
            print("Ride Parameters: startpoint:" + startPoint + " endpoint:" + endPoint + elevatorStartTime);
       
            PlaySound(arrivedClip, false);
            StartCoroutine(timedStateSwitcher(ElevatorState.E_Moving, 1.0f));
        }

        IEnumerator timedStateSwitcher(ElevatorState newElevatorState, float stateTime)
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
                case ElevatorState.E_Moving:
                    elevatorStartTime = Time.time;
                    PlaySound(loopingTheme, true);
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

        private void PlaySound(AudioClip audio, bool looping)
        {
            if (elevatorAudioSource.isPlaying) elevatorAudioSource.Stop();
            if(looping)
            {
                elevatorAudioSource.clip = audio;
                elevatorAudioSource.Play();
            }
            else
            {
                elevatorAudioSource.PlayOneShot(audio);
            }
           
        }
        
    }
}