using UnityEngine;

public class GameController : MonoBehaviour
{
    public static int LevelIndex = 0;
    [SerializeField]
    private float _playerSpeed;
    [SerializeField]
    private UISprite _uiSprite;
    private Player _player;
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnLevelWasLoaded(int level)
    {
        _player = FindObjectOfType<Player>();
        _uiSprite.fillAmount = ((_player._lives * 100f) / _player._maxlives)/100;
        _player.onHitWall += () =>
        {
            _uiSprite.fillAmount = ((_player._lives * 100f) / _player._maxlives) / 100;
        };
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void LevelWin()
    {
        LevelIndex++;

        if (Application.levelCount - 2 == LevelIndex)
        {
            LevelIndex = 0;
            Application.LoadLevel(5);
        }
        else
        {
            Application.LoadLevel(LevelIndex);
        }
    }

    public static void RestartLevel()
    {
        LevelIndex = 0;
        Application.LoadLevel("StartGame");
    }

    public static void LevelLost()
    {
        Application.LoadLevel("GameOver");
    }
}
