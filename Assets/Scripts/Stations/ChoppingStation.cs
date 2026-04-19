using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public class ChoppingStation : BaseStation, IInteractable
{
    public float m_ChopTime = 2f;
    private bool m_IsBusy;

    [SerializeField] Transform m_ItemPlacement;
    [SerializeField] IngredientInstance m_ProducedItem;

    public override void Interact(PlayerController player)
    {
        if (player.CurrentItem == null && m_ProducedItem == null) return;

        else if (player.CurrentItem != null && m_ProducedItem == null)
        {
            if (CanProcess(player.CurrentItem) && !m_IsBusy)
                StartCoroutine(Process(player));
        }
        else if (player.CurrentItem == null && m_ProducedItem != null) 
        {
            player.GiveItem(m_ProducedItem);
            m_ProducedItem = null;
        }
    }

    public override void OnInteractionLeft(PlayerController player)
    {

    }

    public bool CanProcess(IngredientInstance ingredient)
    {
        return ingredient.FoodType.RequiredProcess == ProcessType.Chop
               && !ingredient.IsProcessed;
    }

    public void StartProcessing(IngredientInstance ingredient) { }

    private IEnumerator Process(PlayerController player)
    {
        m_IsBusy = true;
        var item = player.TakeItem();
        PlaceItemOnStation(item);

        yield return new WaitForSeconds(m_ChopTime);

        m_ProducedItem = SpawnProcessedItem(item.FoodType.ProcessedPrefab);
        m_ProducedItem.Init(item.FoodType, true);
        PlaceItemOnStation(m_ProducedItem);
        Destroy(item.gameObject);

        m_IsBusy = false;
    }



    void PlaceItemOnStation(IngredientInstance a_Item) 
    {
        a_Item.transform.parent = transform;
        a_Item.transform.position = m_ItemPlacement.position;
        a_Item.transform.localEulerAngles = new Vector3(90f, 0, 0);
        a_Item.transform.localScale = m_ItemPlacement.localScale * 0.5f;
    }

    IngredientInstance SpawnProcessedItem(GameObject a_ProcessedFood) 
    {
        var l_Item = Instantiate(a_ProcessedFood, transform);
        return l_Item.GetComponent<IngredientInstance>();
    }
}
