using UnityEngine;

public class AcceleratorOutput : MonoBehaviour {

    public float accelAxis;

    [HideInInspector]
    public bool isTrigger;
    [HideInInspector]
    public bool toggled = false;
    bool toggledOne = false;


    private void Update()
    {
        if(accelAxis > 0.65f && !toggledOne)
        {
            toggled = true;
            toggledOne = true;
            Debug.Log("TOGGLE ONE");
        }

        if (accelAxis < 0.25f && toggledOne)
        {
            toggled = true;
            toggledOne = false;
            Debug.Log("TOGGLE TWO");
        }
    }
}
