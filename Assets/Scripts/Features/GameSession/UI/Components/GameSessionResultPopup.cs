using Core.UI;
using TMPro;
using UnityEngine;
using Zenject;

namespace Features.GameSession.UI
{
    public class GameSessionResultPopup : MonoBehaviour, IPopup
    {
        [SerializeField]
        private TMP_Text m_ResultText;

        private IGameSessionService m_GameSessionService;

        [Header("Configs")]
        [SerializeField]
        private string m_WinText;

        [SerializeField]
        private string m_LoseText;

        [Inject]
        private void Construct(IGameSessionService gameSessionService)
        {
            m_GameSessionService = gameSessionService;
        }

        public GameObject ElementGameObject => gameObject;

        private void Start()
        {
            m_ResultText.text = m_GameSessionService.LastGameResult ? m_WinText : m_LoseText;
        }
    }
}