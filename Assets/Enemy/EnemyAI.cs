using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
	[SerializeField]
	private float speed = 2f;
	[SerializeField]
	private float threshold = 0.1f;
	[SerializeField]
	private List<Vector3> moveCheckOffsets;

	private Rigidbody2D rigidbody;
	private Tilemap mazeTilemap;

	private Vector3 targetPos;
	private Vector3 behindDirectionOffset;

	// Start is called before the first frame update
	private void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		mazeTilemap = GameObject.FindGameObjectWithTag("Maze").GetComponent<Tilemap>();

		targetPos = transform.position;
		behindDirectionOffset = Vector3.zero;

		PlayerAbility.OnExplosion += OnExplosion;
	}

	private void OnDestroy()
	{
		PlayerAbility.OnExplosion -= OnExplosion;
	}


	private void Update()
	{
		//rigidbody.MovePosition(Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed));
		Vector3 movement = (targetPos - transform.position).normalized;
		rigidbody.MovePosition(transform.position + movement * speed * Time.deltaTime);

		if (Vector3.Distance(transform.position, targetPos) < threshold)
		{
			rigidbody.MovePosition(targetPos);
			targetPos = GetNextPosition();
		}
	}

	private Vector3 GetNextPosition()
	{
		// Shuffle the offset list
		int n = moveCheckOffsets.Count;
		while (n > 1)
		{
			n--;
			int k = Random.Range(0, n + 1);
			Vector3 switchValue = moveCheckOffsets[k];
			moveCheckOffsets[k] = moveCheckOffsets[n];
			moveCheckOffsets[n] = switchValue;
		}

		int index = moveCheckOffsets.FindIndex(offset => offset == behindDirectionOffset);
		if(index >= 0 && index < moveCheckOffsets.Count)
		{
			Vector3 value = moveCheckOffsets[index];
			moveCheckOffsets[index] = moveCheckOffsets[moveCheckOffsets.Count - 1];
			moveCheckOffsets[moveCheckOffsets.Count - 1] = value;
		}

		// Find first offset which is free to move in
		foreach (Vector3 offset in moveCheckOffsets)
		{
			Vector3 positionFloat = targetPos + offset;
			Vector3Int position = new Vector3Int(Mathf.RoundToInt(positionFloat.x), Mathf.RoundToInt(positionFloat.y), 0);
			//Vector3Int position = new Vector3Int((int)positionFloat.x, (int)positionFloat.y, 0);
			if (mazeTilemap.GetTile(position) == null)
			{
				behindDirectionOffset = offset * -1f;
				return positionFloat;
			}
		}

		return transform.position;
	}

	private void OnExplosion(Vector3 explosionPosition)
	{
		float distance = Vector3.Distance(transform.position, explosionPosition);
		if(distance < PlayerAbility.Instance.ExplosionRange)
		{
			Die();
		}
	}

	private void Die()
	{
		Destroy(gameObject);
	}
}
