/// <summary>
/// Interface to be used by all objects that can be interacted
/// by the player.
/// </summary>
public interface IInteractable
{
    void Interact(PlayerController player);

    void OnInteractionLeft(PlayerController player);
}