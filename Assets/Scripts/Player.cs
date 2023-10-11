using UnityEngine;

public class Player : MonoBehaviour
{
    public int ColorId;
    private Player[] players;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().color = GameplayManager.Instance.Colors[ColorId];
        players = FindObjectsOfType<Player>();
    }
    // Use this method to access the players array
    public Player[] GetPlayers()
    {
        return players;
    }
}
