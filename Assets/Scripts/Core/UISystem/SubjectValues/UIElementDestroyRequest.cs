namespace Core.UI
{
    public readonly struct UIElementDestroyRequest
    {
        public readonly string ElementName;

        public UIElementDestroyRequest(string elementName)
        {
            ElementName = elementName;
        }
    }
}