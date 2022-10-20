public class LifeItem : Item
{
    public float m_Life;

    public override void Pick(FPPlayerController Player)
    {
        if(Player.GetLife()<1.0f)
        {
            Player.AddLife(m_Life);
            gameObject.SetActive(false);
        }
    }
}
