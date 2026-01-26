using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //[Header("Settings")]
    [Header("References")]
    public PlayerMovement Movement;
    public PlayerVisual Visual;
    public PlayerCombat Combat;

    [Space(10)]
    [SerializeField] RSO_Player player;

    //[Header("Input")]
    //[Header("Output")]

    private void Awake()
    {
        player.Set(this);
    }
}