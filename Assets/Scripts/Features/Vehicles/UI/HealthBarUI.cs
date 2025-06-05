using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Vehicles.View
{
    public sealed class HealthBarUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_Root;

        [SerializeField]
        private Image m_FillImage;

        public void Bind(IReadOnlyReactiveProperty<int> currentHp, int maxHp)
        {
            currentHp.Subscribe(hp =>
            {
                float percent = Mathf.Clamp01((float)hp / maxHp);
                m_FillImage.fillAmount = percent;
            }).AddTo(this);
        }
    }
}