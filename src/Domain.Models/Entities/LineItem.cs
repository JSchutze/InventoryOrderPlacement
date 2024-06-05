namespace Domain.Models.Entities
{
    public class LineItem
    {
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public int RequiredQuantity { get; private set; }
    }
}
