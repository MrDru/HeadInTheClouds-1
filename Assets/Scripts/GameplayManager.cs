using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    #region START
    public int game_level;
    // get game_level
    public int get_game_level()
    {
        return game_level;
    }
    private bool hasGameFinished;

    public static GameplayManager Instance;

    public List<Sprite> Sprites;
    private Player[] players;
    private Player[] start_players;

    private Player matchingPlayer;
    private int level_counter;
    GameObject goodJob3xObject;
    [SerializeField] private AudioClip Cloud_Select_Right;
    [SerializeField] private AudioClip Cloud_Select_Wrong;
    private AudioSource audioSource;
    public bool isPaused = false;
    [SerializeField] private Transform nextLevelPopup;
    private void Awake()
    {
        goodJob3xObject = GameObject.Find("good job3x");
        goodJob3xObject.GetComponent<SpriteRenderer>().enabled = false;
        nextLevelPopup.transform.localScale = Vector3.zero;
        // goodJob3xObject.SetActive(false);
        game_level = 1;
        level_counter = 3;
        Instance = this;
        Sprites = ThelistOfAllSprites.Instance.editableList;
        hasGameFinished = false;
        // Create an instance of the GameManager class if it doesn't exist
        if (GameManager.Instance == null)
        {
            GameManager.Instance = gameObject.AddComponent<GameManager>();
        }
        GameManager.Instance.IsInitialized = true;

        score_txt = 0;
        _scoreText.text = ((int)score_txt).ToString();
        StartCoroutine(SpawnScore());
        // spawn 5 SpawnScore() for start
        for (int i = 0; i < 7; i++)
        {
            SpawnScore_start();
        }
        players = FindObjectsOfType<Player>();
        start_players = players;
        matchingPlayer = null;
        audioSource = GetComponent<AudioSource>();
        _spawnTime = 1.2f;
        random_player();
    }

    #endregion

    #region GAME_LOGIC

    [SerializeField] private ScoreEffect _scoreEffect;
    // Assuming you have a Player class with a SpriteId property
    private void Update()
    {
        if (isPaused) return;

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
            if (mousePos.y < -10f) return;
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            bool no_player_match = false;
            foreach(var score in scores)
            {
                if (hit.collider == null) continue;
                if(score.gameObject == hit.collider.gameObject)
                {
                    foreach (Player player in players)
                        {
                            if (player.SpriteId == score.SpriteId)
                            {
                                no_player_match = true;
                                player.SpriteId = -1;
                                // change sprite render color to green
                                player.GetComponent<SpriteRenderer>().color = Color.green;
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
                            t.Init(Sprites[score.SpriteId]);
                            score.transform.DOPunchScale(new Vector3(1f, 1f, 1f), 0.2f).SetEase(Ease.OutBounce).OnComplete(() =>
                            {
                                Destroy(score.gameObject);
                            });

                            // remove tempScore from scores list
                            scores.Remove(score);
                            UpdateScore();
                            // Destroy(matchingPlayer.gameObject);
                            audioSource.PlayOneShot(Cloud_Select_Right);
                            break;
                        }
                }
            }
            if (!no_player_match)
            {
                audioSource.PlayOneShot(Cloud_Select_Wrong);
            }
        }
        // if player.GetComponent<SpriteRenderer>().color == Color.green for all players
        bool all_green = true;
        foreach (Player player in players)
        {
            if (player.GetComponent<SpriteRenderer>().color != Color.green)
            {
                all_green = false;
                break;
            }
        }
        if (all_green)
        {
            StartCoroutine(goodjob());
            game_level++;
            level_counter = 0;
            _spawnTime = Mathf.Max(0.05f, _spawnTime -  0.05f);
            Debug.Log("_spawnTime " + _spawnTime);
            random_player();
        }
    }
    private void random_player()
    {
        players = start_players;
        List<int> randomList = new List<int>();
        int rangeMin = 0;
        int rangeMax =  Mathf.Min(Sprites.Count - 20 + game_level, Sprites.Count);
        while (randomList.Count < 3)
        {
            int randomInt = Random.Range(rangeMin, rangeMax);
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
            // change sprite render color to magenta
            player.GetComponent<SpriteRenderer>().color = Color.magenta;
        }
    }
    public IEnumerator goodjob()
    {
        if (goodJob3xObject != null && !(score_txt % 9 == 0))
        {
            goodJob3xObject.GetComponent<SpriteRenderer>().enabled = true;
            goodJob3xObject.transform.localScale = Vector3.zero;
            goodJob3xObject.transform.DOScale(new Vector3(6.7296f, 6.7296f, 6.7296f), 0.45f).SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(1.5f);
            goodJob3xObject.transform.DOScale(new Vector3(0f, 0f, 0f), 0.35f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                goodJob3xObject.GetComponent<SpriteRenderer>().enabled = false;
            });

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

        //Show "Next level" popup
        if (score_txt % 9 == 0 && !isPaused)
        {
            nextLevelPopup.transform.localScale = Vector3.zero;
            nextLevelPopup.transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.InBounce);
            isPaused = true;
            Score[] scores2 = FindObjectsOfType<Score>();
            foreach (Score score in scores2)
            {
                Destroy(score.gameObject);
                scores.Remove(score);
            }
        }
    }

    public void OnPressedNextLevel()
    {
        nextLevelPopup.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBounce);
        isPaused = false;
    }

    private float _spawnTime;
    [SerializeField] private Score _scorePrefab;
    private Score CurrentScore;
    List<Score> scores = new List<Score>();
    private IEnumerator SpawnScore()
    {

        while(!hasGameFinished)
        {
            yield return new WaitForSeconds(1f);
            if (!isPaused)
            {
                Score score = Instantiate(_scorePrefab);
                // var tempScore = Instantiate(_scorePrefab);
                _scorePrefab.transform.localScale = new Vector3(4,4,4);
                scores.Add(score);
            }

            yield return new WaitForSeconds(_spawnTime);
        }
    }

    private void SpawnScore_start()
    {
        Score score = Instantiate(_scorePrefab);
        // var tempScore = Instantiate(_scorePrefab);
        _scorePrefab.transform.localScale = new Vector3(4,4,4);
        scores.Add(score);
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
