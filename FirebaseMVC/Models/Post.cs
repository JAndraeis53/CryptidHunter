using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptidHunter.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UserProfileId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
