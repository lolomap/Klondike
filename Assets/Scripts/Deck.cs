using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Deck : MonoBehaviour, ISlot
{
    public static Deck Instance;
    
    public Transform DraggingParent;
    public List<Slot> TableauColumns = new();
    public int FirstColumnSize;
    public int ColumnSizeStep;
    public Vector2 DraggedOffset;

    public Card CardPrefab;

    public List<Card> DraggedCards = new();
    
    private Stack<Card> _cards;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        List<Card> cards = new();
        
        // Init 52 cards
        for (byte i = 1; i < 14; i++)
        {
            foreach (Card.SuitType suit in Enum.GetValues(typeof(Card.SuitType)))
            {
                // Another implementation way: instantiation could happen on Take only
                Card card = Instantiate(CardPrefab, transform, false);
                card.Rank = i;
                card.Suit = suit;
                cards.Add(card);
            }
        }
        cards.Shuffle();
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.SetSiblingIndex(i);
        }
        _cards = new(cards);
        
        // Fill tableau with deck cards
        int columnSize = FirstColumnSize;
        foreach (Slot column in TableauColumns)
        {
            column.Add(_cards.Take(columnSize).Reverse().ToList(), false);
            for (int i = 0; i < columnSize; i++) _cards.Pop();
            columnSize += ColumnSizeStep;
        }
    }

    public bool CanTakeFrom(Card card)
    {
        return card == _cards.Peek();
    }

    public bool CanAdd(List<Card> cards)
    {
        return cards.Count == 1;
    }

    public List<Card> TakeFrom(Card card)
    {
        return new() {_cards.Pop()};
    }

    public void Add(List<Card> cards, bool animated = true)
    {
        Card card = cards[0];
        
        _cards.Push(card);
        card.transform.SetParent(transform);
        if (animated)
        {
            card.RectTransform.DOAnchorMin(new(0.5f, 1f), 0.5f);
            card.RectTransform.DOAnchorMax(new(0.5f, 1f), 0.5f);
            card.RectTransform.DOAnchorPos(new(0f, -cards[0].RectTransform.rect.height / 2f), 0.5f);
        }
        else
        {
            card.RectTransform.anchorMin = new(0.5f, 1f);
            card.RectTransform.anchorMax = new(0.5f, 1f);
            card.RectTransform.anchoredPosition = new(0f, -cards[0].RectTransform.rect.height / 2f);
        }
    }
}
