using Microsoft.AspNetCore.Mvc;
using ShoppingCart.EventFeed;

namespace ShoppingCart.Shoppingcart
{
    using Microsoft.AspNetCore.Mvc;
    using ShoppingCart;

    [Route("/shoppingcart")]
    public class ShoppingCartController(
        IShoppingCartStore shoppingCartStore,
        IProductCatalogClient productCatalogClient,
        IEventStore eventStore)
        : Controller
    {
        private readonly IShoppingCartStore shoppingCartStore = shoppingCartStore;
        private readonly IProductCatalogClient productCatalogClient = productCatalogClient;
        private readonly IEventStore eventStore = eventStore;

        [HttpGet("{userId:int}")]
        public ShoppingCart Get(int userId)
        {
            return this.shoppingCartStore.Get(userId);
        }

        [HttpPost("{userId:int}/items")]
        public async Task<ShoppingCart> Post(int userId, [FromBody] int[] productIds)
        {
            var shoppingCart = shoppingCartStore.Get(userId);
            var shoppingCartItems = await this.productCatalogClient.GetShoppingCartItems(productIds);

            shoppingCart.AddItems(shoppingCartItems, eventStore);
            shoppingCartStore.Save(shoppingCart);
            return shoppingCart;
        }

        [HttpDelete("{userid:int}/items")]
        public ShoppingCart Delete(
            int userId,
            [FromBody] int[] productIds)
        {
            var shoppingCart = this.shoppingCartStore.Get(userId);
            shoppingCart.RemoveItems(productIds, this.eventStore);
            this.shoppingCartStore.Save(shoppingCart);
            return shoppingCart;
        }
    }
}