using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheelController : MonoBehaviour
{

    [Header("Hand to track")]
    public GameObject Hand;
    public bool HandSticked = false;
    public float wheelLastDegree;

    private float angleStickyOffset;

    [Header("Steering Wheel Base")]
    public GameObject WheelBase;

    [Header("Wheel & Hand relative position")]
    public Vector3 RelativePos;

    [Header("Output steering wheel angle")]
    public float outputAngle;
    public TextMesh textDisplay;

    [Header("Arrays Values (Debug)")]
    public List<float> lastValues = new List<float>(); // stores last angles
    public List<float> Diffs = new List<float>(); // stores difference between each last angles
    public List<float> formulaDiffs = new List<float>(); // stores formulated diffs
    public List<float> increment = new List<float>(); // calculating incrementation

    void Start()
    {
        CreateArrays(5); // CALLING FUNCTION WHICH CREATES ARRAYS
        angleStickyOffset = 0f;
        HandSticked = false;
        wheelLastDegree = CalculateRawAngle();
    }

    public void OnStick()
    {

        if (HandSticked != true)
        {
            CalculateOffset();
        }
        HandSticked = true;

    }

    void CalculateOffset()
    {
        float rawAngle = CalculateRawAngle();
        angleStickyOffset = wheelLastDegree - rawAngle;
    }

    public void OnUnStick()
    {
        HandSticked = false;
        wheelLastDegree = outputAngle;
        textDisplay.text = Mathf.Round(wheelLastDegree) + "" + ".00 deg. LAST";
    }

    float CalculateRawAngle()
    {
        RelativePos = WheelBase.transform.InverseTransformPoint(Hand.transform.position); // GETTING RELATIVE POSITION BETWEEN STEERING WHEEL BASE AND HAND
        
        return Mathf.Atan2(RelativePos.y, RelativePos.x) * Mathf.Rad2Deg; // GETTING CIRCULAR DATA FROM X & Y RELATIVES  VECTORS
    }

    void FixedUpdate()
    {
        if (HandSticked)
        {
            float angle = CalculateRawAngle() + angleStickyOffset;

            lastValues.RemoveAt(0); // REMOVING FIRST ITEM FROM ARRAY
            lastValues.Add(angle); // ADD LAST ITEM TO ARRAY

            outputAngle = hookedAngles(angle);// SETTING OUTPUT THROUGH FUNCTION
            textDisplay.text = Mathf.Round(outputAngle) + "" + ".00 deg." + Time.time + " offset "+ angleStickyOffset;

            transform.localEulerAngles = new Vector3(outputAngle, -90, -90);// ROTATE WHEEL MODEL
        }
    }


    void CreateArrays(int firstPparam) // FUNCTION WHICH CREATING ARRAYS
    {
        for (int i = 0; i < firstPparam; i++) // FOR LOOP WITH PARAM
        {
            lastValues.Add(0);  // LAST VALUES ARRAY
        }


        for (int i = 0; i < firstPparam - 1; i++) // FOR LOOP WITH PARAM -1
        {
            Diffs.Add(0); // ARRAY TO STORE DIFFERECE BETWEEN NEXT AND PREV
            formulaDiffs.Add(0); // ARRAY TO STORE FORMULATED DIFFS
            increment.Add(0); //  ARRAY TO STORE INCREMENT FOR EACH WHEEL SPIN

        }
    }


    public float hookedAngles(float angle) // FORMULATING AND CALCULATING FUNCTION WHICH COUNTS SPINS OF WHEEL
    {
        float period = 360;
        for (int i = 0; i < lastValues.Count - 1; i++)
        {
            Diffs.RemoveAt(0);
            Diffs.Add(lastValues[i + 1] - lastValues[i]);
        }

        for (int i = 0; i < formulaDiffs.Count; i++)
        {
            formulaDiffs.RemoveAt(0);
            var a = (Diffs[i] + period / 2.0f);
            var b = period;
            var fdiff = a - Mathf.Floor(a / b) * b;
            formulaDiffs.Add(fdiff - period / 2);
        }

        for (int i = 0; i < formulaDiffs.Count; i++)
        {
            increment.RemoveAt(0);
            increment.Add(formulaDiffs[i] - Diffs[i]);
        }

        for (int i = 1; i < formulaDiffs.Count; i++)
        {
            increment[i] += increment[i - 1];
        }

        lastValues[4] += increment[3];

        return lastValues[4] - 0; // COLLIBRATE TO ZERO WHEN STILL AND RETURN CALCULATED VALUE
    }

}
