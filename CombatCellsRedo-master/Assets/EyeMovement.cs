//source : http://forum.unity3d.com/threads/144743-Isometric-RTS-camera

using UnityEngine;
using System.Collections;

public class EyeMovement : MonoBehaviour {
	public float MIN_X, MIN_Y, MIN_Z, MAX_X, MAX_Y, MAX_Z;
	private  float posX;
	private  float posZ;
	private  float posY;

	private float oposX;
	private float oposY;
	private float oposZ;

	private float zoom;

	int camera_velocity = 40;
	// Use this for initialization
	void Start () {
		posX = transform.position.x;
		posY = transform.position.y;
		posZ = transform.position.z;

		oposX = posX;
		oposY = posY;
		oposZ = posZ;

		zoom = ConstantsLib.ZOOM_FACTOR;
	}
	
	// Update is called once per frame
	void Update () {
		//float translationX = Input.GetAxis ("Horizontal");
		//float translationY = Input.GetAxis ("Vertical");
		//float fastTranslationX = 2 * Input.GetAxis ("Horizontal");
		//float fastTranslationY = 2 * Input.GetAxis ("Vertical");
		//int scrollDistance = 2;
		//float scrollSpeed = 40;
		GameObject Eye = GameObject.Find ("Eye");
		CameraMove ();
		CameraZoom (Eye);
	}
	
	void CameraZoom(GameObject Eye)
	{
		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			if( Eye.camera.orthographicSize > 8 )
			{
				Eye.camera.orthographicSize -= zoom;
			}
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			if( Eye.camera.orthographicSize < 100 )
			{
				Eye.camera.orthographicSize += zoom;
			}
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			Eye.camera.orthographicSize = 50;
		}
	}
	
	void CameraMove()
	{
		if( ( Input.GetKey( KeyCode.W ) ) && 
		   transform.position.y < oposY + MAX_Y )
		{
			
			posY += camera_velocity*Time.deltaTime;
			transform.position = new Vector3(posX,posY,posZ);
			
		}
		if( ( Input.GetKey( KeyCode.A ) ) && 
		   ( ( transform.position.x > ( oposX + MIN_X + 1 ) && //+ camera_velocity*Time.deltaTime * 2 ) && 
		   ( transform.position.z < ( oposZ + MAX_Z + 1 ) ) ) ) )//- camera_velocity*Time.deltaTime * 2 ) ) )
		{
			//Debug.Log( "x: " + oposX + MAX_X + ", " + transform.position.x );
			//Debug.Log( "z: " + oposZ + MIN_Z + ", " + transform.position.z );


			posX -= camera_velocity*Time.deltaTime;
			posZ -= camera_velocity*Time.deltaTime;
			transform.position = new Vector3(posX,posY,posZ);
			
		}
		if( ( Input.GetKey( KeyCode.S ) ) && 
		   transform.position.y > oposY + MIN_Y )
		{
			posY -= camera_velocity*Time.deltaTime;
			transform.position = new Vector3(posX,posY,posZ);
		}
		if( ( Input.GetKey( KeyCode.D ) ) && 
		   ( ( transform.position.x < ( oposX + MAX_X - 1 ) ) && 
		 	( transform.position.z > ( oposZ + MIN_Z - 1 ) ) ) )
		{
			//Debug.Log( "x: " + oposX + ( MAX_X - 1 ) + " > " + transform.position.x );
			//Debug.Log( "z: " + oposZ + ( MIN_Z - 1 ) + " < " + transform.position.z );

			posX += camera_velocity*Time.deltaTime;
			posZ += camera_velocity*Time.deltaTime;
			transform.position = new Vector3(posX,posY,posZ);
		}
	}
}