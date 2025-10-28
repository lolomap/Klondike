using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Slot : MonoBehaviour
{
    public Vector2 Offset;
    
    protected readonly List<Card> Content = new();

    public abstract bool CanTakeFrom(Card card);
    public abstract bool CanAdd(List<Card> cards);

    public List<Card> TakeFrom(Card card)
    {
        int id = Content.FindIndex(x => x == card);
        List<Card> result = Content.Skip(id).ToList();
        Content.RemoveRange(id, result.Count);

        return result;
    }

    public void Add(IEnumerable<Card> cards)
    {
        Vector3 lastPosition = Content[^1].transform.position;
        foreach (Card card in cards)
        {
            Content.Add(card);
            //TODO: Replace with DOTWEEN!
            card.transform.position = lastPosition + (Vector3)Offset;
            lastPosition = card.transform.position;
        }
    }
}
