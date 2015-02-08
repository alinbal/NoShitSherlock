using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static int LevelIndex = 0;
    [SerializeField]
    private float _playerSpeed;
    [SerializeField] private UISprite _uiSprite;
    [SerializeField] private UISprite _uiSpriteFace;
    [SerializeField] private GameObject _shitBar;
    [SerializeField] public GameObject shitFountain;
    public static int casesSolved = 0;
    private int _playerFace = 1;
    private Player _player;
    private static GameObject gameControllerInstance;
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void CleanScene(GameObject target)
    {
        if (target!=null)
        {
            var gameControllers = FindObjectsOfType<GameController>();
            Debug.Log("clean" + gameControllers.Count());
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
        if (gameControllerInstance==null)
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

    // Update is called once per frame
    void Update()
    {

    }

    public static void LevelWin()
    {
        LevelIndex++;

        if (Application.levelCount - 1 == LevelIndex)
        {
            LevelIndex = 0;
            LevelIndex++;
            Application.LoadLevel(LevelIndex);
        }
        else
        {
            Application.LoadLevel(LevelIndex);
        }

        if (LevelIndex>=1)
        {
            Debug.Log("Case solved" + casesSolved + "# lvlIndex:" + LevelIndex);
            casesSolved++;
        }
        
    }

    public static void RestartLevel()
    {
        LevelIndex = 0;
        casesSolved = 0;
        
        Application.LoadLevel("StartGame");
    }

    public static void LevelLost()
    {
        Application.LoadLevel("GameOver");
    }
}
