using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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

    private void Awake()
    {
        goodJob3xObject = GameObject.Find("good job3x");
        goodJob3xObject.SetActive(false);
        game_level = 1;
        level_counter = 0;
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
        for (int i = 0; i < 9; i++)
        {
            SpawnScore_start();
        }
        players = FindObjectsOfType<Player>();
        start_players = players;
        matchingPlayer = null;
        audioSource = GetComponent<AudioSource>();
        random_player();
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
                            Destroy(score.gameObject);
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
            random_player();
        }
    }
    private void random_player()
    {
        players = start_players;
        List<int> randomList = new List<int>();
        int rangeMin = 0;
        int rangeMax =  Mathf.Min(Sprites.Count - 19 + game_level, 24);
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
            // change sprite render color to White
            player.GetComponent<SpriteRenderer>().color = Color.white;
        }
        Debug.Log("random player list " + randomList[0] + " " + randomList[1] + " " + randomList[2]);
    }
    public IEnumerator goodjob()
    {
        if (goodJob3xObject != null)
        {
            goodJob3xObject.SetActive(true);
            yield return new WaitForSeconds(2);
            goodJob3xObject.SetActive(false);
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

        while(!hasGameFinished)
        {
            yield return new WaitForSeconds(1f);
            Score score = Instantiate(_scorePrefab);
            // var tempScore = Instantiate(_scorePrefab);
            _scorePrefab.transform.localScale = new Vector3(4,4,4);
            scores.Add(score);

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
