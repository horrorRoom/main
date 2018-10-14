using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeObject : MonoBehaviour {
	// Control Variable ==
	[SerializeField] private GameObject _obj;
	[SerializeField] private Rigidbody _rd;
	// ==

	// Value Variable ==
	// Target Position, Rotation (飛んでいく時の目標ポジション・ローテーション)
	[SerializeField] private Vector3 _targetPos;
	[SerializeField] private Quaternion _targetRot;
	// Floating target Position, Rotation (浮く時の目標ポジション・ローテーション)
	[SerializeField] private Vector3 _floatingPos;
	[SerializeField] private Quaternion _floatingRot;
	// ==

	// ナイフの浮くイベントは終了したか？
	private bool _isPositionMoveOver = false;
	// ナイフのターゲットイベントは終了したか？
	private bool _isTargetMoveOver = false;
	public bool IsPositionMoveOver { get { return _isPositionMoveOver; } }
	public bool IsTargetMoveOver { get { return _isTargetMoveOver; } }

	private void Start()
	{
		
	}
}
