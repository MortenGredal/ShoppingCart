namespace ShoppingCart.EventFeed;

public record Event(long SequenceNumber, DateTimeOffset OccurredAt, string Name, object Content);