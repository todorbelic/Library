using System.ComponentModel;

namespace BookStoreClassLibrary.Core.Enums
{
    public enum Role
    {
        [Description("ADMINISTRATOR")]
        ADMINISTRATOR = 1,
        [Description("CUSTOMER")]
        CUSTOMER = 2,
        [Description("LIBRARIAN")]
        LIBRARIAN = 3,
    }
}
