using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private List<Vector3> _spawnPos;

    [HideInInspector]
    public int SpriteId;

    [HideInInspector]
    public Score NextScore;
    // Create an instance of the GameplayManager class
    GameplayManager gameplayManager;
    GameObject gameplayManagerObject;
    private void Awake()
    {
        // get component GameplayManager from object GameplayManager
        gameplayManagerObject = GameObject.Find("GameplayManager");
        gameplayManager = gameplayManagerObject.GetComponent<GameplayManager>();
        int game_level = gameplayManager.get_game_level();
        // _moveSpeed = random 2 5
        _moveSpeed = Random.Range(1.5f , 5f);
        hasGameFinished = false;
        transform.position = new Vector3(-48.1f, Random.Range(-4, 16), 0);
        // scale object random -game_level * 0.1f,0
        // transform.localScale += new Vector3(Random.Range(-game_level * 0.2f, 0), Random.Range(-game_level * 0.2f, 0), 0);
        // player random rotation x axis game_level * 5
        // player.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, game_level * 10));
        int SpriteCount = Mathf.Min(GameplayManager.Instance.Sprites.Count - 20 + gameplayManager.game_level, GameplayManager.Instance.Sprites.Count);
        SpriteId = Random.Range(0, SpriteCount);
        GetComponent<SpriteRenderer>().sprite = GameplayManager.Instance.Sprites[SpriteId];
    }

    private void FixedUpdate()
    {
        if (hasGameFinished || GameplayManager.Instance.isPaused) return;
        transform.Translate(_moveSpeed * Time.fixedDeltaTime * Vector3.right);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Obstacle"))
        {
            GameplayManager.Instance.GameEnded();
        }
    }

    private void OnEnable()
    {
        GameplayManager.Instance.GameEnd += GameEnded;
    }

    private void OnDisable()
    {
        GameplayManager.Instance.GameEnd -= GameEnded;
    }

    private bool hasGameFinished;

    private void GameEnded()
    {
        hasGameFinished = true;
    }
}
