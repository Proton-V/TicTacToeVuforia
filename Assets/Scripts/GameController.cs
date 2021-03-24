using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CubeController;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; set; }

    [HideInInspector]
    public Player PlayerNow
    {
        get
        {
            return _players[_playerNowID];
        }
    }

    private GameManager _gm;
    private CanvasManager _cm;
    private List<Player> _players = new List<Player>();
    private int _playerNowID = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        _gm = GameManager.Instance;
        _cm = _gm.CanvasManager;
        _players.AddRange(_gm.Players);

        SetStartCanvas();
    }

    public void StartGame()
    {
        SpawnGameField();
    }
    private void CreateRestartGameButton()
    {
        _cm.PlayButton.gameObject.SetActive(true);

        _cm.PlayButton.onClick.RemoveAllListeners();
        _cm.PlayButton.onClick.AddListener(() => {
            _cm.TextCenter.enabled = false;
            _cm.PlayButton.gameObject.SetActive(false);
            _gm.Players.ForEach((player) => player.DictionaryTableVariants = new Dictionary<string, int>());
            _gm.Cubes.ForEach((cube) => {
                cube.Reset();
            });
        });
    }
    private void SetStartCanvas()
    {
        _cm.PlayButton.onClick.AddListener(() => { 
            StartGame();
            _cm.PlayButton.gameObject.SetActive(false);
        });
        _cm.TextCenter.enabled = false;
    }
    private void SetCanvasText(string text)
    {
        _cm.TextCenter.enabled = true;
        _cm.TextCenter.text = text;
    }

    public void SpawnGameField()
    {
        int countInLine = (int)(_gm.GameFieldSize.x);
        float offset = _gm.CubePrefab.transform.localScale.x + _gm.PrefabOffset;
        int numLine = 0, numColumn = 0;
        for(int x = -countInLine / 2; x <= countInLine / 2; x++)
        {
            numLine = 1;
            numColumn++;

            for (int z = -countInLine / 2; z <= countInLine / 2; z++)
            {
                CubeController newObj = Instantiate(_gm.CubePrefab, _gm.GameField.transform).GetComponent<CubeController>();
                newObj.transform.localPosition = new Vector3(x * offset, 0, z * offset);
                newObj.NumberLine = numLine;
                newObj.NumberColumn = numColumn;

                numLine++;
                _gm.Cubes.Add(newObj);
            }
        }
    }

    public void TryMove()
    {
        CheckWin();
        _playerNowID++;
        if(_playerNowID > _players.Count - 1) _playerNowID = 0;

        if (PlayerNow.IsBot)
            _gm.Cubes.First((cube) => cube.IsActive).PlayerMove();
    }
    private void CheckWin()
    {
        int sizeField = (int)GameManager.Instance.GameFieldSize.x;
        bool result1 = true;
        bool result2 = true;
        Dictionary<int, int> DiagonalLineColumn1 = new Dictionary<int, int>(), DiagonalLineColumn2 = new Dictionary<int, int>(); 
        for (int i = 0; i < sizeField/2; i++)
        {
            DiagonalLineColumn1[sizeField - i] = 1 + i;
            DiagonalLineColumn1[1 + i] = sizeField - i;

            DiagonalLineColumn2[sizeField - i] = sizeField - i;
            DiagonalLineColumn2[1 + i] = 1 + i;
        }
        foreach (KeyValuePair<int, int> i in DiagonalLineColumn1)
        {
            string key = Player._key(TableVariants.Diagonal, i.Key, i.Value);
            result1 = result1 && PlayerNow.DictionaryTableVariants.ContainsKey(key);
        }
        foreach (KeyValuePair<int, int> i in DiagonalLineColumn2)
        {
            string key = Player._key(TableVariants.Diagonal, i.Key, i.Value);
            result2 = result2 && PlayerNow.DictionaryTableVariants.ContainsKey(key);
        }

        KeyValuePair<int,int> DiagonalLineCenter = new KeyValuePair<int,int>(sizeField / 2 + 1, sizeField / 2 + 1);
        bool CenterExists = PlayerNow.DictionaryTableVariants.ContainsKey(
            Player._key(TableVariants.Diagonal, DiagonalLineCenter.Key, DiagonalLineCenter.Value));

        result1 = result1 && CenterExists;
        result2 = result2 && CenterExists;

        if (PlayerNow.DictionaryTableVariants.ContainsValue((int)_gm.GameFieldSize.x)
            || result1
            || result2)
        {
            //Win
            Debug.LogError("WIIIN");
            SetCanvasText($"{PlayerNow.PlayerVariant} \nWin!");
            CreateRestartGameButton();
        }
        else if(_gm.Cubes.Where((cube)=>!cube.IsActive).Count() >= sizeField * sizeField)
        {
            //Game Over
            Debug.LogError("Game Over");
            SetCanvasText($"Game Over!");
            CreateRestartGameButton();
        }
    }
}
