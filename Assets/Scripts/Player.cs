using UnityEngine;

public class Player : MonoBehaviour
{
    public int SpriteId;
    GameplayManager gameplayManager;

    private void Awake()
    {
        gameplayManager = new GameplayManager();
        int SpriteCount = Mathf.Max(GameplayManager.Instance.Sprites.Count - 10 + gameplayManager.game_level, 24);
        SpriteId = Random.Range(1, SpriteCount);
        GetComponent<SpriteRenderer>().sprite = GameplayManager.Instance.Sprites[SpriteId];
    }
}
