using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptidHunter.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int UserProfileId { get; set; }
        public int PostId { get; set; }
    }
}
