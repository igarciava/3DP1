using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Controller : MonoBehaviour
{
    private void Start()
    {
        GameController.GetGameController().SetPLayerLife(1.0f);
        GameController.GetGameController().SetPLayerShield(1.0f);
        GameController.GetGameController().SetDroneDamage(0.2f);
    }
}
