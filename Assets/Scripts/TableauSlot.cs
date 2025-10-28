using System.Collections.Generic;

public class TableauSlot : Slot
{
	public override bool CanTakeFrom(Card card)
	{
		return Content.Contains(card);
	}

	public override bool CanAdd(List<Card> cards)
	{
		return true;
	}
}