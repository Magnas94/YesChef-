using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class StoveStation : BaseStation
{
    [System.Serializable]
    public class StoveSlot
    {
        public Transform Point;
        public IngredientInstance CurrentItem;
    }

    public List<StoveSlot> Slots = new List<StoveSlot>();
    public float cookTime = 6f;

    public override void Interact(PlayerController player)
    {
        if (player.CurrentItem == null && !IsThereAnyProcessedItem())
            return;
        else if (player.CurrentItem != null && IsThereAnySlotAvailable())
        {

            foreach (var slot in Slots)
            {
                if (slot.CurrentItem == null && CanProcess(player.CurrentItem))
                {
                    var item = player.TakeItem();
                    slot.CurrentItem = item;
                    PlaceItemAtSlot(item, slot);
                    StartCoroutine(Cook(slot));
                    return;
                }
            }
        }
        else if (player.CurrentItem == null && IsThereAnyProcessedItem()) 
        {
            foreach (var slot in Slots) 
            {
                if (slot.CurrentItem != null && slot.CurrentItem.IsProcessed) 
                {
                    player.GiveItem(slot.CurrentItem);
                    slot.CurrentItem = null;
                    break;
                }
            }
        }
    }

    public override void OnInteractionLeft(PlayerController player)
    {
    }

    private bool CanProcess(IngredientInstance ingredient)
    {
        return ingredient.FoodType.RequiredProcess == ProcessType.Cook
               && !ingredient.IsProcessed;
    }

    private IEnumerator Cook(StoveSlot slot)
    {
        yield return new WaitForSeconds(cookTime);
        var l_NewItem = SpawnProcessedItem(slot.CurrentItem.FoodType.ProcessedPrefab);
        l_NewItem.Init(slot.CurrentItem.FoodType, true);
        Destroy(slot.CurrentItem.gameObject);
        slot.CurrentItem = l_NewItem;
        PlaceItemAtSlot(slot.CurrentItem, slot);
    }

    void PlaceItemAtSlot(IngredientInstance a_Item, StoveSlot a_Slot) 
    {
        a_Item.transform.parent = transform;
        a_Item.transform.position = a_Slot.Point.position;
        a_Item.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
        a_Item.transform.localScale = a_Slot.Point.localScale * 0.5f;
    }

    IngredientInstance SpawnProcessedItem(GameObject a_Prefab) 
    {
        var l_Item = Instantiate(a_Prefab, transform);
        return l_Item.GetComponent<IngredientInstance>();
    }

    public bool IsThereAnyProcessedItem() 
    {
        foreach (var l_Slot in Slots)
        {
            if (l_Slot.CurrentItem != null && l_Slot.CurrentItem.IsProcessed)
                return true;
        }
        return false;
    }

    public bool IsThereAnySlotAvailable() 
    {
        foreach (var l_Slot in Slots)
        {
            if (l_Slot.CurrentItem == null)
                return true;
        }
        return false;
    }
}