using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ■ Knife Event Script
///  ・ メスが飛ぶ方向を指定すること
///  ・ メスが刺さるObjectのLayerを「KnifeEvent」に指定すること
/// </summary>
public class KnifeEventScript : MonoBehaviour {
	[SerializeField, Header("KnifeObject")]
	private KnifeObject[] _knifes;
	[SerializeField]
	private Vector3 KnifeFloatingRot;

	[SerializeField, Header("Sounds")]
	private AudioSource _audioSource;
	private bool _isCollisionSoundPlayed = false;

	// Single Sound
	[SerializeField]
	private AudioClip[] _knifeCollisionSounds;

	// [TEST] Multiple Sound
	[SerializeField]
	private AudioClip _multipleKnifeCollisionSound;
	[SerializeField]
	private bool _isMultipleKnifeCollisionSound = false;

	/// <summary>
	/// 一回のイベントで出すメスサウンド制限回数
	/// (Volumeを調整)
	/// </summary>
	[SerializeField]
	private int _maxKnifeCollisionSoundCount = 3;
	private int _nowKnifeCollisionSoundCount = 0;

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

	public void PlayKnifeCollisionSound()
	{
		if (!_isCollisionSoundPlayed && _isMultipleKnifeCollisionSound && _multipleKnifeCollisionSound != null)
			_audioSource.PlayOneShot(_multipleKnifeCollisionSound);
		else if (!_isMultipleKnifeCollisionSound && _nowKnifeCollisionSoundCount < _maxKnifeCollisionSoundCount && _knifeCollisionSounds != null)
		{
			++_nowKnifeCollisionSoundCount;
			_audioSource.PlayOneShot(_knifeCollisionSounds[UnityEngine.Random.Range(0, _knifeCollisionSounds.Length)]);
		}
			
		_isCollisionSoundPlayed = true;
	}

	private IEnumerator KnifeEvent()
	{
		_isKnifeEventStarted = true;
		_isCollisionSoundPlayed = false;
		_nowKnifeCollisionSoundCount = 0;

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
