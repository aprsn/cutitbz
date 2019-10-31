using System;

namespace CutitBz.Models
{
    public class Link
    {
        public int id { get; set; }
        public string fullLink { get; set; }
        public string shortLink { get; set; }
        public int hits { get; set; }
        public int views { get; set; }
        public string pin { get; set; }
        public int displayed { get; set; }
        public DateTime created { get; set; }
    }
}