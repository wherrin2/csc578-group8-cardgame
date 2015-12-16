using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSC578_Project
{
    class MovableObject : DrawableObject
    {
        public bool Visible { get; set; } = true;
        public bool IsSelected { get; set; }
        public bool IsLocked { get; set; }
        public int Rank { get; set; }
        public int Value { get; set; }

        [JsonExtensionData]
        private IDictionary<string, JToken> _additionalData;

        public bool IsSelectable(int requesterId)
        {
            return (!IsLocked && OwnerId == requesterId);
        }
    }
}
