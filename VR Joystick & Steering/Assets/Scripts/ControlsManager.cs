using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour {
    //Steering Wheel vars
    GameObject empyHandObject;
    GameObject SteeringWheel;
    SteeringWheelController WheelController;
    bool SteeringWheelStick;


    [Header("Steam Controllers Inputs (auto)")]
    public SteamVR_TrackedController VRJoystickTracker;

    //Controle Lever vars
    GameObject Lever;
    GameObject ControlLeverTop; // LEVERS TOP POINT
    Animator ControlleverAnimator; // ANIMATOR
    Vector3 ControlleverTopRelative;// RELATIVE POINT
    bool ConrolStickLever = false;
    float ControlLeverLastX;
    float ControlLeverLastY; // LAST POSITION FOR LERPING
    float ControlleverPosY;
    float ControlleverPosX; // CURRENT POS



    //Accelerate Lever vars
    GameObject AccelerateLever;
    GameObject AcceleratelLeverTop;
    Animator AcceleratorLeverAnimator;
    Vector3 AcceleratorleverTopRelative;
    bool AcceleratelStickLever = false;
    float AccelerateLeverLastX;
    float AccelerateLeverLastY; // LAST POSITION FOR LERPING
    float AccelerateleverPosY;
    float AccelerateleverPosX; // CURRENT POS



    [Header("Hands To enable/disable")]
    public string RealHandName;
    GameObject RealHand;
    GameObject HandOnControl;
    GameObject HandOnAccel;
    GameObject HandOnSteering;


    float RotateWhenPicked;


    // Use this for initialization
    void Start () {


        RealHand = GameObject.Find(RealHandName);
        VRJoystickTracker = gameObject.GetComponent<SteamVR_TrackedController>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name== "SteeringWheelCore" && VRJoystickTracker.triggerPressed && !ConrolStickLever && !AcceleratelStickLever && !SteeringWheelStick)
        {
            SteeringWheel = other.gameObject;
            SteeringWheelStick = true;
            HandOnSteering = other.transform.Find(gameObject.name).gameObject;
            WheelController = SteeringWheel.GetComponent<SteeringWheelController>();
        }

     if (other.name == "LeverControl" && VRJoystickTracker.triggerPressed && !ConrolStickLever && !AcceleratelStickLever && !SteeringWheelStick) // STICK CONTROL LEVER
        {
            Lever = other.transform.parent.gameObject;
            ControlLeverTop = Lever.transform.parent.parent.transform.Find("Relative Center Point").gameObject;
            ControlleverAnimator = Lever.transform.parent.GetComponent<Animator>();
            ControlLeverTop.transform.position = transform.position;
            RotateWhenPicked = transform.localEulerAngles.y;
            ConrolStickLever = true;
            HandOnControl = other.transform.parent.Find(gameObject.name).gameObject;
        }

        if (other.name == "LeverAccelerate" && VRJoystickTracker.triggerPressed && !ConrolStickLever && !AcceleratelStickLever && !SteeringWheelStick) // STICK ACCELERATE LEVER
        {
            AccelerateLever = other.gameObject;
            AcceleratelLeverTop = AccelerateLever.transform.parent.parent.parent.transform.Find("Relative Center Point").gameObject;
            AcceleratorLeverAnimator = AccelerateLever.transform.parent.parent.GetComponent<Animator>();
            AcceleratelLeverTop.transform.position = transform.position;
            AcceleratelStickLever = true;
            HandOnAccel = other.transform.parent.Find(gameObject.name).gameObject;

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
            HandOnControl = null;

        }

        if (AcceleratelStickLever)
        {
            AcceleratelStickLever = false; // ACCELERATE LEVER UNSTICK
            RealHand.SetActive(true);
            HandOnAccel.SetActive(false);
            HandOnAccel = null;
        }

        if (SteeringWheelStick)
        {
            WheelController.OnUnStick();
            SteeringWheelStick = false; // STEERING WHEEL UNSTICK
            //empyHandObject.transform.position = transform.position;
            WheelController.Hand = null;
            RealHand.SetActive(true);
            HandOnSteering.SetActive(false);
            HandOnSteering = null;
            SteeringWheel = null;
            WheelController = null;
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        //Debug.Log(getLeverRotate(Lever.transform.localEulerAngles.y));

        if (SteeringWheelStick) // STEERING WHEEL CONTROLLER
        {
            if (!WheelController.Hand)
            {
                WheelController.Hand = GameObject.Find(gameObject.name); // CHECK IF ALREADY HAND GRABBED
            }

            RealHand.SetActive(false);
            HandOnSteering.SetActive(true);
            WheelController.OnStick(VRJoystickTracker, HandOnSteering);
        }

        if (!VRJoystickTracker.triggerPressed) // UNSTICK EVERYTHING
        {
            unstickEveryThing();
        }

        if (ConrolStickLever) // CONTROL LEVER CONTROLLER 
        {
            RealHand.SetActive(false);
            HandOnControl.SetActive(true);

            Lever.transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y - RotateWhenPicked, 0);
            ControlleverTopRelative = ControlLeverTop.transform.InverseTransformPoint(transform.position);
            ControlleverAnimator.SetFloat("Blend Z", ControlleverTopRelative.z / 2);
            ControlleverAnimator.SetFloat("Blend X", ControlleverTopRelative.x / 2);
        }

        if (!ConrolStickLever && ControlleverAnimator)
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

        if (!AcceleratelStickLever && AcceleratorLeverAnimator)
        {
            // UNSTICK ACELERATE LEVER
            AccelerateLeverLastX = AcceleratorLeverAnimator.GetFloat("Blend X");
            AccelerateleverPosX = Mathf.Lerp(AccelerateLeverLastX, 0, Time.time / 150);
            AcceleratorLeverAnimator.SetFloat("Blend X", AccelerateleverPosX);

        }


    }

}
