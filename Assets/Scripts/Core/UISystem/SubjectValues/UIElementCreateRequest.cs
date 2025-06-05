namespace Core.UI
{
    public readonly struct UIElementCreateRequest
    {
        public readonly UISystemPlaces Place;
        public readonly IUIElement Element;

        public UIElementCreateRequest(UISystemPlaces place, IUIElement element)
        {
            Place = place;
            Element = element;
        }
    }
}