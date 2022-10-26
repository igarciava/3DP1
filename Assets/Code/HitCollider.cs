using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public float m_LifeDealt;

    public DroneEnemy m_DroneEnemy;
    public void Hit()
    {
        m_DroneEnemy.Hit(m_LifeDealt);
    }
}
