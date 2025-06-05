using System;

namespace Core.UI
{
    public interface IUISystem
    {
        IObservable<UIElementCreateRequest> OnUIElementCreateRequest { get; }
        IObservable<UIElementDestroyRequest> OnUIElementDestroyRequest { get; }

        void ShowSimpleUI(UISystemPlaces place, IUIElement uiElement);
        void HideSimpleUI(UISystemPlaces place, string elementId);
        void ShowPopup(IPopup popup);
        void HidePopup(string popupId);
    }
}