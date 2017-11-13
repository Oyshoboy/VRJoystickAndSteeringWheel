using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoysctickController : MonoBehaviour {

    public GameObject LeverTop;
    public SteamVR_TrackedController VRJoystickTracker;
    public Vector3 leverTopRelative;
    public Animator leverAnimator;
    bool StickLever = false;

    Vector3 PostitionWhenSticked;

    float LeverLastX;
    float LeverLastY;

    float leverPosY;
    float leverPosX;

    // Use this for initialization
    void Start () {
		
	}
	
    void OnTriggerStay(Collider other)
    {
     if (other.name == "Lever" && VRJoystickTracker.triggerPressed && !StickLever)
        {
            LeverTop.transform.position = transform.position;
            StickLever = true;
        }
    }

	// Update is called once per frame
	void FixedUpdate () {

        Debug.Log(transform.eulerAngles);

        if (StickLever)
        {
            leverTopRelative = LeverTop.transform.InverseTransformPoint(transform.position);
            leverAnimator.SetFloat("Blend Y", leverTopRelative.z / 2);
            leverAnimator.SetFloat("Blend X", leverTopRelative.x / 2);
            if (!VRJoystickTracker.triggerPressed)
            {
                StickLever = false;
            }
        }

        if (!StickLever)
        {
            LeverLastX = leverAnimator.GetFloat("Blend X");
            LeverLastY = leverAnimator.GetFloat("Blend Y");
            leverPosY = Mathf.Lerp(LeverLastY, 0, Time.time/150);
            leverAnimator.SetFloat("Blend Y", leverPosY);

            leverPosX = Mathf.Lerp(LeverLastX, 0, Time.time/150);
            leverAnimator.SetFloat("Blend X", leverPosX);
        }
    }

}
