using UnityEngine;
using System.Collections.Generic;
using Exiled.API.Features;

namespace BetterSearch
{
    public class StealMenu : MonoBehaviour
    {
        public Dictionary<int, Dictionary<ItemType, string>> itemsToSteal = new Dictionary<int, Dictionary<ItemType, string>>();
        public Player target;
        public bool globalsearch;
        public bool myitems;
    }
}