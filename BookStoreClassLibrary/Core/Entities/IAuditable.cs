namespace BookStoreClassLibrary.Core.Entities
{
    public interface IAuditable
    {
        DateTime CreatedDate { get; set; }
        DateTime ModifiedDate { get; set;}
    }
}
