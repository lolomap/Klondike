using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(DraggableUI))]
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

    public RectTransform RectTransform { get; private set; }

    private DraggableUI _draggable;
    
    private Vector2 _startPosition;
    private Vector2 _forceEndPosition;
    private Slot _startSlot;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        _draggable = GetComponent<DraggableUI>();

        _draggable.DragBegin += OnDragBegin;
        _draggable.DragEnd += OnDragEnd;
    }

    private void Update()
    {
        if (!DraggableUI.Dragged) return;
        int pos = Deck.DraggedCards.FindIndex(x => x == this);
        if (pos < 0) return;

        RectTransform.anchoredPosition = DraggableUI.Dragged.Rect.anchoredPosition + Deck.DraggedOffset * pos;
    }

    private void OnDragBegin(PointerEventData data)
    {
        _startPosition = RectTransform.anchoredPosition;
        
        List<RaycastResult> hits = new();
        EventSystem.current.RaycastAll(data, hits);
        RaycastResult hit = hits.Find(x => x.gameObject.GetComponent<Slot>() != null);
        
        if (hit.gameObject != null)
        {
            Slot hitSlot = hit.gameObject.GetComponent<Slot>();
            if (hitSlot != null)
            {
                _startSlot = hitSlot;
                _draggable.IsDraggable = hitSlot.CanTakeFrom(this);
                if (_draggable.IsDraggable)
                    Deck.DraggedCards = hitSlot.TakeFrom(this);
            }
        }
        else
        {
            _draggable.IsDraggable = false;
        }
    }
    
    private void OnDragEnd(PointerEventData data)
    {
        List<RaycastResult> hits = new();
        EventSystem.current.RaycastAll(data, hits);
        RaycastResult hit = hits.Find(x => x.gameObject.GetComponent<Slot>() != null);
        
        if (hit.gameObject != null)
        {
            Slot hitSlot = hit.gameObject.GetComponent<Slot>();
            if (hitSlot != null)
            {
                if (hitSlot.CanAdd(Deck.DraggedCards))
                    hitSlot.Add(Deck.DraggedCards);
                else
                    _startSlot.Add(Deck.DraggedCards);
            }
        }
        else _startSlot.Add(Deck.DraggedCards);
        
        Deck.DraggedCards.Clear();
    }

    public void Flip()
    {
        IsClosed = !IsClosed;
        Face.enabled = !IsClosed;
        Back.enabled = IsClosed;
    }

    public static void MoveBunch(List<Card> cards, Vector2 pos, Vector2 offset)
    {
        Vector2 p = pos;
        foreach (Card card in cards)
        {
            card.RectTransform.anchoredPosition = p; //TODO: REPLACE WITH DOTWEEN!
            p += offset;
        }
    }
}
