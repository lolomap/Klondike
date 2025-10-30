# Тестовое задание:  Реализация базового пасьянса в Unity

## Реализованные механики

1. Карта - перетаскиваются группами и помещаются в стопки.
2. Стопки - позволяют помещать внутрь себя карты с заданным смещением и вынимать из них группы карт, в соответствии с назначенными правилами.
3. Колода - создает базовые 52 карты и размещает их в игровые стопки (расклад), использует стек, ведет себя аналогично стопкам-приемникам.

https://github.com/user-attachments/assets/158253a2-43d0-4bff-9189-05b3639160b5

https://github.com/user-attachments/assets/9b2ac55f-fcd0-409f-bcbb-2410347c5347



## Документация к скриптам

`DraggableUI` - компонент, реализующий возможность перетаскивать UI элемент по экрану и вызывать дополнительную логику при событиях перетаскивания.

```c#
[RequireComponent(typeof(RectTransform))]
public class DraggableUI : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public static DraggableUI Dragged { get; private set; }
    public static bool IsDragging { get; private set; }

    public bool IsDraggable = true; 

    public RectTransform Rect { get; private set; }

    public delegate void DragBeginHandler(PointerEventData data);
    public event DragBeginHandler DragBegin;

    public delegate void DragEndHandler(PointerEventData data);
    public event DragEndHandler DragEnd;

    public void OnBeginDrag(PointerEventData eventData)
    public void OnDrag(PointerEventData eventData)
    public void OnEndDrag(PointerEventData eventData)
    private void Awake()
}
```

`Card` - обрабатывает перетаскивание с помощью событий DraggableUI, вызывая логику стопки под выбранной картой с помощью рейкаста, в Update происходит привязка позиции выбранных карт к перетаскиваемой.
Содержит базовые данные игральной карты (масть, значение, лицевая/оборотная сторона).

```c#
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
        ...

        _draggable.DragBegin += OnDragBegin;
        _draggable.DragEnd += OnDragEnd;
    }

    private void Update()

    private void OnDragBegin(PointerEventData data)
    private void OnDragEnd(PointerEventData data)
    public void Flip()
}
```

`ISlot` - позволяет взаимодействовать с компонентом как со стопкой: добавлять и забирать карты, проверять доступность действия.

```c#
public interface ISlot
{
    public bool CanTakeFrom(Card card);
    public bool CanAdd(List<Card> cards);

    public List<Card> TakeFrom(Card card);
    public void Add(List<Card> cards, bool animated = true);
}
```

`Slot` - абстрактная реализация стопки с логикой хранения, добавления и взятия карт, для определения различного поведения стопок (напр. стопок-лесенок расклада и стопок-приемников) реализуется соответствующая логика абстрактных методов `CanTakeFrom` и `CanAdd`.

```c#
public abstract class Slot : MonoBehaviour, ISlot
{
    public Vector2 Offset;

    public RectTransform RectTransform { get; private set; }
    
    protected readonly List<Card> Content = new();

    private void Awake()

    public abstract bool CanTakeFrom(Card card);
    public abstract bool CanAdd(List<Card> cards);

    /// <summary>
    /// Pop all cards under selected (included) in the same order
    /// </summary>
    /// <param name="card">Selected card</param>
    public List<Card> TakeFrom(Card card)

    /// <summary>
    /// Adds all selected cards to the end in the same order
    /// </summary>
    /// <param name="cards">Selected cards</param>
    /// <param name="animated">Using DOTween to move card into slot</param>
    public void Add(List<Card> cards, bool animated = true)
}
```

`TableauSlot` и `FoundationSlot` - реализации компонента `Slot` для стопок-лесенок и стопок-приемников соответственно.

`Deck` - колода, хранящая свое содержимое в стеке, реализует интерфейс стопки, начальное заполнение расклада из своего содержимого, хранит буфер перемещаемых карт.

```c#
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
    private void Start()

    public bool CanTakeFrom(Card card)
    public bool CanAdd(List<Card> cards)

    public List<Card> TakeFrom(Card card)
    public void Add(List<Card> cards, bool animated = true)
}
```
