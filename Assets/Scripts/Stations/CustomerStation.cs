using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEditorInternal;
using TMPro;

/// <summary>
/// Stations where customers will come with order and player has to deliver
/// the order in order to score.
/// </summary>
public class CustomerStation : BaseStation
{
    [SerializeField] OrderManager m_OrderManager;
    public Order m_CurrentOrder;
    private List<IngredientInstance> submitted = new List<IngredientInstance>();
    [SerializeField] Customer m_CurrentCustomer;
    public Customer CurrentCustomer => m_CurrentCustomer;

    [SerializeField] UI_IngredientInfo m_IngredientPrefab; 
    [SerializeField] GameObject m_OrderUI;
    [SerializeField] RectTransform m_OrderParent;

    [SerializeField] List<UI_IngredientInfo> m_SpawnedIngredients;
    [SerializeField] TextMeshPro m_TimerText;

    [SerializeField] ScoreIndicator m_ScoreIndicatorPrefab;

    public void SetCustomer(Customer a_C) => m_CurrentCustomer = a_C;

    GameManager m_GameManager;

    private void Start()
    {
        m_GameManager = GameManager.instance;
    }

    public override void Interact(PlayerController a_Player)
    {
        if (a_Player.CurrentItem == null || m_CurrentOrder == null) return;

        FoodType l_PlayerItem = a_Player.CurrentItem.FoodType;

        if (!IsTheItemRequired(l_PlayerItem))
            return;

        if (!l_PlayerItem.RequiresProcessing)
            PlacePlayerItemOnStation(a_Player);

        else 
        {
            if (a_Player.CurrentItem.IsProcessed)
                PlacePlayerItemOnStation(a_Player);
            else
            {
                Debug.Log("Item not processed!");
                // SHOW UI NOTIFICATION TO PROCESS TE ITEM!
            }
        }

        if (m_CurrentOrder.IsCompleted(submitted))
            FinishCurrentOrder();
    }

    void PlacePlayerItemOnStation(PlayerController a_Player) 
    {
        var l_Item = a_Player.TakeItem();
        // Take Item and add to submitted list
        submitted.Add(l_Item);
        // Reflect in UI
        m_SpawnedIngredients[GetIndexOfFoodType(l_Item.FoodType)].CollectIngredient();
        Destroy(l_Item.gameObject);
    }

    

    /// <summary>
    /// Lets show the order timer!
    /// </summary>
    private void Update()
    {
        if(m_CurrentOrder == null) return;
        var l_Time = Time.time - m_CurrentOrder.StartTime;
        int minutes = Mathf.FloorToInt(l_Time / 60);
        int seconds = Mathf.FloorToInt(l_Time % 60);
        m_TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public override void OnInteractionLeft(PlayerController player)
    {

    }

    public void CreateNewOrder()
    {
        Debug.Log("Created order at: " + this.gameObject, gameObject);
        m_CurrentOrder = m_OrderManager.GenerateOrder();
        ShowOrderUI();
    }

    void FinishCurrentOrder() 
    {
        int score = m_CurrentOrder.CalculateScore(Time.time);
        m_GameManager.AddScore(score);
        m_GameManager.TotalOrdersServed += 1;

        submitted.Clear();
        m_CurrentOrder = null;

        // Hide UI after showing all ticks
        var l_S = Instantiate(m_ScoreIndicatorPrefab);
        l_S.transform.position = transform.position + Vector3.up;
        l_S.Initialize(score);
        Invoke(nameof(HideOrderUI), 2f);
    }

    void ShowOrderUI() 
    {
        if (m_CurrentOrder == null)
            return;
        m_OrderUI.SetActive(true);
        foreach (var item in m_CurrentOrder.RequiredItems) 
        {
            // If ingredient does not exist, add it
            if (GetIndexOfFoodType(item) == -1)
            {
                UI_IngredientInfo l_IngredientUI = Instantiate(m_IngredientPrefab, m_OrderParent);
                l_IngredientUI.ShowIngredientInfo(item, 1);
                m_SpawnedIngredients.Add(l_IngredientUI);
            }
            else 
            {
                // Increase amount if it exists
                m_SpawnedIngredients[GetIndexOfFoodType(item)].IncreaseAmount();
            }
        }
        m_TimerText.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hide the order UI
    /// </summary>
    void HideOrderUI() 
    {
        m_OrderUI.SetActive(false);
        foreach(var item in m_SpawnedIngredients)
            Destroy(item.gameObject);
        m_SpawnedIngredients.Clear();
        m_TimerText.gameObject.SetActive(false);
        m_CurrentCustomer.OnGettingOrder();
    }


    public bool IsTheItemRequired(FoodType a_Item)
    {
        foreach (var l_Item in m_CurrentOrder.RequiredItems)
        {
            if (l_Item == a_Item)
                return true;
        }
        return false;
    }


    /// <summary>
    /// We will check in ingredients list if the ingredient
    /// already exists in the list of spawned list or not.
    /// </summary>
    /// <param name="a_FT"></param>
    /// <returns></returns>
    public int GetIndexOfFoodType(FoodType a_FT) 
    {
        if(m_SpawnedIngredients.Count == 0)
            return -1;
        foreach (var l_FT in m_SpawnedIngredients)
        {
            if (l_FT.FoodType == a_FT)
                return m_SpawnedIngredients.IndexOf(l_FT);
        }
        return -1;
    }
}