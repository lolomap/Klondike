using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<Slot> TableauColumns = new();
    public int FirstColumnSize;
    public int ColumnSizeStep;

    public Card CardPrefab;
    
    private Stack<Card> _cards;

    public void Start()
    {
        List<Card> cards = new();
        
        // Init 52 cards
        for (byte i = 1; i < 14; i++)
        {
            foreach (Card.SuitType suit in Enum.GetValues(typeof(Card.SuitType)))
            {
                Card card = Instantiate(CardPrefab, transform, false);
                card.Rank = i;
                card.Suit = suit;
                cards.Add(card);
            }
        }
        cards.Shuffle();
        _cards = new(cards);
        
        // Fill tableau with deck cards
        int columnSize = FirstColumnSize;
        foreach (Slot column in TableauColumns)
        {
            column.Add(_cards.Take(columnSize).Reverse().ToList());
            for (int i = 0; i < columnSize; i++) _cards.Pop();
            columnSize += ColumnSizeStep;
        }
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
