using UnityEngine;
using System.Collections;

public class ButtonGameOver : MonoBehaviour
{

    [SerializeField]
    private UILabel label;
    // Use this for initialization
    void Start()
    {
        if (label!=null)
        {
            label.text = Mathf.Clamp(GameController.casesSolved - 1, 0, int.MaxValue).ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameOver()
    {
        GameController.RestartLevel();
    }

    public void GameStart()
    {
        GameController.LevelWin();
    }
}
