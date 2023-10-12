using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameplayManager : MonoBehaviour
{
    #region START
    public int game_level;

    private bool hasGameFinished;

    public static GameplayManager Instance;

    public List<Sprite> Sprites;
    private Player[] players;
    private Player[] start_players;

    private Player matchingPlayer;
    private int level_counter;
    private void Awake()
    {
        game_level = 1;
        level_counter = 0;
        Instance = this;
        Sprites = ThelistOfAllSprites.Instance.editableList;
        hasGameFinished = false;
        // Create an instance of the GameManager class if it doesn't exist
        if (GameManager.Instance == null)
        {
            GameManager.Instance = new GameManager();
        }
        GameManager.Instance.IsInitialized = true;

        score_txt = 0;
        _scoreText.text = ((int)score_txt).ToString();
        StartCoroutine(SpawnScore());
        players = FindObjectsOfType<Player>();
        start_players = FindObjectsOfType<Player>();
        matchingPlayer = null;
    }

    #endregion

    #region GAME_LOGIC

    [SerializeField] private ScoreEffect _scoreEffect;
    // Assuming you have a Player class with a SpriteId property
    private void Update()
    {
        for (int i = scores.Count - 1; i >= 0; i--)
        {
            var score = scores[i];
            if (score.transform.position.x > 50)
            {
                scores.RemoveAt(i);
                Destroy(score.gameObject);
            }
        }
        if(Input.GetMouseButtonDown(0) && !hasGameFinished)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            int currentScoreId = CurrentScore.SpriteId;
            bool no_player_match = false;
            foreach(var score in scores)
            {
                if(score.gameObject == hit.collider.gameObject)
                {
                    foreach (Player player in players)
                        {
                            if (player.SpriteId == score.SpriteId)
                            {
                                no_player_match = true;
                            }
                        }
                    if (no_player_match)
                        {

                            foreach (Player player in players)
                                {
                                    if (player.SpriteId == score.SpriteId)
                                    {
                                        matchingPlayer = player;
                                        break;
                                    }
                                }
                            // get player component matching SpriteId from score.SpriteId
                            var t = Instantiate(_scoreEffect, score.gameObject.transform.position, Quaternion.identity);
                            t.Init(Sprites[currentScoreId]);
                            Destroy(score.gameObject);
                            // remove tempScore from scores list
                            scores.Remove(score);
                            UpdateScore();
                            // Destroy(matchingPlayer.gameObject);
                            Debug.Log("IN");
                            break;
                        }
                }
            }
        }
        if (level_counter == 3)
        {
            game_level++;
            level_counter = 0;
            players = start_players;
            List<int> randomList = new List<int>();
            int rangeMin = 0;
            int rangeMax =  Mathf.Min(Sprites.Count - 15 + game_level*2, 24);
            while (randomList.Count < 3)
            {
                int randomInt = Random.Range(rangeMin, rangeMax + 1);
                if (!randomList.Contains(randomInt))
                {
                    randomList.Add(randomInt);
                }
            }
            // random the SpriteId of the players
            for (int i = 0; i < players.Length; i++)
            {
                int randomIndex = randomList[i];
                Player player = players[i];
                player.SpriteId = randomIndex;
                player.GetComponent<SpriteRenderer>().sprite = Sprites[randomIndex];
                // player random rotation x axis game_level * 5
                player.transform.rotation = Quaternion.Euler(Random.Range(0, game_level * 5), 0, 0);
            }
        }
    }

    #endregion

    #region SCORE

    private float score_txt;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private AudioClip _pointClip;

    private void UpdateScore()
    {
        score_txt++;
        level_counter++;
        // SoundManager.Instance.PlaySound(_pointClip);
        _scoreText.text = ((int)score_txt).ToString();
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
            _scorePrefab.transform.localScale = new Vector3(4,4,4);
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
        GameManager.Instance.CurrentScore = (int)score_txt;
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.GoToMainMenu();
    }

    #endregion
}
