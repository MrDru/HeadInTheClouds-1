using UnityEngine;

public class Player : MonoBehaviour
{
    public int SpriteId;
    GameplayManager gameplayManager;

    private void Awake()
    {
        gameplayManager = new GameplayManager();
        int SpriteCount = Mathf.Min(GameplayManager.Instance.Sprites.Count - 15 + gameplayManager.game_level, 24);
        SpriteId = Random.Range(0, SpriteCount);
        GetComponent<SpriteRenderer>().sprite = GameplayManager.Instance.Sprites[SpriteId];
    }
}
