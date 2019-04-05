using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallButton : MonoBehaviour
{
    [SerializeField]
    Elevator calledElevator;
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
        calledElevator.StartElevator(0);
    }
}
