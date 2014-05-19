using UnityEngine;
using System.Collections;

public class NextWaySide : MonoBehaviour {

	public GameObject[] WaySideQueue;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter( Collider other )
	{
		if( other.tag == ConstantsLib.ENEMY_TAG )
		{
			int index = Random.Range( 0, WaySideQueue.Length );

			//Debug.Log( index );

			other.GetComponent<NavMeshAgent>().SetDestination( 
			                                                  WaySideQueue[ Random.Range( 0, 
			                           											WaySideQueue.Length ) ].transform.position );


			//other.GetComponent<NavMeshAgent>().SetDestination( WaySideQueue[0].transform.position );
			//Debug.Log( other.GetComponent<NavMeshAgent>().destination );
		}
	}
}
