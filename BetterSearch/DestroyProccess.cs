using UnityEngine;
using USB079;
using CraftKnife;
using EXILED.Extensions;
using System.Linq;

namespace BetterSearch
{
    class DestroyProccess : MonoBehaviour
    {
        private float timer = 0f;
        private readonly float timeIsUp = 1.0f;
        public float progress = Global.time_destroy;
        private Vector3 startpos;
        private ReferenceHub destroyer;
        public ItemType curType;
        public bool hidden_inventory;
        
        public void Start()
        {
            startpos = gameObject.transform.position;
            destroyer = Player.GetPlayer(gameObject);
        }

        public void Update()
        {
            timer += Time.deltaTime;
            if (timer >= timeIsUp)
            {
                timer = 0f;
                progress = progress - timeIsUp;
                if (hidden_inventory)
                {
                    if (Global.items[curType] == Global._usb079itemname && gameObject.GetComponent<UsbHolder>() == null)
                    {
                        destroyer.ClearBroadcasts();
                        destroyer.Broadcast(10, "Уничтожение прекращено: у вас нет " + Global.items[curType], true);
                        Destroy(gameObject.GetComponent<DestroyProccess>());
                    }
                    if (Global.items[curType] == Global._knifeitemname && gameObject.GetComponent<KnifeHolder>() == null)
                    {
                        destroyer.ClearBroadcasts();
                        destroyer.Broadcast(10, "Уничтожение прекращено: у вас нет " + Global.items[curType], true);
                        Destroy(gameObject.GetComponent<DestroyProccess>());
                    }
                }
                else
                {
                    if (destroyer.inventory.items.Where(x => x.id == curType).FirstOrDefault() == default)
                    {
                        destroyer.ClearBroadcasts();
                        destroyer.Broadcast(10, "Уничтожение прекращено: у вас нет " + Global.items[curType], true);
                        Destroy(gameObject.GetComponent<DestroyProccess>());
                    }
                }
            }

            if (progress <= 0f)
            {
                if (hidden_inventory)
                {
                    if (Global.items[curType] == Global._usb079itemname)
                    {
                        if (gameObject.GetComponent<UsbHolder>() == null)
                        {
                            destroyer.ClearBroadcasts();
                            destroyer.Broadcast(10, "Уничтожение прекращено: у вас нет " + Global.items[curType], true);
                        }
                        else
                        {
                            Destroy(gameObject.GetComponent<UsbHolder>());
                            destroyer.ClearBroadcasts();
                            destroyer.Broadcast(10, "Уничтожение " + Global.items[curType] + " успешно завершено", true);
                        }
                    }
                    if (Global.items[curType] == Global._knifeitemname)
                    {
                        if (gameObject.GetComponent<KnifeHolder>() == null)
                        {
                            destroyer.ClearBroadcasts();
                            destroyer.Broadcast(10, "Уничтожение прекращено: у вас нет " + Global.items[curType], true);
                        }
                        else
                        {
                            Destroy(gameObject.GetComponent<KnifeHolder>());
                            destroyer.ClearBroadcasts();
                            destroyer.Broadcast(10, "Уничтожение " + Global.items[curType] + " успешно завершено", true);
                        }
                    }
                }
                else
                {
                    if (destroyer.inventory.items.Where(x => x.id == curType).FirstOrDefault() == default)
                    {
                        destroyer.ClearBroadcasts();
                        destroyer.Broadcast(10, "Уничтожение прекращено: у вас нет " + Global.items[curType], true);
                    }
                    else
                    {
                        for (int i = 0; i < destroyer.inventory.items.Count; i++)
                        {
                            if (destroyer.inventory.items[i].id == curType)
                                destroyer.inventory.items.Remove(destroyer.inventory.items[i]);
                        }
                        destroyer.ClearBroadcasts();
                        destroyer.Broadcast(10, "Уничтожение " + Global.items[curType] + " успешно завершено", true);
                    }
                }
                Destroy(gameObject.GetComponent<DestroyProccess>());
            }

            if (Vector3.Distance(gameObject.transform.position, startpos) > Global.distance_to_search_not_move)
            {
                destroyer.ClearBroadcasts();
                destroyer.Broadcast(10, "Уничтожение " + Global.items[curType] + " прекращено: вы сдвинулись", true);
                Destroy(gameObject.GetComponent<DestroyProccess>());
            }

        }  


        public void OnDestroy()
        {
            gameObject.AddComponent<CooldownDestroy>();
        }
    }
}