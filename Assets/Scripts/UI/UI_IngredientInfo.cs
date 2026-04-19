using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UI_IngredientInfo : MonoBehaviour
{
    [SerializeField] Image m_OrderImage;
    [SerializeField] TextMeshProUGUI m_OrderText;
    [SerializeField] GameObject m_CompletedUI;

    [SerializeField]FoodType m_FoodType;
    public FoodType FoodType => m_FoodType;

    [SerializeField] int m_Amount = 0;

    public void ShowIngredientInfo(FoodType a_Food, int a_Amount) 
    {
        m_OrderImage.sprite = a_Food.ProcessedIcon != null ? a_Food.ProcessedIcon : a_Food.RawIcon;
        m_Amount = a_Amount;
        m_OrderText.text = "x" + a_Amount;
        m_CompletedUI.SetActive(false);
        m_FoodType = a_Food;
    }

    public void CollectIngredient() 
    {
        m_Amount -= 1;
        m_OrderText.text = "x" + m_Amount;
        if (m_Amount == 0)
            CompleteIngredient();
    }

    void CompleteIngredient() 
    {
        m_OrderText.gameObject.SetActive(false);
        m_CompletedUI.SetActive(true);
    }

    public void IncreaseAmount() 
    {
        m_Amount += 1;
        m_OrderText.text = "x" + m_Amount;
    }
}
