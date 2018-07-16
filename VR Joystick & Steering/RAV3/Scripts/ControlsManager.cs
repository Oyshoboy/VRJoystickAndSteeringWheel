using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour {
    [HideInInspector]
    public GameObject SteeringWheel;
    SteeringWheelController WheelController;
    bool SteeringWheelStick;


    [Header("Steam Controllers Inputs (auto)")]
    [HideInInspector]
    public SteamVR_TrackedController VRJoystickTracker;

    //Controle Lever vars
    [HideInInspector]
    public GameObject Lever;
    [HideInInspector]
    public GameObject ControlLeverTop; // LEVERS TOP POINT
    Animator ControlleverAnimator; // ANIMATOR
    Vector3 ControlleverTopRelative;// RELATIVE POINT
    bool ConrolStickLever = false;
    float ControlLeverLastX;
    float ControlLeverLastZ; // LAST POSITION FOR LERPING
    float ControlleverPosY;
    float ControlleverPosX; // CURRENT POS



    //Accelerate Lever vars
    [HideInInspector]
    public GameObject AccelerateLever;
    [HideInInspector]
    public GameObject AcceleratelLeverTop;
    Animator AcceleratorLeverAnimator;
    Vector3 AcceleratorleverTopRelative;
    bool AcceleratelStickLever = false;
    float AccelerateLeverLastX;
    float AccelerateLeverLastBlend; // LAST POSITION FOR LERPING
    float AccelerateleverPosX; // CURRENT POS


    float RotateWhenPicked;
    [HideInInspector]
    [Header("Joystick Output")]
    public JoystickOutput joystickOutput;

    [HideInInspector]
    [Header("Accelerator Output")]
    public AcceleratorOutput acceleratorOutPut;


    // Use this for initialization
    void Start () {
        VRJoystickTracker = gameObject.GetComponent<SteamVR_TrackedController>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "SteeringWheelCore" && VRJoystickTracker.triggerPressed && !ConrolStickLever && !AcceleratelStickLever && !SteeringWheelStick)
        {
            SteeringWheel = other.gameObject;
            SteeringWheelStick = true;
            WheelController = SteeringWheel.GetComponent<SteeringWheelController>();
        }
        else if (other.name == "LeverControl" && VRJoystickTracker.triggerPressed && !ConrolStickLever && !AcceleratelStickLever && !SteeringWheelStick) // STICK CONTROL LEVER
        {
            OnControlBegin(other);
        }
        else if (other.name == "LeverAccelerate" && VRJoystickTracker.triggerPressed && !ConrolStickLever && !AcceleratelStickLever && !SteeringWheelStick) // STICK ACCELERATE LEVER
        {
            OnAcceleratorBegin(other);
        }
        else if (other.name == "LeverTrigger" && VRJoystickTracker.triggerPressed && !ConrolStickLever && !AcceleratelStickLever && !SteeringWheelStick) // STICK ACCELERATE TRIGGER
        { 
            OnAcceleratorBegin(other);
        }

    }

    void OnControlBegin(Collider other)
    {
        joystickOutput = other.transform.root.GetComponent<JoystickOutput>();
        Lever = other.transform.parent.gameObject;
        joystickOutput = other.GetComponentInParent<JoystickOutput>();
        ControlLeverTop = Lever.transform.parent.parent.transform.Find("Relative Center Point").gameObject;
        ControlleverAnimator = Lever.transform.parent.GetComponent<Animator>();
        ControlLeverLastX = ControlleverAnimator.GetFloat("Blend X");
        ControlLeverLastZ = ControlleverAnimator.GetFloat("Blend Z");
        ControlLeverTop.transform.position = transform.position;
        RotateWhenPicked = transform.localEulerAngles.y;
        ConrolStickLever = true;
    }

    void OnAcceleratorBegin(Collider other)
    {
        acceleratorOutPut = other.transform.root.GetComponent<AcceleratorOutput>();
        acceleratorOutPut = other.GetComponentInParent<AcceleratorOutput>();
        AccelerateLever = other.gameObject;
        AcceleratelLeverTop = AccelerateLever.transform.parent.parent.parent.transform.Find("Relative Center Point").gameObject;
        AcceleratorLeverAnimator = AccelerateLever.transform.parent.parent.GetComponent<Animator>();
        AccelerateLeverLastBlend = AcceleratorLeverAnimator.GetFloat("Blend X");
        AcceleratelLeverTop.transform.position = transform.position;
        AcceleratelStickLever = true;
    }

    void UnstickEveryThing()
    {
        if (ConrolStickLever)
        {
            ConrolStickLever = false; // CONTROL LEVER UNSTICK

            Lever.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        if (AcceleratelStickLever)
        {
            AcceleratelStickLever = false; // ACCELERATE LEVER UNSTICK
        }

        if (SteeringWheelStick)
        {
            WheelController.OnUnStick();
            SteeringWheelStick = false; // STEERING WHEEL UNSTICK
            WheelController.Hand = null;
            SteeringWheel = null;
            WheelController = null;
        }
    }

	// Update is called once per frame
	void FixedUpdate () {

        if (SteeringWheelStick) // STEERING WHEEL CONTROLLER
        {
            if (!WheelController.Hand)
            {
                WheelController.Hand = gameObject; // CHECK IF ALREADY HAND GRABBED
            }
            WheelController.OnStick(VRJoystickTracker);
        }

        if (!VRJoystickTracker.triggerPressed) // UNSTICK EVERYTHING
        {
            UnstickEveryThing();
        }

        if (ConrolStickLever) // CONTROL LEVER CONTROLLER 
        {
            ControlleverTopRelative = ControlLeverTop.transform.InverseTransformPoint(transform.position);
            ControlleverAnimator.SetFloat("Blend Z", ControlleverTopRelative.z / 2);
            if (ControlleverAnimator.GetFloat("Blend Z") < 4.5f && ControlleverAnimator.GetFloat("Blend Z") > -4.5f)
            {
                joystickOutput.joyPitch = ControlleverAnimator.GetFloat("Blend Z");
            }

            ControlleverAnimator.SetFloat("Blend X", ControlleverTopRelative.x / 2);

            if (ControlleverAnimator.GetFloat("Blend X") < 4.5f && ControlleverAnimator.GetFloat("Blend X") > -4.5f)
            {
                joystickOutput.joyRoll = ControlleverAnimator.GetFloat("Blend X");
            }

            if(transform.localEulerAngles.y - RotateWhenPicked <= 60 && transform.localEulerAngles.y - RotateWhenPicked >= -60)
            {
            joystickOutput.joyYaw = transform.localEulerAngles.y - RotateWhenPicked;
            Lever.transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y - RotateWhenPicked, 0);
            }
        }

        if (!ConrolStickLever && ControlleverAnimator)
        {
            // UNSTICK CNROL LEVER
            ControlLeverLastX = ControlleverAnimator.GetFloat("Blend X");
            ControlleverPosX = Mathf.Lerp(ControlLeverLastX, 0, Time.time / 150);
            ControlleverAnimator.SetFloat("Blend X", ControlleverPosX);
            joystickOutput.joyRoll = ControlleverPosX;


            joystickOutput.joyYaw = 0;

            ControlLeverLastZ = ControlleverAnimator.GetFloat("Blend Z");
            ControlleverPosY = Mathf.Lerp(ControlLeverLastZ, 0, Time.time/150);
            ControlleverAnimator.SetFloat("Blend Z", ControlleverPosY);
            joystickOutput.joyPitch = ControlleverPosY;

        }



        if (AcceleratelStickLever) // ACCELERATE LEVER CONTROLLER 
        {
            AcceleratorleverTopRelative = AcceleratelLeverTop.transform.InverseTransformPoint(transform.position);
            AcceleratorLeverAnimator.SetFloat("Blend X", -(AcceleratorleverTopRelative.z / 40)+ AccelerateLeverLastBlend);

            if (AcceleratorLeverAnimator.GetFloat("Blend X") > 0 && AcceleratorLeverAnimator.GetFloat("Blend X") < 0.5f)
            {
                if (acceleratorOutPut.withNegative)
                {
                    acceleratorOutPut.accelAxis = (AcceleratorLeverAnimator.GetFloat("Blend X") * 4) - 1f;
                } else
                {
                    acceleratorOutPut.accelAxis = (AcceleratorLeverAnimator.GetFloat("Blend X") * 2);
                }
            }

        }

        if (!AcceleratelStickLever && AcceleratorLeverAnimator)
        {
            // UNSTICK ACELERATE LEVER
            NormalizeAcceleratorBlend();
            acceleratorOutPut = null;

        }


    }

    void NormalizeAcceleratorBlend()
    {
        if (AcceleratorLeverAnimator.GetFloat("Blend X") >= 0.5f)
        {
            AcceleratorLeverAnimator.SetFloat("Blend X", 0.5f);
        } else if (AcceleratorLeverAnimator.GetFloat("Blend X") <= 0)
        {
            AcceleratorLeverAnimator.SetFloat("Blend X", 0);
        }
    }

    void ReturnAccelerator()
    {
        AccelerateLeverLastX = AcceleratorLeverAnimator.GetFloat("Blend X");
        AccelerateleverPosX = Mathf.Lerp(AccelerateLeverLastX, 0, Time.time / 150);
        AcceleratorLeverAnimator.SetFloat("Blend X", AccelerateleverPosX);
    }

}
