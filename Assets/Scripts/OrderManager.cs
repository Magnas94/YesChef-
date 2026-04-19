using UnityEngine;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    public FoodType[] AllFoods;

    public Order GenerateOrder()
    {
        Order order = new Order();
        int count = Random.value > 0.5f ? 2 : 3;

        for (int i = 0; i < count; i++)
        {
            order.RequiredItems.Add(AllFoods[Random.Range(0, AllFoods.Length)]);
        }

        order.StartTime = Time.time;
        return order;
    }
}