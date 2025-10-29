using System.Collections.Generic;
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
    
    private Vector2 _forceEndPosition;
    private ISlot _startSlot;

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
        int pos = Deck.Instance.DraggedCards.FindIndex(x => x == this);
        if (pos < 0) return;

        RectTransform.anchoredPosition = DraggableUI.Dragged.Rect.anchoredPosition + Deck.Instance.DraggedOffset * pos;
    }

    private void OnDragBegin(PointerEventData data)
    {
        List<RaycastResult> hits = new();
        EventSystem.current.RaycastAll(data, hits);
        RaycastResult hit = hits.Find(x => x.gameObject.GetComponent<ISlot>() != null);
        
        if (hit.gameObject != null)
        {
            ISlot hitSlot = hit.gameObject.GetComponent<ISlot>();
            if (hitSlot != null)
            {
                _startSlot = hitSlot;
                _draggable.IsDraggable = hitSlot.CanTakeFrom(this);
                if (_draggable.IsDraggable)
                {
                    Deck.Instance.DraggedCards = hitSlot.TakeFrom(this);
                    foreach (Card card in Deck.Instance.DraggedCards)
                    {
                        card.transform.SetParent(Deck.Instance.DraggingParent, true);
                    }
                }
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
        RaycastResult hit = hits.Find(x => x.gameObject.GetComponent<ISlot>() != null);
        
        if (hit.gameObject != null)
        {
            ISlot hitSlot = hit.gameObject.GetComponent<ISlot>();
            if (hitSlot != null)
            {
                if (hitSlot.CanAdd(Deck.Instance.DraggedCards))
                    hitSlot.Add(Deck.Instance.DraggedCards);
                else
                    _startSlot.Add(Deck.Instance.DraggedCards);
            }
        }
        else _startSlot.Add(Deck.Instance.DraggedCards);
        
        Deck.Instance.DraggedCards.Clear();
    }

    public void Flip()
    {
        IsClosed = !IsClosed;
        Face.enabled = !IsClosed;
        Back.enabled = IsClosed;
    }
}
