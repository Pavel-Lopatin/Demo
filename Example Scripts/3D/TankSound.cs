using UnityEngine;

namespace YandexTanks.PlayerTank
{
    public class TankSound : MonoBehaviour
    {
        [Header("Single Sounds")]
        [SerializeField] private AudioSource shotSound;
        [SerializeField] private AudioSource impactSound;

        [Header("Moving forward sound")]
        [SerializeField] private AudioSource engineAudioMoving;
        [SerializeField] private float soundRateMoving = 1;
        [SerializeField] private float minPitchMoving = 0.2f;
        [SerializeField] private float maxPitchMoving = 1f;
        [SerializeField] private float minVolumeMoving = 0.3f;
        [SerializeField] private float maxVolumeMoving = 0.7f;
        [Space]
        private float currentPitchMoving = 1;
        private float currentVolumeMoving;
        private float verticaInput;

        [Header("Rotation sound")]
        [SerializeField] private AudioSource engineAudioRotation;
        [SerializeField] private float soundRateRotation = 1;
        [SerializeField] private float minPitchRotation = 0.2f;
        [SerializeField] private float maxPitchRotation = 1f;
        [SerializeField] private float minVolumeRotation = 0.3f;
        [SerializeField] private float maxVolumeRotation = 0.7f;

        private float currentPitchRotation = 1;
        private float currentVolumeRotation;
        private float horizontalInput;

        public void Init()
        {
            engineAudioMoving.volume = 0;
            engineAudioRotation.volume = 0;
        }

        public void SetInput(Vector2 input, bool enable)
        {
            verticaInput = input.x;
            horizontalInput = input.y;

            if (enable)
            {
                PlayMovingSound();
                PlayRotationSound();
            }
            else
            {
                engineAudioMoving.volume = 0;
                engineAudioRotation.volume = 0;
            }

        }

        private void PlayMovingSound()
        {
            if (engineAudioMoving == null || !engineAudioMoving.enabled) return;

            if (verticaInput != 0)
            {
                currentPitchMoving =
                    Mathf.Lerp(currentPitchMoving, maxPitchMoving * Mathf.Abs(verticaInput), Time.deltaTime * soundRateMoving);
                currentVolumeMoving =
                    Mathf.Lerp(currentVolumeMoving, maxVolumeMoving * Mathf.Abs(verticaInput), Time.deltaTime * soundRateMoving);
            }
            else
            {
                currentPitchMoving =
                    Mathf.Lerp(currentPitchMoving, minPitchMoving, Time.deltaTime * soundRateMoving);
                currentVolumeMoving =
                    Mathf.Lerp(currentVolumeMoving, minVolumeMoving, Time.deltaTime * soundRateMoving);
            }

            engineAudioMoving.pitch = currentPitchMoving - 0.3f * currentPitchRotation;
            engineAudioMoving.volume = currentVolumeMoving;
        }

        private void PlayRotationSound()
        {
            if (engineAudioRotation == null || !engineAudioRotation.enabled) return;

            if (horizontalInput != 0)
            {
                currentPitchRotation =
                    Mathf.Lerp(currentPitchRotation, maxPitchRotation * Mathf.Abs(horizontalInput), Time.deltaTime * soundRateRotation);
                currentVolumeRotation =
                    Mathf.Lerp(currentVolumeRotation, maxVolumeRotation * Mathf.Abs(horizontalInput), Time.deltaTime * soundRateRotation);
            }
            else
            {
                currentPitchRotation =
                    Mathf.Lerp(currentPitchRotation, minPitchRotation, Time.deltaTime * soundRateRotation);
                currentVolumeRotation =
                    Mathf.Lerp(currentVolumeRotation, minVolumeRotation, Time.deltaTime * soundRateRotation);
            }

            engineAudioRotation.pitch = currentPitchRotation;
            engineAudioRotation.volume = currentVolumeRotation;
        }

        public void PlayShotSound()
        {
            shotSound.PlayOneShot(shotSound.clip);
        }

        public void PlayImpactSound()
        {
            impactSound.PlayOneShot(impactSound.clip);
        }

        public void Destroy()
        {
            engineAudioMoving.Stop();
            engineAudioRotation.Stop();
        }
    }
}


