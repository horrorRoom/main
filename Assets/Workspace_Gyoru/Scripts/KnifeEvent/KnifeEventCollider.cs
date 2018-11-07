using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeEventCollider : MonoBehaviour {
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(1, 0, 0, 0.5f);
		Gizmos.DrawCube(transform.position, transform.localScale);
	}
}
