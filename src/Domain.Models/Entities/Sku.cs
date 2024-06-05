namespace Domain.Models.Entities
{
    public class Sku
    {
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public Product Product { get; private set; }
        public int LocationId { get; private set; }
        public Location Location { get; private set; }
        public int Quantity { get; private set; }
        public int Allocated { get; private set; }
        public int Available => Quantity - Allocated;
        public bool CanBeAllocated => !Location.IsLocked;
        public void Allocate(int quantity)
        {
            if (Allocated + quantity > Quantity)
                throw new Exception("Stock level can't go below zero");

            Allocated += quantity;
        }

        public void Deallocate(int quantity)
        {
            if (Allocated - quantity < 0)
                throw new Exception();
                
            Allocated -= quantity;
            Quantity += quantity;
        }
    }
}
