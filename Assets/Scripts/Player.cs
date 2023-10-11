using UnityEngine;

public class Player : MonoBehaviour
{
    public int SpriteId;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = GameplayManager.Instance.Sprites[SpriteId];
    }
}
