public class AmmoItem : Item
{
    int AmmoGiven = 50;
    public override void Pick(FPPlayerController Player)
    {
        Player.AddAmmo(AmmoGiven);
        Destroy(gameObject);
    }
}
