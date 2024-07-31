using UnityEngine;

namespace YandexTanks.PlayerTank
{
    public class TankTurret : MonoBehaviour
    {
        [SerializeField] private LayerMask LayerMaskForGun;
        [Space]
        [SerializeField] private float barrelElevationSpeed;
        [SerializeField] private float barrelMaxNegativeAngle;
        [SerializeField] private float barrelMaxPositiveAngle;
        [SerializeField] private float turretAimedAngleThreshold;
        [Space]
        [SerializeField] private Transform turretBody;
        [SerializeField] private Transform barrelRotationPoint;
        [Space]
        [SerializeField] private float angleHorizontalToTarget;
        [SerializeField] private float angleVerticalToTarget;
        [SerializeField] private bool isTurretAimedCompletely;


        private TankConfig config;

        private float turretRotationSpeed;

        public void Init(TankConfig newConfig)
        {
            config = newConfig;
            SetParametersFromConfig(config);
        }

        private void SetParametersFromConfig(TankConfig config)
        {
            turretRotationSpeed = config.TurretRotationSpeed;
        }

        public void FixedUpdateRotation(Vector3 target)
        {
            RotationTurretToAim(target);
            TiltBarrels(target);

            angleHorizontalToTarget = TurretHorizontalAngleToAim(target);
            isTurretAimedCompletely = angleHorizontalToTarget < turretAimedAngleThreshold;
        }

        private void RotationTurretToAim(Vector3 target)
        {
            Vector3 turretUp = turretBody.up;
            Vector3 directionToTarget = target - turretBody.position;
            Vector3 flattenedVectorForTurret = Vector3.ProjectOnPlane(directionToTarget, turretUp);

            turretBody.rotation = Quaternion.RotateTowards(
                    Quaternion.LookRotation(turretBody.forward, turretUp),
                    Quaternion.LookRotation(flattenedVectorForTurret, turretUp),
                    turretRotationSpeed * Time.fixedDeltaTime);
        }

        private void TiltBarrels(Vector3 targetPosition)
        {
            Vector3 localTargetPos = turretBody.InverseTransformDirection(targetPosition - barrelRotationPoint.position);
            Vector3 flattenedVecForBarrels = Vector3.ProjectOnPlane(localTargetPos, Vector3.up);
            float targetElevation = Vector3.Angle(flattenedVecForBarrels, localTargetPos);

            targetElevation *= Mathf.Sign(localTargetPos.y);
            targetElevation = Mathf.Clamp(targetElevation,
                -barrelMaxNegativeAngle,
                barrelMaxPositiveAngle);

            angleVerticalToTarget = Mathf.MoveTowards(
                angleVerticalToTarget,
                targetElevation,
                barrelElevationSpeed * Time.fixedDeltaTime);

            if (Mathf.Abs(angleVerticalToTarget) > 0.05f)
            {
                barrelRotationPoint.localEulerAngles = Vector3.right * -angleVerticalToTarget;
            }
        }

        private float TurretHorizontalAngleToAim(Vector3 targetPosition)
        {
            float angle = Vector3.Angle(targetPosition - barrelRotationPoint.position, barrelRotationPoint.forward);
            return angle;
        }

        public Vector3 GetGunFacePointToCamera()
        {
            Ray ray = new Ray(barrelRotationPoint.position, barrelRotationPoint.forward);
            RaycastHit hit;

            Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMaskForGun);

            return Camera.main.WorldToScreenPoint(hit.point);
        }

        public Vector3 GetGunFacePointToWorld()
        {
            Ray ray = new Ray(barrelRotationPoint.position, barrelRotationPoint.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMaskForGun))
            {
                return hit.point;
            }
            else
            {
                return ray.GetPoint(100f);
            }
        }
    }
}


