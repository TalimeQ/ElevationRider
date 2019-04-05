using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polyslash.Winda
{
    public class Elevator : MonoBehaviour
    {
        Animator elevatorAnimator;
        private Vector3 startPoint;
        // [SerializeField]
        private Vector3 endPoint;

        // Chyba nie potrafie w traceing, na poczatku trace normalnie zwracal przyciski na konsoli i winda jechala, potem zaczal zwracac caly prefab windy :/
        [SerializeField]
        GameObject konsola;
        [SerializeField]
        Transform miejsceNaKonsole;

        float elevatorStartTime = 0.0f;
        [SerializeField]
        float speed = 2;

        [SerializeField]
        float averageTravelDistance = 5.5f;
        float journeyLenght;

        bool isIdle = true;

        // Start is called before the first frame update
        void Start()
        {
            konsola = Instantiate(konsola);
            var Buttony = konsola.GetComponentsInChildren<CallButton>();
            foreach(CallButton button in Buttony)
            {
                print(button);
                button.CalledElevator = this;
            }
            elevatorAnimator = GetComponent<Animator>();
            

            startPoint = transform.position;
            endPoint = new Vector3(transform.position.x, transform.position.y + averageTravelDistance, transform.position.z);
            journeyLenght = Vector3.Distance(startPoint, endPoint);
            elevatorStartTime = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            float distCovered = (Time.time - elevatorStartTime) * speed;
            konsola.transform.position = miejsceNaKonsole.position;
            float fracJourney = distCovered / journeyLenght;
            transform.position = Vector3.Lerp(startPoint, endPoint, fracJourney);
            if (fracJourney >= 1.0f) isIdle = true;
        }


        public void StartElevator(int floorLevel)
        {
            print("Started elevator!!");
            if (!isIdle)
            {
                print("Dammit" + isIdle);
                return;
            }

            int currentFloorLevel = (int)(transform.position.y / averageTravelDistance);
            print(currentFloorLevel + " called to: " + floorLevel);
            if (floorLevel == currentFloorLevel)
            {
                return;
            }


            isIdle = false;

            if (floorLevel > currentFloorLevel)
            {
                print("Called Up");
                startPoint = transform.position;
                endPoint = new Vector3(transform.position.x, transform.position.y + averageTravelDistance * Mathf.Abs(floorLevel - currentFloorLevel), transform.position.z);
                elevatorStartTime = Time.time;
            }
            else if (floorLevel < currentFloorLevel)
            {
                print("Called Down");
                startPoint = transform.position;
                endPoint = new Vector3(transform.position.x, transform.position.y - averageTravelDistance * Mathf.Abs(floorLevel - currentFloorLevel), transform.position.z);
                elevatorStartTime = Time.time;
            }



        }

        public void OpenDoor(int floorLevel)
        {
            int currentFloorLevel = (int)(transform.position.y / averageTravelDistance);
            if (floorLevel == currentFloorLevel)
            elevatorAnimator.Play("WindaDrzwi");
        }
    }
}