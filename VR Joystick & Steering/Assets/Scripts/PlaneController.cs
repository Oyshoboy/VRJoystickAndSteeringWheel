using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour {
    [Header("Inputs")]
    public GameObject Lever;
    public Animator JosytickAnimator;

    //PLANE PARTS BELOW
    [Header("Plane Parts to rotate")]
    public GameObject leftAileron;
    public GameObject rightAileron;
    public GameObject Rudder;
    public GameObject Elevator;


    // ACCELERATOR BELOW
    [Header("Rotor & Acceleration")]
    public Animator acceleratorAnimator;
    public float AcceleratorPower;
    public rotorController rotorControll;
    public float planeForwardSpeed = 0;

    float X;
    float Y;
    // Use this for initialization
    void Start () {
		
	}

    public float getLeverRotate(float LeverAngle)
    {
        if (LeverAngle > 180)
        {
            LeverAngle -= 360;
        }
        return LeverAngle;
    }

    // Update is called once per frame
    void Update () {

        AcceleratorPower = acceleratorAnimator.GetFloat("Blend X") * 10; // ACCELERATOR CONTROLL
        if (AcceleratorPower <= 0)
        {
            rotorControll.degPerSec = 180;
        }
        else
        {
            rotorControll.degPerSec = (180 * AcceleratorPower) + 180;
        }


        // AILERON MOVEMENTS
        leftAileron.transform.localEulerAngles = new Vector3(-JosytickAnimator.GetFloat("Blend X") * 20, 0, 0);
        rightAileron.transform.localEulerAngles = new Vector3(JosytickAnimator.GetFloat("Blend X") * 20, 0, 0);

        //ELEVATOR MOVEMENT
        Elevator.transform.localEulerAngles = new Vector3(-JosytickAnimator.GetFloat("Blend Z") * 20, 0, 0);

        //RUDDER MOVEMENT
        Rudder.transform.localEulerAngles = new Vector3(0, -getLeverRotate(Lever.transform.localEulerAngles.y), 0);

        // PLANE MOVEMENT AROUND ROTATE
        transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, getLeverRotate(Lever.transform.localEulerAngles.y), 0)/100;
        X = JosytickAnimator.GetFloat("Blend X")*20;
        Y = JosytickAnimator.GetFloat("Blend Z")*20;
        Quaternion rotation = Quaternion.Euler(Y,0,-X);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime*2);
        // PLANE MOVEMENT FORWARD
        transform.position = transform.position + new Vector3(transform.forward.x, transform.forward.y, transform.forward.z) / 30 * planeForwardSpeed;
    }
}
