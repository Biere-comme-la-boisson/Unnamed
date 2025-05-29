using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneStreamer : MonoBehaviour
{
    public Transform player;
    public float sceneSize = 100f;
    public float loadRadius = 1;
    public float loadCooldown = 0.5f; // Temps minimal entre deux chargements

    private Vector2Int currentSceneCoord;
    private HashSet<string> loadedScenes = new HashSet<string>();
    private HashSet<string> loadingScenes = new HashSet<string>(); // Scènes en cours de chargement

    private float lastLoadTime = -Mathf.Infinity;

    void Start()
    {
        currentSceneCoord = GetSceneCoordFromPosition(player.position);
        LoadSurroundingScenes(currentSceneCoord);
    }

    void Update()
    {
        if (Time.time - lastLoadTime < loadCooldown)
            return; // Pas encore le moment

        Vector2Int newCoord = GetSceneCoordFromPosition(player.position);

        if (newCoord != currentSceneCoord)
        {
            currentSceneCoord = newCoord;
            LoadSurroundingScenes(currentSceneCoord);
            lastLoadTime = Time.time;
        }
    }

    Vector2Int GetSceneCoord(Vector3 pos)
    {
        float x = pos.x / sceneSize;
        float z = pos.z / sceneSize;
        Debug.Log($"[SceneStreamer] Raw pos: x={pos.x}, z={pos.z}, x/size={x}, z/size={z}");
        return new Vector2Int(
            Mathf.RoundToInt(pos.x / sceneSize),
            Mathf.RoundToInt(pos.z / sceneSize)
        );

    }


    Vector2Int GetSceneCoordFromPosition(Vector3 position)
    {
        return new Vector2Int(
            Mathf.FloorToInt(position.x / sceneSize),
            Mathf.FloorToInt(position.z / sceneSize)
        );
    }

    void LoadSurroundingScenes(Vector2Int center)
    {
        HashSet<string> scenesToKeep = new HashSet<string>();
        HashSet<string> sceneToLoad = new HashSet<string>();
        HashSet<string> sceneToUnload = new HashSet<string>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int coord = new Vector2Int(center.x + x, center.y + y);
                string sceneName = $"Scene_{coord.x}_{coord.y}";
                if(sceneName != $"Scene_{currentSceneCoord.x}_{currentSceneCoord.y}") scenesToKeep.Add(sceneName);

                if (sceneName != $"Scene_{currentSceneCoord.x}_{currentSceneCoord.y}" && !loadedScenes.Contains(sceneName))
                {
                    sceneToLoad.Add(sceneName);
                }
            }
        }
        
        foreach(var scene in sceneToLoad)
        {
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            loadedScenes.Add(scene);
        }

        // Déchargement
        var toUnload = new List<string>();
        foreach (var scene in loadedScenes)
        {
            if (!scenesToKeep.Contains(scene))
            {
                Debug.Log($"[SceneStreamer] Unloading: {scene}");
                SceneManager.UnloadSceneAsync(scene);
                toUnload.Add(scene);
            }
        }

        foreach (var scene in toUnload)
        {
            loadedScenes.Remove(scene);
        }
    }
}
