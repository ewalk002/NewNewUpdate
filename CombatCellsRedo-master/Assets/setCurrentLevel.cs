using UnityEngine;
using System.Collections;

public class setCurrentLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PreviousLevel.previousLevel = Application.loadedLevel;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
