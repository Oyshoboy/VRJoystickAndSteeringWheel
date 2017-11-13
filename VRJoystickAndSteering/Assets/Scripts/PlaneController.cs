using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour {

    public Animator JosytickAnimator;
    float X;
    float Y;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        X = JosytickAnimator.GetFloat("Blend X")*20;
        Y = JosytickAnimator.GetFloat("Blend Y")*20;
        Quaternion rotation = Quaternion.Euler(Y,0,-X);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime*2); 
        transform.position = transform.position + new Vector3(transform.forward.x, transform.forward.y, transform.forward.z) / 30;
    }
}
