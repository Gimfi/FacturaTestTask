using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Tools
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class SafeArea : MonoBehaviour
    {
        private RectTransform m_Panel;
        private Rect m_LastSafeArea;
        private ScreenOrientation m_LastOrientation;

        private void Awake()
        {
            AssignValues();
            ApplySafeArea();
            LaunchSafeAreaTask();
        }

        private void AssignValues()
        {
            m_Panel = GetComponent<RectTransform>();
            m_LastSafeArea = Screen.safeArea;
            m_LastOrientation = Screen.orientation;
        }

        private void LaunchSafeAreaTask()
        {
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            WaitNextFrameAndCheck(token).Forget();
        }

        private UniTask WaitNextFrameAndCheck(CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return UniTask.CompletedTask;

            return UniTask.Delay(1000, cancellationToken: token).ContinueWith<UniTask>(() =>
            {
                if (m_LastSafeArea != Screen.safeArea || m_LastOrientation != Screen.orientation)
                {
                    ApplySafeArea();
                    m_LastSafeArea = Screen.safeArea;
                    m_LastOrientation = Screen.orientation;
                }

                return WaitNextFrameAndCheck(token);
            }).Unwrap();
        }

        private void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            m_Panel.anchorMin = anchorMin;
            m_Panel.anchorMax = anchorMax;
        }
    }
}