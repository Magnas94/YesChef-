using UnityEngine;
public class IngredientInstance : MonoBehaviour
{
    public FoodType FoodType { get; private set; }
    public bool IsProcessed { get; private set; }

    public void Init(FoodType type, bool processed = false)
    {
        FoodType = type;
        IsProcessed = processed;
    }

    public void MarkProcessed()
    {
        IsProcessed = true;
    }
}