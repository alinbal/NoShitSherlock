using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static int LevelIndex = 0;
    [SerializeField]
    private float _playerSpeed;
    [SerializeField]
    private UISprite _uiSprite;
    [SerializeField]
    private UISprite _uiSpriteFace;
    [SerializeField]
    private GameObject _shitBar;
    [SerializeField]
    public GameObject shitFountain;
    [SerializeField]
    List<string> startLevels = new List<string>();
    [SerializeField]
    List<string> randomizeLevels = new List<string>();
    private static List<string> levels = new List<string>();
    public static int casesSolved = 0;
    private int _playerFace = 1;
    private Player _player;
    private static GameObject gameControllerInstance;
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (levels.Count == 0)
        {
            levels.Clear();
            levels.AddRange(startLevels);
            randomizeLevels.Shuffle();
            levels.AddRange(randomizeLevels);
        }

        _player = FindObjectOfType<Player>();
        if (_player != null)
        {
            _playerFace = 1;
            _player.shitFountain = shitFountain;
            _uiSpriteFace.spriteName = "face" + _playerFace;
            _shitBar.SetActive(true);
            _uiSprite.fillAmount = 1 - (((_player._lives * 100f) / _player._maxlives) / 100);
            _player.onHitWall += () =>
            {
                _playerFace++;
                _uiSprite.fillAmount = 1 - (((_player._lives * 100f) / _player._maxlives) / 100);
                _uiSpriteFace.spriteName = "face" + _playerFace;
            };
        }
        else
        {
            _shitBar.SetActive(false);
        }
    }

    private void CleanScene(GameObject target)
    {
        if (target != null)
        {
            var gameControllers = FindObjectsOfType<GameController>();
            //Debug.Log("clean" + gameControllers.Count());
            foreach (var gameController in gameControllers)
            {
                if (!gameController.gameObject.Equals(target))
                {
                    Destroy(gameController.gameObject);
                }
            }
        }

    }

    void OnLevelWasLoaded(int level)
    {
        if (levels.Count == 0)
        {
            levels.Clear();
            levels.AddRange(startLevels);
            randomizeLevels.Shuffle();
            levels.AddRange(randomizeLevels);
        }

        if (gameControllerInstance == null)
        {
            gameControllerInstance = gameObject;
        }

        CleanScene(gameControllerInstance);

        _player = FindObjectOfType<Player>();
        if (_player != null)
        {
            _playerFace = 1;
            _player.shitFountain = shitFountain;
            _uiSpriteFace.spriteName = "face" + _playerFace;
            _shitBar.SetActive(true);
            _uiSprite.fillAmount = 1 - (((_player._lives * 100f) / _player._maxlives) / 100);
            _player.onHitWall += () =>
            {
                if (_playerFace < 3)
                {
                    _playerFace++;
                }

                _uiSprite.fillAmount = 1 - (((_player._lives * 100f) / _player._maxlives) / 100);
                _uiSpriteFace.spriteName = "face" + _playerFace;
            };
        }
        else
        {
            _shitBar.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void LevelWin()
    {
        if (Application.levelCount - 1 == LevelIndex)
        {
            LevelIndex = 0;
            LevelIndex++;
            Application.LoadLevel(levels[LevelIndex]);
            Debug.Log("Load level " + levels[LevelIndex]);
        }
        else
        {
            Debug.Log("Load level " + levels[LevelIndex]);
            Application.LoadLevel(levels[LevelIndex]);
        }

        LevelIndex++;

        if (LevelIndex >= 1)
        {
            //Debug.Log("Case solved" + casesSolved + "# lvlIndex:" + LevelIndex);
            casesSolved++;
        }

    }

    public static void RestartLevel()
    {
        LevelIndex = 0;
        casesSolved = 0;
        levels.Clear();
        Application.LoadLevel("StartGame");
    }

    public static void LevelLost()
    {
        Application.LoadLevel("GameOver");
    }
}

public static class ExtensionMethods
{
    public static void Shuffle<T>(this IList<T> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
