using UnityEngine;
using GameUI;

public class Item :
	CoreObject
{
    public Description Description;
    public ItemType ItemType;
    public Points Stack;
    public Recipe CraftRecipe;
    public bool IsStackable
    {
        get { return Stack.Maximum > 1 + Mathf.Epsilon; }
    }

    public bool FillFrom(Item item, int count = int.MaxValue)
    {
        if (Stack != null && item != null && item.ID == ID && item.Stack != null)
        {
            if (count == int.MaxValue)
            {
                count = (int)Stack.Maximum;
            }
            float amount = Stack.Current + item.Stack.Current;
            float surplus = amount - count;
            if (surplus > 0)
            {
                item.Stack.Current = surplus;
                Stack.Current = count;
            }
            else
            {
                Stack.Current = amount;
                item.Stack.Current = 0;
            }
            return true;
        }
        return false;
    }
}
