using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Enemies.View
{
    public sealed class HealthBarUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_Root;

        [SerializeField]
        private Image m_FillImage;

        [SerializeField]
        private Canvas m_Canvas;

        private void Start()
        {
            m_Canvas.worldCamera = Camera.main;
        }

        public void Bind(IReadOnlyReactiveProperty<int> currentHp, int maxHp)
        {
            currentHp.Subscribe(hp =>
            {
                float percent = Mathf.Clamp01((float)hp / maxHp);
                m_FillImage.fillAmount = percent;
                m_Root.SetActive(percent < 1f);
            }).AddTo(this);
        }
    }
}