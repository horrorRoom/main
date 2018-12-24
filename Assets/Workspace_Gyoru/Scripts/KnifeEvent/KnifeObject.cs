using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KnifeObject : MonoBehaviour {
	// Control Variable ==
	[SerializeField] private KnifeEventScript _parentScript;
	[SerializeField] private BoxCollider _coll;
	// ==

	// Floating target Position, Rotation (浮く時の目標ポジション・ローテーション)
	private Vector3 _floatingPos = Vector3.zero;
	private Vector3 _floatingRot = Vector3.zero;
	private float _floatingTime = 0.0f;

	// Target Position, Rotation (飛んでいく時の目標ポジション・ローテーション)
	private Vector3 _targetMoveDirection = Vector3.zero;
	private Vector3 _targetPos = Vector3.zero;
	private float _targetMoveSpeed = 0.0f;

	/// <summary> メスが空を飛ぶイベントは終了したか？ </summary>
	private bool _isFloatingMoveOver = false;
	/// <summary>  メスがターゲットに向かって飛ぶイベントは終了したか？ </summary>
	private bool _isTargetMoveOver = false;
	public bool IsFloatingMoveOver { get { return _isFloatingMoveOver; } }
	public bool IsTargetMoveOver { get { return _isTargetMoveOver; } }

	private Coroutine C_KnifeEvent = null;

	public void KnifeStart(Vector3 floatingPos, Vector3 floatingRot, Vector3 targetMoveDirection, float floatingTime, float targetMoveSpeed)
	{
		_floatingPos = floatingPos;
		_floatingRot = floatingRot;
		_floatingTime = floatingTime;
		_targetMoveDirection = targetMoveDirection;
		_targetMoveSpeed = targetMoveSpeed;

		_isFloatingMoveOver = false;
		_isTargetMoveOver = false;

		if (C_KnifeEvent != null) StopCoroutine(C_KnifeEvent);
		C_KnifeEvent = StartCoroutine(KnifeEventStart());
	}

	public void EndKnifeFloatingEvent()
	{
		_isFloatingMoveOver = true;
	}

	/// <summary> Knifeが壁に刺された時のイベント </summary>
	public void EndTargetMoveEvent()
	{
		_isTargetMoveOver = true;
		_coll.enabled = false;

		// Play Collision Sound
		_parentScript.KnifeCollisionEvent();
	}

	private IEnumerator KnifeEventStart()
	{
		// Start Floating
		transform.DOLocalMove(_floatingPos, _floatingTime).SetEase(Ease.InOutQuad);
		transform.DORotate(_floatingRot, _floatingTime).SetEase(Ease.InOutQuad);
		yield return new WaitForSeconds(_floatingTime);

		// Wait for next Event
		while (!_isFloatingMoveOver)
			yield return null;

		// Move to Target
		while (!_isTargetMoveOver)
		{
			transform.position += _targetMoveDirection * _targetMoveSpeed * Time.deltaTime;
			yield return null;
		}
	}
}
