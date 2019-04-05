using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace polyslash.Player
{
    public class PlayerRaycaster : MonoBehaviour
    {
        [SerializeField] Camera traceStart;
        [SerializeField] private float traceRange = 100.0f;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void Raycast()
        {
            RaycastHit hitInformation;
            if(Physics.Raycast(traceStart.transform.position, traceStart.transform.forward, out hitInformation, traceRange))
            {
                Debug.Log(hitInformation.transform.name);
            }
        }
    }
}