using UnityEngine;
using System.Collections;

public class Planets_SelfRotation_LPSS : MonoBehaviour 
{

    public float RotationSpeedDPerS = 10.0f;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {

        this.transform.Rotate( 0.0f, 0.0f, Time.deltaTime * this.RotationSpeedDPerS );	
	}

}
