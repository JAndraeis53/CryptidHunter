using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptidHunter.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string UserProfileId { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
