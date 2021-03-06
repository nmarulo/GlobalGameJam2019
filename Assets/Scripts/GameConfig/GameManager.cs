﻿using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float TimeToAdd;
    public GameObject WolfPrefab;
    public GameObject PlayerPrefab;
    public Sprite[] PlayerSprites;
    public MusicManager MusicManagerScript;

    private GameObject[] _walls;

    private readonly Dictionary<Wolf, GameObject> _enemies = new Dictionary<Wolf, GameObject>();
    private readonly List<GameObject> _freeWalls = new List<GameObject>();
    private float _counter = 0;
    private int _wolfCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (TimeToAdd == 0f) TimeToAdd = 5;
        _walls = GameObject.FindGameObjectsWithTag(GameConfiguration.WALL);
        _freeWalls.AddRange(_walls);

        // Player spawner
        var nPlayers = PlayerPrefs.GetInt(GameConfiguration.PLAYERS);
        if (nPlayers == 0) nPlayers = 1;
        var floors = GameObject.FindGameObjectsWithTag(GameConfiguration.FLOOR);
        for (int i = 0; i < nPlayers; i++)
        {
            var player = Instantiate(PlayerPrefab, floors[Random.Range(0, floors.Length)].transform.position, Quaternion.identity);
            player.GetComponentInChildren<SpriteRenderer>().sprite = PlayerSprites[i];
            player.GetComponentInChildren<JoystickController>().numberPlayer = i + 1;
        }

        System.Func<Material> wallMaterial;
        if (nPlayers == 1)
        {
            var difficulty = (Difficulty)PlayerPrefs.GetInt(GameConfiguration.DIFFICULTY);
            switch (difficulty)
            {
                case Difficulty.Easy:
                    wallMaterial = () => Material.Stone;
                    break;
                case Difficulty.Hard:
                    wallMaterial = () => Material.Wheat;
                    break;
                case Difficulty.Medium:
                default:
                    wallMaterial = () => Material.Wood;
                    break;
            }
        }
        else
        {
            wallMaterial = () =>
            {
                switch (Random.Range(0, 10))
                {
                    case int n when (n >= 0 && n < 5):
                        return Material.Wheat;
                    case int n when (n >= 8):
                        return Material.Stone;
                    case int n when (n >= 5 && n < 8):
                    default:
                        return Material.Wood;
                }
            };
        }

        foreach (var wall in _walls)
        {
            var controller = wall.GetComponent<WallController>();
            controller.wallMaterial = wallMaterial();
            controller.InitWall();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_counter <= 0)
        {
            var wolf = Instantiate(WolfPrefab).GetComponentInChildren<Wolf>();
            wolf.Manager = this;
            _enemies.Add(wolf, null);
            _counter += TimeToAdd;
        }
        else
        {
            _counter -= Time.deltaTime;
        }
        MusicManagerScript.CheckWolfs(_wolfCounter);
    }

    public GameObject TryToAppear(Wolf w)
    {
        if (_freeWalls.Count == 0) return null;
        _wolfCounter++;
        var i = Mathf.RoundToInt(Random.Range(0, _freeWalls.Count));
        var wallAppear = _freeWalls[i];
        _freeWalls.RemoveAt(i);
        _enemies[w] = wallAppear;
        return wallAppear;
    }

    public void Disappear(Wolf w)
    {
        var wall = _enemies[w];
        if (wall == null) return;

        _freeWalls.Add(wall);
        _enemies[w] = null;
    }
}
