using UnityEngine;

public abstract class BaseStation : MonoBehaviour, IInteractable
{
    public abstract void Interact(PlayerController player);
    public abstract void OnInteractionLeft(PlayerController player);
}