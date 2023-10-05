namespace BookStoreClassLibrary.Core.Entities
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
