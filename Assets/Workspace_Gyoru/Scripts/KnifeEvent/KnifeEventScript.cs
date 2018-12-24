using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ■ Knife Event Script
///  ・ メスが壁に刺さるようにする場合、壁のオブジェクトのLayerを「KnifeEvent」に指定すること
/// </summary>
public class KnifeEventScript : MonoBehaviour {

	[SerializeField, Header("Target")]
	private Transform _targetTransform;
	private Vector3 _targetPosition = Vector3.zero;

	[SerializeField, Header("KnifeObjects")]
	private KnifeObject[] _knifes;
	private Vector3 _knifeFloatingRot;
	private Vector3 _knifeMoveDirection;
	private int _completedKnifeEventCount = 0;

	[SerializeField] private Vector3 _knifeFloatingRandomRangeMin;
	[SerializeField] private Vector3 _knifeFloatingRandomRangeMax;
	[SerializeField] private float _knifeFloatingTime;
	[SerializeField] private float _knifeTargetMoveSpeed;

	[SerializeField, Header("Sounds")]
	private AudioSource _audioSource;

	// [TEST] Multiple Sound
	[SerializeField]
	private bool _isMultipleKnifeCollisionSound = false;
	[SerializeField]
	private AudioClip _multipleKnifeCollisionSound;

	// Single Sound
	[SerializeField]
	private AudioClip[] _knifeCollisionSounds;

	/// <summary>
	/// 一回のイベントで出すメスサウンド制限回数
	/// _isMultipleKnifeCollisionSoundがfalseの場合のみ使用
	/// (Volumeを調整のため)
	/// </summary>
	[SerializeField]
	private int _maxKnifeCollisionSoundCount = 3;
	private int _nowKnifeCollisionSoundCount = 0;

	private bool _isKnifeEventStarted = false;
	private bool _isCollisionSoundPlayed = false;

	// Event Callback
	private UnityAction _eventCompleteCallback = null;

	public void KnifeEventStart(UnityAction completeCallback = null)
	{
		if (!_isKnifeEventStarted)
		{
			_eventCompleteCallback = completeCallback;
			StartCoroutine(KnifeEvent());
		}
	}

	public void KnifeCollisionEvent()
	{
		++_completedKnifeEventCount;

		// Play Sound
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
		// Set Value
		_isKnifeEventStarted = true;
		_isCollisionSoundPlayed = false;
		_nowKnifeCollisionSoundCount = 0;
		_completedKnifeEventCount = 0;
		_targetPosition = _targetTransform.position;

		Vector3 floatingPos = Vector3.zero;
		Vector3 knifePositionForCalcDir = new Vector3(_knifes[0].transform.position.x, 
			_targetPosition.y, _knifes[0].transform.position.z);

		// Set Rotation
		var dir = (_targetPosition - knifePositionForCalcDir).normalized;
		_knifeFloatingRot = Quaternion.LookRotation(dir, Vector3.up).eulerAngles;
		_knifeFloatingRot.z = 0.0f;

		// Start Knife Event
		for (int i = 0; i < _knifes.Length; ++i)
		{
			floatingPos = GetKnifeFloatingPosition(_knifes[i].transform.localPosition);
			_knifes[i].KnifeStart(floatingPos, _knifeFloatingRot, dir, 2.0f, 8.0f);
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.05f, 0.15f));
		}

		// Wait last object complete floating event (メスが空を飛ぶまで待機)
		yield return new WaitForSeconds(2.25f);

		// メスをターゲットに向けて発射
		for (int i = 0; i < _knifes.Length; ++i)
			_knifes[i].EndKnifeFloatingEvent();

		// Wait Knife Event Complete
		while (_completedKnifeEventCount == _knifes.Length)
			yield return null;

		// End Knife Event
		// [TODO] Knife 位置・状態をリセット
		// _isKnifeEventStarted = false;
		if (_eventCompleteCallback != null)
			_eventCompleteCallback.Invoke();
	}

	private Vector3 GetKnifeFloatingPosition(Vector3 pos)
	{
		Vector3 result = new Vector3(
			pos.x * UnityEngine.Random.Range(1.5f, 4.0f),
			UnityEngine.Random.Range(pos.y + 0.1f, pos.y + 1.5f),
			pos.z);

		return result;
	}
}
