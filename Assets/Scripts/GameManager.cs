using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        player = FindFirstObjectByType<Player_Move>().gameObject;

    }

    void Update()
    {
        if (player.transform.position.y < -10)
        {
            
            HighscoreManager.instance.AddHighScore(HighscoreManager.instance.CurrentScore);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


}
