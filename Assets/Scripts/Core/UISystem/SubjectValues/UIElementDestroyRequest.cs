namespace Core.UI
{
    public readonly struct UIElementDestroyRequest
    {
        public readonly UISystemPlaces Place;
        public readonly string ElementName;

        public UIElementDestroyRequest(UISystemPlaces place, string elementName)
        {
            Place = place;
            ElementName = elementName;
        }
    }
}