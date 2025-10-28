using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public enum SuitType
    {
        Spades,
        Clubs,
        Diamonds,
        Hearts
    }
    
    // 1 - A, 13 - K
    public byte Rank;
    public SuitType Suit;
    
    public bool IsClosed { get; private set; }

    public Image Face;
    public Image Back;

    public void Flip()
    {
        IsClosed = !IsClosed;
        Face.enabled = !IsClosed;
        Back.enabled = IsClosed;
    }
}
