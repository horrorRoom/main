using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ■ Knife Event Script
///  ・ メスが飛ぶ方向を指定すること
///  ・ メスが刺さるObjectのLayerを「KnifeEvent」に指定すること
/// </summary>
public class KnifeEventScript : MonoBehaviour {
	[SerializeField] private KnifeObject[] _knifes;
	[SerializeField] private Vector3 KnifeFloatingRot;
	private Vector3 KnifeMoveDirection;

	private bool _isKnifeEventStarted = false;

	private void Start()
	{
		// [TODO] 方向を変更可能にすること
		KnifeMoveDirection = transform.right;
	}

	// DEBUG
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
			KnifeEventStart();
	}

	public void KnifeEventStart()
	{
		if (!_isKnifeEventStarted)
			StartCoroutine(KnifeEvent());
	}

	private IEnumerator KnifeEvent()
	{
		_isKnifeEventStarted = true;

		for (int i = 0; i < _knifes.Length; ++i)
		{
			_knifes[i].KnifeStart(GetKnifeFloatingPosition(
				_knifes[i].transform.localPosition),
				KnifeFloatingRot,
				KnifeMoveDirection,
				2.0f,
				8.0f);

			yield return new WaitForSeconds(UnityEngine.Random.Range(0.05f, 0.15f));
		}

		// Wait last object floating event
		yield return new WaitForSeconds(2.25f);

		for (int i = 0; i < _knifes.Length; ++i)
			_knifes[i].EndKnifeFloatingEvent();
	}

	private Vector3 GetKnifeFloatingPosition(Vector3 pos)
	{
		Vector3 result = new Vector3(
			pos.x,
			UnityEngine.Random.Range(pos.y + 0.5f, pos.x + 2.0f),
			UnityEngine.Random.Range(pos.z - 0.3f, pos.z + 0.3f));

		return result;
	}
}
