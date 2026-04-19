public class Trashbin : BaseStation
{
    public override void Interact(PlayerController player)
    {
        player.ClearItem();
    }

    public override void OnInteractionLeft(PlayerController player)
    {
        
    }
}
