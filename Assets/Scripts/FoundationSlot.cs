using System.Collections.Generic;

public class FoundationSlot : Slot
{
	public override bool CanTakeFrom(Card card)
	{
		return card == Content[^1];
	}

	public override bool CanAdd(List<Card> card)
	{
		return card.Count == 1;
	}
}