using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _livesSprite;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _pressRKey;
    private GameManager _gameManager;
    

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _pressRKey.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    public void ScoreUpdater(int playerscore)
    {
        //making the score to be displayed using the tostring keyword
        _scoreText.text = "Score: " + playerscore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        //sprite changes everytime there is a change in the player's life, due to the use of an array
        _livesImage.sprite = _livesSprite[currentLives];
        if (currentLives == 0)

        {
            GameOverSequence();
        }
    }

    //Game Over screen, all thise texts.
    private void GameOverSequence()
    { 
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(FlickerSwitchRoutine());
        _pressRKey.gameObject.SetActive(true);
        _gameManager.GameOver();
    }

    //Disappearing text, appearing after 3 sec.
    IEnumerator FlickerSwitchRoutine()
    {
        while (true)
        {
            _gameOverText.text="";
            yield return new WaitForSeconds(0.3f);
            _gameOverText.text = "Game Over";
            yield return new WaitForSeconds(0.3f);
        }

    }

}
