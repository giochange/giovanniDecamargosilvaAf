using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Dead;
    private bool pauseGame = false;// ativação de pause

    // Start is called before the first frame update
    void Start()
    {
        Dead.SetActive(false);// setagem da tela de game over
    }

    public void GameOver()
    {
        Dead.SetActive(true);
        ToggleTime();
    }

    private void ToggleTime()
    {
        pauseGame = !pauseGame;// parada do jogo

        if (pauseGame)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void Retry()// recarrega a fase
    {
        ToggleTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
}
