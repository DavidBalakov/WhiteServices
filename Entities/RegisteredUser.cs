using Microsoft.AspNetCore.Identity;

namespace Diploma.Entities
{
    public class RegisteredUser : IdentityUser
    {
        // public string UserName { get; set; }
        public string Password { get; set; }
        // public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // public string PhoneNumber { get; set; }
        public List<Product> Products { get; set; }
    }
}
