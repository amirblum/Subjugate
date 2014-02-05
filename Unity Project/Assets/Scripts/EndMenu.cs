using UnityEngine;
using System.Collections;

public class EndMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad > 2.5 && Input.anyKeyDown)
		{
			Application.LoadLevel(1);
		}
	}
}
