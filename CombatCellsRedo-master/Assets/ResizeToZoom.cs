using UnityEngine;
using System.Collections;

public class ResizeToZoom : MonoBehaviour {

	private float previousOrthoSize = 1f;
	private float currentOrthoSize = 1f;
	private float cameraScalar = 0f;

	// Use this for initialization
	void Start () {
		currentOrthoSize = Camera.main.orthographicSize;
		previousOrthoSize = currentOrthoSize;
	}

	void updateCameraScalar()
	{
		currentOrthoSize = Camera.main.orthographicSize;
		
		if( currentOrthoSize != previousOrthoSize )
		{
			cameraScalar = currentOrthoSize/previousOrthoSize;
			transform.localScale *= ( cameraScalar );			
			previousOrthoSize = currentOrthoSize;
		}
	}

	// Update is called once per frame
	void Update () {
		updateCameraScalar();
	}
}
