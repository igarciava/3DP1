public class ShieldItem : Item
{
    public float ShieldGiven = 1.0f;
    public override void Pick(FPPlayerController Player)
    {
        if (Player.GetShield() < 1.0f)
        {
            Player.AddShield(ShieldGiven);
            Destroy(gameObject);
        }
    }
}
