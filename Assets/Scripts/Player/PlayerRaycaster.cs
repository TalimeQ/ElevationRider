using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using polyslash.Winda;

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
            Vector3 endPos = traceStart.transform.forward * traceRange;
            Debug.DrawRay(traceStart.transform.position, endPos, Color.blue, 10.0f);
            if(Physics.Raycast(traceStart.transform.position, traceStart.transform.forward, out hitInformation, traceRange,15,QueryTriggerInteraction.Ignore))
            {
                Debug.DrawLine(hitInformation.transform.position, hitInformation.transform.position,Color.black,1.0f);
                Debug.Log(hitInformation.transform.name);
                CallButton callbutton = hitInformation.transform.gameObject.GetComponent<CallButton>();
                if(callbutton)
                {
                    callbutton.CallElevator();
                }

            }
        }
    }
}