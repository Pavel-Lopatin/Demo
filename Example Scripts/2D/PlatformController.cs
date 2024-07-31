using UnityEngine;
using System.Collections.Generic;

namespace Platformer.GameplayObjects
{
	public class PlatformController : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField] private Rigidbody2D rb;
		[SerializeField] private Transform platform;

		[Header("Parametrs")]
		[SerializeField] private float speed;
		
		[Header("Objects")]
		[SerializeField] private GameObject pointPrefab;

		[HideInInspector] public List<Transform> movePoints = new List<Transform>();
		[HideInInspector] public bool isLoop;
		
		private Vector2 moveDirection;
		private Transform nextPoint;
		
		private bool isMoveToEndPoint = true;
		private int movePointIndex = 0;

		private void Awake()
		{
			nextPoint = movePoints[0];
			moveDirection = nextPoint.position - platform.position;
			moveDirection.Normalize();
		}

		private void Update()
		{
			if (movePoints.Count < 2) return;

			if (Vector2.Distance(platform.position, nextPoint.position) <= 0.1f)
			{
				CalculateNextPoint();
			}
		}

		private void FixedUpdate()
		{
			Vector2 currentVelocity = Vector2.SmoothDamp(rb.position, nextPoint.position, ref moveDirection, 1, speed);
			rb.position = currentVelocity;
		}
		
		public GameObject GetPointPrefab() 
		{
			return pointPrefab;
		}

		private void CalculateNextPoint()
		{
			if (isMoveToEndPoint)
			{
				movePointIndex++;
				nextPoint = movePoints[movePointIndex];

				if (movePointIndex == movePoints.Count - 1) isMoveToEndPoint = false;
			}
			else if (!isMoveToEndPoint)
			{
				movePointIndex = isLoop ? 0 : --movePointIndex;
				
				nextPoint = movePoints[movePointIndex];

				if (movePointIndex == 0) isMoveToEndPoint = true;
			}

			moveDirection = nextPoint.position - platform.position;
			moveDirection.Normalize();
		}
		
		private void OnDrawGizmos()
		{
			if (movePoints.Count < 2) return;

			Gizmos.color = Color.white;
			float step = 0.25f;

			for (int i = 0; i + 1 < movePoints.Count; i++)
			{
				Vector3 newDirection = movePoints[i + 1].position - movePoints[i].position;
				int count = Mathf.RoundToInt(newDirection.magnitude / step);

				for (int p = 1; p < count; p++)
				{
					Vector3 linePoint = movePoints[i].position + newDirection * (step * p / newDirection.magnitude);
					Gizmos.DrawLine(linePoint - newDirection.normalized * (step / 4), linePoint + newDirection.normalized * (step / 4));
				}
			}
			
			if (isLoop) 
			{
				Vector3 newDirection = movePoints[movePoints.Count - 1].position - movePoints[0].position;
				int count = Mathf.RoundToInt(newDirection.magnitude / step);

				for (int p = 1; p < count; p++)
				{
					Vector3 linePoint = movePoints[movePoints.Count - 1].position - newDirection * (step * p / newDirection.magnitude);
					Gizmos.DrawLine(linePoint - newDirection.normalized * (step / 4), linePoint + newDirection.normalized * (step / 4));
				}
			}
		}
	}
}