using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeEvent : MonoBehaviour {
	[SerializeField]
	private KnifeObject[] _knifes;
	private bool _isKnifeEventStarted = false;

	public void KnifeEventStart()
	{
		if (!_isKnifeEventStarted)
		{

			_isKnifeEventStarted = true;
		}
	}

	/*private IEnumerator KnifeEventLoop()
	{

	}*/
}
