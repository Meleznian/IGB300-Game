using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI YouDiedtxt;

    bool gameHasEnded = false;
    public float restartDelay = 0.5f; 

    public void EndGame()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("GAME OVER!!!");
            YouDiedtxt.gameObject.SetActive(true);//for gameObject
            //YouDiedtxt.enabled = true;//for component only 
            Invoke("Restart", restartDelay);
            //Restart();
        }
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//return the name of the current scene
    }

}
