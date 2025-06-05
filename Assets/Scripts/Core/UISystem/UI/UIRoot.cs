using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.UI
{
    public sealed class UIRoot : MonoBehaviour, IUIRoot
    {
        [SerializeField]
        private RectTransform m_FullScreenUnderUI;

        [SerializeField]
        private RectTransform m_SafeArea;

        [SerializeField]
        private RectTransform m_FullScreenOverUI;

        private IUISystem m_UISystem;
        private DiContainer m_Container;
        private readonly CompositeDisposable m_Disposable = new();

        private readonly Dictionary<string, IUIElement> m_Elements = new();

        [Inject]
        private void Construct(IUISystem uiSystem, DiContainer container)
        {
            m_UISystem = uiSystem;
            m_Container = container;
        }

        private void Awake()
        {
            m_UISystem.OnUIElementCreateRequest.Subscribe(CreateUI).AddTo(m_Disposable);
            m_UISystem.OnUIElementDestroyRequest.Subscribe(DestroyUI).AddTo(m_Disposable);
        }

        private void CreateUI(UIElementCreateRequest request)
        {
            RectTransform parent = null;

            switch (request.Place)
            {
                case UISystemPlaces.FullScreenUnderUI:
                    parent = m_FullScreenUnderUI;
                    break;
                case UISystemPlaces.SafeArea:
                    parent = m_SafeArea;
                    break;
                case UISystemPlaces.FullScreenOverUI:
                    parent = m_FullScreenOverUI;
                    break;
                case UISystemPlaces.None:
                    Debug.LogError("[UIRoot] request.Place cannot be None!!!");
                    return;
            }

            if (parent)
            {
                GameObject go = m_Container.InstantiatePrefab(request.Element.ElementGameObject, parent);
                m_Elements.Add(request.Element.ElementGameObject.name, go.GetComponent<IUIElement>());
            }
        }

        private void DestroyUI(UIElementDestroyRequest request)
        {
            if (m_Elements.TryGetValue(request.ElementName, out IUIElement element))
            {
                Destroy(element.ElementGameObject);
                m_Elements.Remove(request.ElementName);
            }
        }

        private void OnDestroy()
        {
            m_Disposable.Dispose();
        }
    }
}