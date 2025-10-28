using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private Stack<Card> _cards;

    public void Awake()
    {
        List<Card> cards = new();
        
        // Init 52 cards
        for (byte i = 1; i < 14; i++)
        {
            cards.Add(new() {Rank = i, Suit = Card.SuitType.Spades});
            cards.Add(new() {Rank = i, Suit = Card.SuitType.Clubs});
            cards.Add(new() {Rank = i, Suit = Card.SuitType.Diamonds});
            cards.Add(new() {Rank = i, Suit = Card.SuitType.Hearts});
        }
        cards.Shuffle();

        _cards = new(cards);
    }

    public void Add(Card card)
    {
        _cards.Push(card);
    }

    public Card Take()
    {
        return _cards.Pop();
    }
}
