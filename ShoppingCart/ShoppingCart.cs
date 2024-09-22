namespace ShoppingCart.ShoppingCart
{
    using global::ShoppingCart.EventFeed;
    using System.Collections.Generic;
    using System.Linq;

    public class ShoppingCart(int userId)
    {
        private readonly HashSet<ShoppingCartItem> _items = [];

        public int UserId { get; } = userId;
        public IEnumerable<ShoppingCartItem> Items => this._items;

        public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems, IEventStore eventStore)
        {
            foreach (var item in shoppingCartItems)
            {
                if (this._items.Add(item))
                    eventStore.Raise(
                        "ShoppingCartItemAdded",
                        new { UserId, item });
            }
        }

        public void RemoveItems(int[] productCatalogueIds, IEventStore eventStore) =>
            this._items.RemoveWhere(i => productCatalogueIds.Contains(i.ProductCatalogueId));
    }

    public record ShoppingCartItem(
        int ProductCatalogueId,
        string ProductName,
        string Description,
        Money Price)
    {
        public virtual bool Equals(ShoppingCartItem? obj)
        {
            return obj != null && this.ProductCatalogueId.Equals(obj.ProductCatalogueId);
        }

        public override int GetHashCode()
        {
            return this.ProductCatalogueId.GetHashCode();
        }
    }

    public record Money(string Currency, decimal Amount);
}