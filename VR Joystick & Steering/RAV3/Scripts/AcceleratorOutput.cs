using UnityEngine;

public class AcceleratorOutput : MonoBehaviour {

    public float accelAxis;  // INHERITED FROM CONTROLS MANAGER ( IN HANDS )

    [HideInInspector]
    public bool isTrigger;
    [HideInInspector]
    public bool toggled = false;
    bool toggledOne = false;
    public bool withNegative = false;
    public TextMesh TextState;

    private void Update()
    {
        if (TextState)
        {
            if (accelAxis > -0.3 && accelAxis < 0.3)
            {
                TextState.text = "Brake";
            }
            else if (accelAxis > 0)
            {
                TextState.text = "Forward";
            }
            else if (accelAxis < 0)
            {
                TextState.text = "Reverse";
            }
        }

        if(accelAxis > 0.65f && !toggledOne)
        {
            toggled = true;
            toggledOne = true;
        }

        if (accelAxis < 0.25f && toggledOne)
        {
            toggled = true;
            toggledOne = false;
        }
    }
}
