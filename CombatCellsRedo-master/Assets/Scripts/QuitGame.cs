﻿using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKey( "escape" ) )
		{
			Application.Quit();
		}
	}

	void OnMouseDown()
	{
		Application.Quit ();
	}
}