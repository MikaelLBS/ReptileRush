using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapColoring : MonoBehaviour
{
    [SerializeField] Tilemap[] foregound;
    [SerializeField] Tilemap backgound;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var tilemap in foregound)
            tilemap.color -= new Color(0, 0.1f, 0.1f,0)*GameData.difficultyMultiplayer;

        backgound.color -= new Color(0, 0.1f, 0.1f,0) * GameData.difficultyMultiplayer;
    }
}
