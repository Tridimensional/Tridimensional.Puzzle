using Tridimensional.Puzzle.Core.Enumeration;
using UnityEngine;

namespace Tridimensional.Puzzle.Service.Contract
{
    public class GameContract
    {
        public OnlineType OnlineType { get; set; }
        public Texture2D Image { get; set; }
        public string ImageSource { get; set; }
        public string ImageHash { get; set; }
        public Difficulty Difficulty { get; set; }
        public SlicePattern SlicePattern { get; set; }
        public SliceContract SliceContract { get; set; }
    }
}
