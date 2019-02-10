using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

/// <summary>
/// ■ Knife Event Script
///  ・ Prefabの中にある'KnifeEventStartCollider'はイベント発動当たり判定なので、必ず座標やスケールなどを指定すること
///  ・ Prefabの中にある'KnifeEventCollider'は各メスが刺さる壁の当たり判定なので、必ず座標やスケールなどを指定し、Layerを「KnifeEvent」にすること
/// </summary>
public class KnifeEventScript : MonoBehaviour {

	// ターゲット(プレイヤのカメラ)
	private Camera _targetCamera = null;
	private Vector3 _targetPosition = Vector3.zero;

	// プレイヤ移動関連スクリプト (移動制御のため)
	private PlayerMove _playerMove = null;

	// スローモーション発動条件(距離)
	[SerializeField, Header("Slow Motion")]
	private AudioClip _slowMotionSE;
	[SerializeField]
	private float _slowMotionDistance = 2.0f;
	private bool _slowMotionStarted = false;
	private Wilberforce.CameraLens.CameraLensCommandBuffer _slowMotionScreenEffect = null;
	private Sequence _slowMotionScreenEffectSequence = null;

	/// <summary> スローモーション発動時間 (SEに合わせた時間) </summary>
	private static readonly float SLOW_MOTION_TIME = 2.8f;

	// 各メスオブジェクト
	[SerializeField, Header("KnifeObject")]
	private KnifeObject[] _knifes;

	// メスが浮かぶ基準となる座標
	[SerializeField]
	private Vector3 _knifeFloatingPosition;
	private Vector3 _knifeFloatingRot;

	// 壁に刺されたメスの数 (イベント終了をチェックするため)
	private int _completedKnifeEventCount = 0;

	[SerializeField] private float _knifeFloatingTime = 2.0f;
	[SerializeField] private float _knifeTargetMoveSpeed = 8.0f;

	[SerializeField, Header("Sound")]
	private AudioSource _audioSource;

	// [TEST] Multiple Sound (各メスが壁に刺さる時間差がある場合に使用)
	[SerializeField]
	private bool _isMultipleKnifeCollisionSound = false;
	[SerializeField]
	private AudioClip _multipleKnifeCollisionSound;

	// メスが壁に刺さる時のサウンド
	[SerializeField]
	private AudioClip[] _knifeCollisionSounds;

	/// <summary>
	/// メスが壁に刺さる時のサウンドの回数制限
	/// (多数のメスが壁に刺さる際、音が一回しか鳴らないと違和感があるため)
	/// _isMultipleKnifeCollisionSoundがfalseの場合のみ使用
	/// </summary>
	[SerializeField]
	private int _maxKnifeCollisionSoundCount = 3;
	private int _nowKnifeCollisionSoundCount = 0;

	private bool _isKnifeEventStarted = false;
	private bool _isCollisionSoundPlayed = false;

	// メスイベント終了時のコールバッグ
	private Action _eventCompleteCallback = null;

	/// <summary>
	/// メスイベント開始
	/// </summary>
	/// <param name="completeCallback"> メスイベント終了時のコールバッグ </param>
	public void KnifeEventStart(Action completeCallback = null)
	{
		if (!_isKnifeEventStarted)
		{
			_eventCompleteCallback = completeCallback;
			StartCoroutine(KnifeEvent());
		}
	}

	/// <summary> 各メスが壁に刺された時のイベント </summary>
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
		// 初期化
		_targetCamera = Camera.main;
		_playerMove = FindObjectOfType<PlayerMove>();
		_isKnifeEventStarted = true;
		_isCollisionSoundPlayed = false;
		_slowMotionStarted = false;
		_nowKnifeCollisionSoundCount = 0;
		_completedKnifeEventCount = 0;
		_targetPosition = _targetCamera.transform.position;

		Vector3 floatingPos = Vector3.zero;
		Vector3 knifePositionForCalcDir = new Vector3(_knifes[0].transform.position.x, 
			_targetPosition.y, _knifes[0].transform.position.z);

		// プレイヤの移動制限をON
		_playerMove.IsMovable = false;

		// Set Rotation
		var dir = (_targetPosition - knifePositionForCalcDir).normalized;
		_knifeFloatingRot = Quaternion.LookRotation(dir, Vector3.up).eulerAngles;
		_knifeFloatingRot.z = 0.0f;

		// メスを空に浮かすイベント開始
		for (int i = 0; i < _knifes.Length; ++i)
		{
			floatingPos = _knifeFloatingPosition + UnityEngine.Random.insideUnitSphere * 0.75f;
			_knifes[i].KnifeStart(floatingPos, _knifeFloatingRot, dir, _knifeFloatingTime, _knifeTargetMoveSpeed);
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.05f, 0.15f));
		}

		// メスが空に浮かぶまで待機
		yield return new WaitForSeconds(2.25f);

		// メスをターゲット(プレイヤ)に向けて発射するイベント開始
		for (int i = 0; i < _knifes.Length; ++i)
			_knifes[i].EndKnifeFloatingEvent();

		// 全てのメスが壁に刺さるまで下のイベントを実行
		while (_completedKnifeEventCount < _knifes.Length)
		{
			// // スローモーションイベント開始 (チェック及び実行)
			if (!_slowMotionStarted)
			{
				if (Vector3.Distance(_targetCamera.transform.position, _knifes[0].transform.position) <= _slowMotionDistance)
				{
					for (int i = 0; i < _knifes.Length; ++i)
						_knifes[i].SetKnifeMoveSpeedByPercent(0.05f);

					ActiveSlowMotionScreenEffect(true);
					_audioSource.PlayOneShot(_slowMotionSE);
					_slowMotionStarted = true;
					yield return new WaitForSeconds(SLOW_MOTION_TIME);

					for (int i = 0; i < _knifes.Length; ++i)
						_knifes[i].SetKnifeMoveSpeedByPercent(1.0f);
				}
			}
			yield return null;
		}

		// メスイベント終了後の処理
		// プレイヤの移動制限をOF
		_playerMove.IsMovable = true;

		if (_eventCompleteCallback != null)
			_eventCompleteCallback.Invoke();
	}

	// スローモーションエフェクト開始及び終了
	private void ActiveSlowMotionScreenEffect(bool active)
	{
		if (active)
		{
			_slowMotionScreenEffect = _targetCamera.gameObject.AddComponent<Wilberforce.CameraLens.CameraLens>();
			_slowMotionScreenEffect.DistortionEnabled = false;
			_slowMotionScreenEffect.DofEnabled = false;
			_slowMotionScreenEffect.BloomEnabled = false;
			_slowMotionScreenEffect.VignetteEnabled = false;
			_slowMotionScreenEffect.ChromaEnabled = true;
			_slowMotionScreenEffect.ChromaWeight = 0.8f;
			_slowMotionScreenEffect.ChromaSize = 0.0f;
			_slowMotionScreenEffect.ChromaRadialness = 1.25f;
			_slowMotionScreenEffectSequence = DOTween.Sequence();
			_slowMotionScreenEffectSequence.Append(DOTween.To(() => _slowMotionScreenEffect.ChromaSize, (size) => _slowMotionScreenEffect.ChromaSize = size, 1.0f, SLOW_MOTION_TIME - 0.25f));
			_slowMotionScreenEffectSequence.Append(DOTween.To(() => _slowMotionScreenEffect.ChromaSize, (size) => _slowMotionScreenEffect.ChromaSize = size, 0.0f, 0.25f)
				.OnComplete(() =>ActiveSlowMotionScreenEffect(false)));
		}
		else
		{
			if (_slowMotionScreenEffect != null)
				Destroy(_slowMotionScreenEffect);
		}
	}
}
