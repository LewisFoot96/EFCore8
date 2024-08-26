using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublisherDomain
{
    public class BookCover
    {
        public int BookCoverId { get; set; }
        public string DesignIdeas { get; set; }
        public bool DigitalOnly { get; set; }
        public List<Artist> Artists { get; set; } = [];
    }
}
