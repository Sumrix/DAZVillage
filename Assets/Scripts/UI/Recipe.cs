using System;

namespace GameUI
{
    [Serializable]
    public class Recipe
    {
        public int ProducedQuantity = 1;
        public InspectorItem[] Resources;
    }
}