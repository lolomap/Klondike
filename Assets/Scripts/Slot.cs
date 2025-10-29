using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Slot : MonoBehaviour
{
    public Vector2 Offset;

    public RectTransform RectTransform { get; private set; }
    
    protected readonly List<Card> Content = new();

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public abstract bool CanTakeFrom(Card card);
    public abstract bool CanAdd(List<Card> cards);

    /// <summary>
    /// Pop all cards under selected (included) in the same order
    /// </summary>
    /// <param name="card">Selected card</param>
    public List<Card> TakeFrom(Card card)
    {
        int id = Content.FindIndex(x => x == card);
        List<Card> result = Content.Skip(id).ToList();
        Content.RemoveRange(id, result.Count);

        return result;
    }

    /// <summary>
    /// Adds all selected cards to the end in the same order
    /// </summary>
    /// <param name="cards">Selected cards</param>
    public void Add(List<Card> cards)
    {
        if (!cards.Any()) return;

        Vector3 lastPosition = Content.Count > 0
            ? Content[^1].RectTransform.anchoredPosition
            : new Vector2(0f, -cards[0].RectTransform.rect.height / 2f);
        foreach (Card card in cards)
        {
            Content.Add(card);
            //TODO: Replace with DOTWEEN!
            card.transform.SetParent(transform);
            card.RectTransform.anchorMin = new(0.5f, 1f);
            card.RectTransform.anchorMax = new(0.5f, 1f);
            card.RectTransform.anchoredPosition = lastPosition + (Vector3)Offset;
            lastPosition = card.RectTransform.anchoredPosition;
        }
    }
}
