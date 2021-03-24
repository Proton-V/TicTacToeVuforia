using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    //UI
    public CanvasManager CanvasManager;

    public GameObject GameField;
    public Vector2 GameFieldSize;

    public GameObject CubePrefab;
    [Range(-1f,1f)]
    public float PrefabOffset = 0.1f;

    public List<Player> Players;
    [HideInInspector]
    public List<CubeController> Cubes;

    public void OnValidate()
    {
        float valueX = GameFieldSize.x%2 == 0? GameFieldSize.x-1: GameFieldSize.x;
        int value = (int)Mathf.Clamp(valueX, 2, 5);
        GameFieldSize = new Vector2(value, value);
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
