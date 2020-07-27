using UnityEngine;
using USB079;
using CraftKnife;
using System.Collections.Generic;
using Exiled.API.Features;

namespace BetterSearch
{
    public class Search : MonoBehaviour
    {
        public Player target;
        public Player searcher;

        private float timer = 0f;
        private readonly float timeIsUp = 1.0f;
        public Vector3 localposTarget;
        private Vector3 localposSearcher;
        private float progress = Global.time_search;

        public void Start()
        {
            searcher = Player.Get(gameObject);
            localposSearcher = searcher.Position;
        }


        public void Update()
        {
            timer += Time.deltaTime;
            if (timer >= timeIsUp)
            {
                timer = 0f;
                progress -= timeIsUp;
                
            }

            if (progress <= 0f)
            {
                Dictionary<int, Dictionary<ItemType, string>> stealmenu = new Dictionary<int, Dictionary<ItemType, string>>();
                string answer = "";
                int count = 1;
                foreach (Inventory.SyncItemInfo item in target.Inventory.items)
                {
                    answer = answer + count.ToString() + ") " + Global.items[item.id] + "\n";
                    stealmenu.Add(count, new Dictionary<ItemType, string> { { item.id, Global.items[item.id] } });
                    count += 1;
                }
                if (target.GameObject.GetComponent<UsbHolder>() != null)
                {
                    answer = answer + count.ToString() + ") " + Global._usb079itemname + "\n";
                    stealmenu.Add(count, new Dictionary<ItemType, string> { { ItemType.None, Global._usb079itemname } });
                    count += 1;
                }
                if (target.GameObject.GetComponent<KnifeHolder>() != null)
                {
                    answer = answer + count.ToString() + ") " + Global._knifeitemname + "\n";
                    stealmenu.Add(count, new Dictionary<ItemType, string> { { ItemType.None, Global._knifeitemname } });
                    count += 1;
                }
                if (gameObject.GetComponent<StealMenu>() == null)
                {
                    gameObject.AddComponent<StealMenu>();
                }
                gameObject.GetComponent<StealMenu>().itemsToSteal = stealmenu;
                gameObject.GetComponent<StealMenu>().target = target;
                gameObject.GetComponent<StealMenu>().globalsearch = true;
                gameObject.GetComponent<StealMenu>().myitems = false;
                searcher.SendConsoleMessage("При полном обыске у " + target.Nickname + " вы нашли: " + "\n" + answer, "yellow");
                searcher.ClearBroadcasts();
                searcher.Broadcast(10, "<color=#42aaff>Обыск удался. Откройте консоль для просмотра вещей " + target.Nickname + "</color>", Broadcast.BroadcastFlags.Normal);
                Destroy(gameObject.GetComponent<Search>());
            }
            if (Vector3.Distance(gameObject.transform.position, localposSearcher) > Global.distance_to_search_not_move || Vector3.Distance(target.Position, localposTarget) > Global.distance_to_search_not_move)
            {
                searcher.ClearBroadcasts();
                searcher.Broadcast(10, "<color=#228b22>Обыск не удался: цель или вы сдвинулись</color>", Broadcast.BroadcastFlags.Normal);
                Destroy(gameObject.GetComponent<Search>());
            }
        }

        public void OnDestroy()
        {
            if (!Global.IsFullRp)
                gameObject.AddComponent<CooldownSearch>();
        }
    }
}