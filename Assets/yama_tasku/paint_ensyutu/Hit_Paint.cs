using UnityEngine;
using System.Collections;

public class Hit_Paint : MonoBehaviour {
	public bool Hit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.collider.gameObject.name == "Ground")
			Hit = true;
	}
}
