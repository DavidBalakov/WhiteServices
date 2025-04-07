using Microsoft.AspNetCore.Identity;

namespace Diploma.Entities
{
    public class RegisteredUser : IdentityUser
    {
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Product> Products { get; set; }
    }
}
