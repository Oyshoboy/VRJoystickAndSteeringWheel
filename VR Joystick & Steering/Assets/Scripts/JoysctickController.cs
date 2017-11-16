using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoysctickController : MonoBehaviour {
    [Header("Steering Wheel")]
    public GameObject empyHandObject;
    public GameObject SteeringWheel;
    public SteeringWheelController WheelController;
    bool SteeringWheelStick;


    [Header("Steam Controllers Inputs")]
    public SteamVR_TrackedController VRJoystickTracker;

    [Header("Control Lever")]
    public GameObject Lever;
    public GameObject ControlLeverTop; // LEVERS TOP POINT
    public Animator ControlleverAnimator; // ANIMATOR
    public Vector3 ControlleverTopRelative;// RELATIVE POINT
    bool ConrolStickLever = false;
    float ControlLeverLastX;
    float ControlLeverLastY; // LAST POSITION FOR LERPING
    float ControlleverPosY;
    float ControlleverPosX; // CURRENT POS



    [Header("Accelerator Lever")]
    public GameObject AcceleratelLeverTop;
    public Animator AcceleratorLeverAnimator;
    public Vector3 AcceleratorleverTopRelative;
    bool AcceleratelStickLever = false;
    float AccelerateLeverLastX;
    float AccelerateLeverLastY; // LAST POSITION FOR LERPING
    float AccelerateleverPosY;
    float AccelerateleverPosX; // CURRENT POS



    [Header("Hands To enable/disable")]
    public GameObject RealHand;
    public GameObject HandOnControl;
    public GameObject HandOnAccel;
    public GameObject HandOnSteering;


    float RotateWhenPicked;


    // Use this for initialization
    void Start () {
        WheelController = SteeringWheel.GetComponent<SteeringWheelController>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name== "SteeringWheelCollider" && VRJoystickTracker.triggerPressed && !ConrolStickLever && !AcceleratelStickLever && !SteeringWheelStick)
        {
            SteeringWheelStick = true;
        }

     if (other.name == "LeverControl" && VRJoystickTracker.triggerPressed && !ConrolStickLever && !AcceleratelStickLever && !SteeringWheelStick) // STICK CONTROL LEVER
        {
            ControlLeverTop.transform.position = transform.position;
            RotateWhenPicked = transform.parent.localEulerAngles.y;
            ConrolStickLever = true;

        }

        if (other.name == "LeverAccelerate" && VRJoystickTracker.triggerPressed && !ConrolStickLever && !AcceleratelStickLever && !SteeringWheelStick) // STICK ACCELERATE LEVER
        {
            AcceleratelLeverTop.transform.position = transform.position;
            AcceleratelStickLever = true;

        }
    }

    void unstickEveryThing()
    {
        if (ConrolStickLever)
        {
            ConrolStickLever = false; // CONTROL LEVER UNSTICK
            RealHand.SetActive(true);
            HandOnControl.SetActive(false);
            Lever.transform.localEulerAngles = new Vector3(0, 0, 0);

        }

        if (AcceleratelStickLever)
        {
            AcceleratelStickLever = false; // ACCELERATE LEVER UNSTICK
            RealHand.SetActive(true);
            HandOnAccel.SetActive(false);
        }

        if (SteeringWheelStick)
        {
            WheelController.OnUnStick();
            SteeringWheelStick = false; // STEERING WHEEL UNSTICK
            empyHandObject.transform.position = transform.position;
            WheelController.Hand = empyHandObject;
            RealHand.SetActive(true);
            HandOnSteering.SetActive(false);
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        //Debug.Log(getLeverRotate(Lever.transform.localEulerAngles.y));

        if (SteeringWheelStick) // STEERING WHEEL CONTROLLER
        {
            WheelController.Hand = GameObject.Find(transform.parent.name);
            RealHand.SetActive(false);
            HandOnSteering.SetActive(true);
            WheelController.OnStick();
        }

        if (!VRJoystickTracker.triggerPressed) // UNSTICK EVERYTHING
        {
            unstickEveryThing();
        }

        if (ConrolStickLever) // CONTROL LEVER CONTROLLER 
        {
            RealHand.SetActive(false);
            HandOnControl.SetActive(true);

            Lever.transform.localEulerAngles = new Vector3(0, transform.parent.localEulerAngles.y - RotateWhenPicked, 0);
            ControlleverTopRelative = ControlLeverTop.transform.InverseTransformPoint(transform.position);
            ControlleverAnimator.SetFloat("Blend Z", ControlleverTopRelative.z / 2);
            ControlleverAnimator.SetFloat("Blend X", ControlleverTopRelative.x / 2);
        }

        if (!ConrolStickLever)
        {
            // UNSTICK CNROL LEVER
            ControlLeverLastX = ControlleverAnimator.GetFloat("Blend X");
            ControlleverPosX = Mathf.Lerp(ControlLeverLastX, 0, Time.time / 150);
            ControlleverAnimator.SetFloat("Blend X", ControlleverPosX);

            ControlLeverLastY = ControlleverAnimator.GetFloat("Blend Z");
            ControlleverPosY = Mathf.Lerp(ControlLeverLastY, 0, Time.time/150);
            ControlleverAnimator.SetFloat("Blend Z", ControlleverPosY);

        }



        if (AcceleratelStickLever) // ACCELERATE LEVER CONTROLLER 
        {
            RealHand.SetActive(false);
            HandOnAccel.SetActive(true);

            AcceleratorleverTopRelative = AcceleratelLeverTop.transform.InverseTransformPoint(transform.position);
            AcceleratorLeverAnimator.SetFloat("Blend X", -(AcceleratorleverTopRelative.z / 40));

        }

        if (!AcceleratelStickLever)
        {
            // UNSTICK ACELERATE LEVER
            AccelerateLeverLastX = AcceleratorLeverAnimator.GetFloat("Blend X");
            AccelerateleverPosX = Mathf.Lerp(AccelerateLeverLastX, 0, Time.time / 150);
            AcceleratorLeverAnimator.SetFloat("Blend X", AccelerateleverPosX);

        }


    }

}
