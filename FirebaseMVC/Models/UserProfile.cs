using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CryptidHunter.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string FirebaseUserId { get; set; }
        public string Email { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }
    }
}
