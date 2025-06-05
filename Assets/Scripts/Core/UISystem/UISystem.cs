using System;
using System.Collections.Generic;
using UniRx;

namespace Core.UI
{
    public sealed class UISystem : IUISystem
    {
        private readonly Dictionary<string, IUIElement> m_UIElements = new();

        private readonly Subject<UIElementCreateRequest> m_OnUIElementCreateRequest = new();
        private readonly Subject<UIElementDestroyRequest> m_OnUIElementDestroyRequest = new();

        public IObservable<UIElementCreateRequest> OnUIElementCreateRequest => m_OnUIElementCreateRequest;
        public IObservable<UIElementDestroyRequest> OnUIElementDestroyRequest => m_OnUIElementDestroyRequest;

        public void ShowSimpleUI(UISystemPlaces place, IUIElement uiElement)
        {
            if (m_UIElements.TryAdd(uiElement.ElementGameObject.name, uiElement))
            {
                UIElementCreateRequest createRequest = new UIElementCreateRequest(place, uiElement);
                m_OnUIElementCreateRequest.OnNext(createRequest);
            }
        }

        public void HideSimpleUI(UISystemPlaces place, string elementId)
        {
            if (m_UIElements.Remove(elementId))
            {
                UIElementDestroyRequest destroyRequest = new UIElementDestroyRequest(elementId);
                m_OnUIElementDestroyRequest.OnNext(destroyRequest);
            }
        }

        public void ShowPopup(IPopup popup)
        {
            if (m_UIElements.TryAdd(popup.ElementGameObject.name, popup))
            {
                UIElementCreateRequest createRequest = new UIElementCreateRequest(UISystemPlaces.SafeArea, popup);
                m_OnUIElementCreateRequest.OnNext(createRequest);
            }
        }

        public void HidePopup(string popupId)
        {
            if (m_UIElements.Remove(popupId))
            {
                UIElementDestroyRequest destroyRequest = new UIElementDestroyRequest(popupId);
                m_OnUIElementDestroyRequest.OnNext(destroyRequest);
            }
        }
    }
}