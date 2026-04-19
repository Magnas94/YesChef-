using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Order
{
    public List<FoodType> RequiredItems = new List<FoodType>();
    public float StartTime;

    public bool IsCompleted(List<IngredientInstance> submitted)
    {
        List<FoodType> temp = new List<FoodType>(RequiredItems);

        foreach (var item in submitted)
        {
            if (!item.IsProcessed && item.FoodType.RequiresProcessing)
                return false;

            if (temp.Contains(item.FoodType))
                temp.Remove(item.FoodType);
        }

        return temp.Count == 0;
    }

    public int CalculateScore(float currentTime)
    {
        int baseScore = 0;

        foreach (var item in RequiredItems)
            baseScore += item.ScoreValue;

        int timePenalty = Mathf.FloorToInt(currentTime - StartTime);

        return baseScore - timePenalty;
    }
}