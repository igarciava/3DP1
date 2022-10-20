using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Controller : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            GameController.GetGameController().SetPLayerLife(0.3f);
            
        }SceneManager.LoadSceneAsync("Level 2 Scene");
    }
}
