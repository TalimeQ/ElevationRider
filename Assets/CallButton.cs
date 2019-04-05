using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polyslash.Winda
{
    public class CallButton : MonoBehaviour
    {
        [SerializeField]
        Elevator calledElevator;
        [SerializeField]
        int LevelNumber = 0;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public void CallElevator()
        {
            calledElevator.StartElevator(LevelNumber);
        }
    }
}