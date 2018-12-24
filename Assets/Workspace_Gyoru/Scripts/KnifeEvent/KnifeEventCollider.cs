using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Map内にColliderを設置する時、もっと見えやすく表示するためのEditor用デバッグコード
/// 削除しても問題ない
/// </summary>
public class KnifeEventCollider : MonoBehaviour {

  // [MEMO] KnifeEventScriptスクリプトを持つオブジェクトを指定
	[SerializeField] private KnifeEventScript _eventParentScript;

	// [MEMO] 当たり判定の対象(ターゲット)のオブジェクトのLayerとColliderTypeを指定(なければNONE)
	[SerializeField] private LayerMask _targetLayerMask;

	// Collider Type
	[SerializeField] private KNIFE_EVENT_COLLIDER_TYPE _colliderType;

	public enum KNIFE_EVENT_COLLIDER_TYPE
	{
		None = 0,

		// メス
		Knife = 1,

	  // メスが刺さる壁
		Wall = 2,

		// イベント開始ポイント
		Starter = 3
	};

	// [Editor用] 当たり判定の表示(削除しても問題なし)
	[SerializeField] private bool _isDrawCollider = false;
	private void OnDrawGizmosSelected()
	{
		if (_isDrawCollider)
		{
			Gizmos.color = new Color(1, 0, 0, 0.5f);
			Gizmos.DrawCube(transform.position, transform.localScale);
		}
	}

	/// <summary> 当たり判定イベント </summary>
	private void OnTriggerEnter(Collider other)
	{
		if (_targetLayerMask == (_targetLayerMask | (1 << other.gameObject.layer)))
		{
			switch (_colliderType)
			{
				case KNIFE_EVENT_COLLIDER_TYPE.Wall:
					var script = other.transform.GetComponent<KnifeObject>();
					if (script != null && script is KnifeObject)
					{
						script.EndTargetMoveEvent();
					}
					break;

				case KNIFE_EVENT_COLLIDER_TYPE.Starter:
					_eventParentScript.KnifeEventStart();
					break;
			}
		}
	}
}
