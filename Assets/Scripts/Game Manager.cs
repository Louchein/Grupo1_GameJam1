using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject[] pilesOfDirt;
    float pilesAmount = 0;
    PlayerController playerControlerScript;
    public GameObject gameOverPanel;
    public GameObject familyFoundPanel;
    public GameObject gameCompletedPanel;
    // Start is called before the first frame update

    private void OnEnable()
    {
        Time.timeScale = 1;
    }
    void Start()
    {
        playerControlerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        StartCoroutine("SpawnPileOfDirt");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControlerScript.gameOver)
        {
            ShowGameOver();
        }

        if (playerControlerScript.familyCount == 4)
        {
            ShowGameCompleted();
        }
    }

    IEnumerator SpawnPileOfDirt()
    {
        for (int i = 0; i < 10; i++)
        {
            if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-20.0f, 14.0f), -0.6f, Random.Range(-25.0f, 18.0f));
                Instantiate(pilesOfDirt[i], spawnPosition, Quaternion.identity);
                pilesAmount++;
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-20.0f, 14.0f), -0.6f, Random.Range(-25.0f, 18.0f));
                Instantiate(pilesOfDirt[5], spawnPosition, Quaternion.identity);
                pilesAmount++;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    void ShowGameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        familyFoundPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        AudioManager.Instance.StopMusic();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ShowGameCompleted()
    {
        Cursor.lockState = CursorLockMode.None;
        familyFoundPanel.SetActive(false);
        gameCompletedPanel.SetActive(true);
        Time.timeScale = 0;
        AudioManager.Instance.StopMusic();
    }

    public void GoHome()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
