using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FridgeIngredient : MonoBehaviour
{
    [SerializeField] Image m_IngredientImage;
    [SerializeField] Button m_AddButton;

    FoodType m_FoodType;
    PlayerController m_PC;

    public void Intialize(FoodType a_FT, PlayerController a_PC) 
    {
        m_PC = a_PC;
        m_FoodType = a_FT;
        m_IngredientImage.sprite = m_FoodType.RawIcon;
    }

    public void AddIngredientToPlayer() 
    {
        GameObject l_Prefab = Instantiate(m_FoodType.RawPrefab, Vector3.zero, Quaternion.identity);
        var l_Ingredient = l_Prefab.GetComponent<IngredientInstance>();
        l_Ingredient.Init(m_FoodType);

        m_PC.GiveItem(l_Ingredient);
    }
}
