using UnityEngine;

namespace Platformer.GameplayObjects
{
	public class PlatformVelocityController : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField] private Rigidbody2D rb;
		[SerializeField] private BoxCollider2D exitTrigger;

		// =============== Move ===============
		private Vector2 lastPosition;
		private Vector2 currentPosition;
		private Vector2 currentSpeed;

		private void FixedUpdate()
		{
			currentPosition = rb.position;

			CalculateVelocity();
		}

		public float GetPlatformVelocity()
		{
			return currentSpeed.x;
		}

		public void SetTriggerState(bool state)
		{
			exitTrigger.enabled = state;
		}

		private void CalculateVelocity()
		{
			currentSpeed = currentPosition - lastPosition;
			currentSpeed /= Time.deltaTime;

			lastPosition = currentPosition;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			PlayerInteractive playerInteractive = other.GetComponentInChildren<PlayerInteractive>();
			
			if (playerInteractive != null)
			{
				playerInteractive.SetPlatform(this);
				SetTriggerState(true);
			}
		}
	}
}