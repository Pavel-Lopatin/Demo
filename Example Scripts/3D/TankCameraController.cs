using System.Collections;
using UnityEngine;

namespace YandexTanks.PlayerTank
{
    public class TankCameraController : MonoBehaviour
    {
        [SerializeField] private Camera gameCamera;
        [SerializeField] private float dampTime;
        [SerializeField, Range(2, 6)] private float cameraDelta;

        [Header("Camera Shake")]
        [SerializeField] private float duration;
        [SerializeField] private float magnitude;

        private Vector3 moveVelocity;

        public void Move(Vector3 tankPos, Vector3 aimPos)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                FindAveragePosition(tankPos, aimPos),
                ref moveVelocity,
                dampTime);
        }

        public IEnumerator MoveCameraToTank(Vector3 tankPos)
        {
            StartCoroutine(ZoomCameraToTank(gameCamera.fieldOfView, gameCamera.fieldOfView/2));

            while (transform.position != tankPos)
            {
                transform.position = Vector3.SmoothDamp(
                transform.position,
                tankPos,
                ref moveVelocity,
                dampTime * 4);

                yield return null;
            }
        }

        private IEnumerator ZoomCameraToTank(float currentCameraSize, float deathCameraSize)
        {
            float t = 0;

            while (gameCamera.fieldOfView > deathCameraSize)
            {
                t += 0.5f * Time.deltaTime;
                gameCamera.fieldOfView = Mathf.Lerp(currentCameraSize, deathCameraSize, t);

                yield return null;
            }
        }

        private Vector3 FindAveragePosition(Vector3 tankPos, Vector3 aimPos)
        {
            Vector3 averagePos = aimPos - tankPos;
            averagePos = tankPos + averagePos / cameraDelta;
            averagePos.y = transform.position.y;

            return averagePos;
        }

        public IEnumerator Shake()
        {
            Vector3 originalPos = gameCamera.transform.localPosition;

            float elapsed = 0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                gameCamera.transform.localPosition += new Vector3(x, y, x);

                elapsed += Time.deltaTime;

                yield return null;
            }

            gameCamera.transform.localPosition = originalPos;
        }
    }
}
