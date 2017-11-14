using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoysctickController : MonoBehaviour {

    public GameObject Lever;
    float RotateWhenPicked;

    public GameObject RealHand;
    public GameObject HandOnControl;
    public GameObject HandOnAccel;

    public GameObject ControlLeverTop; // LEVERS TOP POINT
    public Animator ControlleverAnimator; // ANIMATOR

    public Vector3 ControlleverTopRelative;// RELATIVE POINT
    bool ConrolStickLever = false;

    float ControlLeverLastX;
    float ControlLeverLastY; // LAST POSITION FOR LERPING

    float ControlleverPosY;
    float ControlleverPosX; // CURRENT POS


    public SteamVR_TrackedController VRJoystickTracker;


    public GameObject AcceleratelLeverTop;
    public Animator AcceleratorLeverAnimator;

    public Vector3 AcceleratorleverTopRelative;
    bool AcceleratelStickLever = false;

    float AccelerateLeverLastX;
    float AccelerateLeverLastY; // LAST POSITION FOR LERPING

    float AccelerateleverPosY;
    float AccelerateleverPosX; // CURRENT POS


    // Use this for initialization
    void Start () {
		
	}
	
    public float getLeverRotate(float LeverAngle)
    {
        if(LeverAngle > 180)
        {
           LeverAngle-= 360;
        }
        return LeverAngle;
    }

    void OnTriggerStay(Collider other)
    {
     if (other.name == "LeverControl" && VRJoystickTracker.triggerPressed && !ConrolStickLever && !AcceleratelStickLever) // STICK CONTROL LEVER
        {
            ControlLeverTop.transform.position = transform.position;
            RotateWhenPicked = transform.parent.localEulerAngles.y;
            ConrolStickLever = true;

        }

        if (other.name == "LeverAccelerate" && VRJoystickTracker.triggerPressed && !ConrolStickLever && !AcceleratelStickLever) // STICK ACCELERATE LEVER
        {
            AcceleratelLeverTop.transform.position = transform.position;
            AcceleratelStickLever = true;

        }
    }

	// Update is called once per frame
	void Update () {
        //Debug.Log(getLeverRotate(Lever.transform.localEulerAngles.y));

        if (ConrolStickLever) // CONTROL LEVER CONTROLLER 
        {
            Debug.Log(transform.parent.name +" rotating:"+ Lever.transform.localEulerAngles.y + " self Rotating" + transform.parent.localEulerAngles.y);

            RealHand.SetActive(false);
            HandOnControl.SetActive(true);

            Lever.transform.localEulerAngles= new Vector3(0, transform.parent.localEulerAngles.y-RotateWhenPicked, 0); 

            ControlleverTopRelative = ControlLeverTop.transform.InverseTransformPoint(transform.position);
            ControlleverAnimator.SetFloat("Blend Z", ControlleverTopRelative.z / 2);
            ControlleverAnimator.SetFloat("Blend X", ControlleverTopRelative.x / 2);

            if (!VRJoystickTracker.triggerPressed)
            {
                ConrolStickLever = false;
                RealHand.SetActive(true);
                HandOnControl.SetActive(false);
                Lever.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
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

            if (!VRJoystickTracker.triggerPressed)
            {
                AcceleratelStickLever = false;
                RealHand.SetActive(true);
                HandOnAccel.SetActive(false);
            }
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
