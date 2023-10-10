using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameplayManager : MonoBehaviour
{
    #region START

    private bool hasGameFinished;

    public static GameplayManager Instance;

    public List<Color> Colors;

    private void Awake()
    {
        Instance = this;

        hasGameFinished = false;
        GameManager.Instance.IsInitialized = true;

        score = 0;
        _scoreText.text = ((int)score).ToString();
        StartCoroutine(SpawnScore());

    }

    #endregion

    #region GAME_LOGIC

    [SerializeField] private ScoreEffect _scoreEffect;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !hasGameFinished)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            // if(!hit.collider || !hit.collider.gameObject.CompareTag("Block"))
            // {
            //     GameEnded();
            //     return;
            // }

            int currentScoreId = CurrentScore.ColorId;
            int clickedScoreId = hit.collider.gameObject.GetComponent<Player>().ColorId;
            Score ScoreId = hit.collider.gameObject.GetComponent<Score>();
            Player clickedScore = hit.collider.gameObject.GetComponent<Player>();

            Debug.Log("ScoreId Id: " + ScoreId);
            Debug.Log("Clicked Score Id: " + clickedScoreId);

            // if(currentScoreId != clickedScoreId)
            // {
            //     GameEnded();
            //     return;
            // }
            foreach(var score in scores)
            {
                Debug.Log("Score Id: " + score.ColorId);
                if(score.ColorId == clickedScoreId)
                {
                    var t = Instantiate(_scoreEffect, score.gameObject.transform.position, Quaternion.identity);
                    t.Init(Colors[currentScoreId]);
                    var tempScore = score;
                    Destroy(clickedScore.gameObject);
                    Destroy(score.gameObject);
                    // remove tempScore from scores list
                    scores.Remove(tempScore);
                    UpdateScore();
                    break;
                }
            }

        }
    }

    #endregion

    #region SCORE

    private float score;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private AudioClip _pointClip;

    private void UpdateScore()
    {
        score++;
        SoundManager.Instance.PlaySound(_pointClip);
        _scoreText.text = ((int)score).ToString();
    }

    [SerializeField] private float _spawnTime;
    [SerializeField] private Score _scorePrefab;
    private Score CurrentScore;
    List<Score> scores = new List<Score>();
    private IEnumerator SpawnScore()
    {
        Score prevScore = null;

        while(!hasGameFinished)
        {
            var tempScore = Instantiate(_scorePrefab);
            scores.Add(tempScore);
            if(prevScore == null)
            {
                prevScore = tempScore;
                CurrentScore = prevScore;
            }
            else
            {
                prevScore.NextScore = tempScore;
                prevScore = tempScore;
            }

            yield return new WaitForSeconds(_spawnTime);
        }
    }

    #endregion

    #region GAME_OVER

    [SerializeField] private AudioClip _loseClip;
    public UnityAction GameEnd;

    public void GameEnded()
    {
        hasGameFinished = true;
        GameEnd?.Invoke();
        SoundManager.Instance.PlaySound(_loseClip);
        GameManager.Instance.CurrentScore = (int)score;
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.GoToMainMenu();
    }

    #endregion
}
