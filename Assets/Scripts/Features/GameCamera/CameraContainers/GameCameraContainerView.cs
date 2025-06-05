using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.GameCamera.View
{
    public sealed class GameCameraContainerView : MonoBehaviour
    {
        [SerializeField]
        private GameCameraView m_CameraView;

        [Header("Configs Start")]
        [SerializeField]
        private Vector3 m_StartPosition;

        [SerializeField]
        private Vector3 m_StartRotation;

        [SerializeField]
        private float m_FlyTime;

        [Header("Configs Follow")]
        [SerializeField]
        private Vector3 m_FollowPosition;

        [SerializeField]
        private Vector3 m_FollowRotation;

        private bool m_IsBusy;
        private Vector3 m_Shift;
        private GameObject m_Target;

        public void SetNewTarget(GameObject newTarget)
        {
            m_Target = newTarget;
        }

        public void PrepareToStart()
        {
            m_CameraView.transform.position = m_Target.transform.position + m_StartPosition;
            m_CameraView.transform.rotation = Quaternion.Euler(m_StartRotation);
        }

        public async UniTask StartFlySequence()
        {
            await PlayFlyToFollowPosition();
            Follow().Forget();
        }

        private async UniTask PlayFlyToFollowPosition()
        {
            Vector3 finalFollowPosition = Vector3.zero;
            Vector3 startPos = m_CameraView.transform.position;
            Quaternion startRot = m_CameraView.transform.rotation;
            Quaternion targetRot = Quaternion.Euler(m_FollowRotation);

            float elapsed = 0f;

            while (elapsed < m_FlyTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / m_FlyTime);

                finalFollowPosition = new Vector3(0, 0, m_Target.transform.position.z) + m_FollowPosition;
                m_CameraView.transform.position = Vector3.Lerp(startPos, finalFollowPosition, t);
                m_CameraView.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

                await UniTask.NextFrame();
            }

            m_CameraView.transform.position = finalFollowPosition;
            m_CameraView.transform.rotation = targetRot;
        }

        private async UniTask Follow()
        {
            m_IsBusy = true;

            while (m_IsBusy)
            {
                if (m_CameraView && m_Target)
                {
                    float oldY = m_CameraView.transform.position.y;
                    float oldX = m_CameraView.transform.position.x;
                    float newZ = m_Target.transform.position.z + m_FollowPosition.z;

                    Vector3 newPosition = new Vector3(oldX, oldY, newZ);
                    m_CameraView.transform.position = newPosition;
                }
                else
                {
                    StopFollow();
                    break;
                }

                await UniTask.NextFrame();
            }
        }

        public void StopFollow()
        {
            m_IsBusy = false;
        }
    }
}