using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        // Exemple : charger la scène "Scene_0_0" en mode ADDITIVE
        SceneManager.LoadSceneAsync("Scene_0_0", LoadSceneMode.Additive);
    }
}
