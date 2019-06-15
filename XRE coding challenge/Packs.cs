using System.Collections.Generic;

namespace XRECodingChallenge
{
    // Storage of pack lists and total size
    class Packs
    {
        public List<Pack> PackList { get; set; }
        public int TotalSize { get; set; }
        public Packs() { }
    }
}