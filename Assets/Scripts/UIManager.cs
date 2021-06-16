using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // handle to Text
    [SerializeField] GameManager _gm;
    [SerializeField] private Text _scoreText;
    [SerializeField] private GameObject _gameOverTextObj;
    [SerializeField] private GameObject _restartTextObj;
    [SerializeField] private Image _LivesImg;
    [SerializeField] private Sprite[] _liveSprites;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverTextObj.SetActive(false);
        _gm = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gm == null)
        {
            Debug.LogError("UI_MANAGER::GameManager is NULL");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequqnce();
        }
    }

    public void GameOverSequqnce()
    {
        _gm.GameOver();
        _gameOverTextObj.SetActive(true);
        _restartTextObj.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }


    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverTextObj.GetComponent<Text>().text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverTextObj.GetComponent<Text>().text = "";
            yield return new WaitForSeconds(0.25f);
        }
    }
}
