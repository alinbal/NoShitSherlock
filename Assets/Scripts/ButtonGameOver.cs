using UnityEngine;
using System.Collections;

public class ButtonGameOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
