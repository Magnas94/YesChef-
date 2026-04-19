using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Refrigerator : BaseStation
{
    public FoodType[] m_AvailableFoods;
    public Transform m_SpawnPoint;

    [SerializeField] GameObject m_UI;
    [SerializeField] UI_FridgeIngredient m_FridgeIngredientUIPrefab;

    [SerializeField] RectTransform m_UIParent; 
    
    bool m_CreatedUI = false;
    PlayerController m_Player;

    public override void Interact(PlayerController a_Player)
    {
        ShowFridgeUI(true, a_Player);

        //if (player.CurrentItem != null) return;

        //var food = m_AvailableFoods[Random.Range(0, m_AvailableFoods.Length)];

        //GameObject prefab = Instantiate(food.RawPrefab, m_SpawnPoint.position, Quaternion.identity);
        //var ingredient = prefab.GetComponent<IngredientInstance>();
        //ingredient.Init(food);

        //player.GiveItem(ingredient);
    }

    public override void OnInteractionLeft(PlayerController a_Player)
    {
        ShowFridgeUI(false, a_Player);
    }

    public void ShowFridgeUI(bool a_Show, PlayerController a_Player) 
    {
        if(!m_CreatedUI)
            CreateFridgeUI(a_Player);
        m_UI.SetActive(a_Show);
    }

    void CreateFridgeUI(PlayerController a_Player) 
    {
        if(m_CreatedUI) return;
        m_CreatedUI = true;
        foreach (var l_V in m_AvailableFoods) 
        {
            var l_I = Instantiate(m_FridgeIngredientUIPrefab, m_UIParent);
            l_I.Intialize(l_V, a_Player);
        }
    }
}