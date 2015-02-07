using UnityEngine;

public class GameController : MonoBehaviour
{
    public static int LevelIndex = 0;
    [SerializeField] private float _playerSpeed;
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
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
