using UnityEngine;
using USB079;
using CraftKnife;
using System.Linq;
using Exiled.API.Features;

namespace BetterSearch
{
    class StealProccess : MonoBehaviour
    {
        public Player target;
        private Player stealer;
        public ItemType itemtype;
        public string itemname;
        public bool hidden_Inventory;
        public bool iscurrentitem;
        private float timer = 0f;
        private readonly float timeIsUp = 1.0f;
        private float progress = Global.time_steal;

        public void Start()
        {
            stealer = Player.Get(gameObject);
        }

        public void Update()
        {
            timer += Time.deltaTime;
            if (timer >= timeIsUp)
            {
                timer = 0f;
                progress -= timeIsUp;
                if (hidden_Inventory)
                {
                    if (target.GameObject.GetComponent<UsbHolder>() == null)
                    {
                        stealer.ClearBroadcasts();
                        stealer.Broadcast(10, "Кража не удалась, " + target.Nickname + " не имеет " + itemname, Broadcast.BroadcastFlags.Normal);
                        Destroy(gameObject.GetComponent<StealProccess>());
                    }
                }
                else
                {
                    if (target.Inventory.items.Where(x => x.id == itemtype).FirstOrDefault() == default)
                    {
                        stealer.ClearBroadcasts();
                        stealer.Broadcast(10, "Кража не удалась, " + target.Nickname + " не имеет " + itemname, Broadcast.BroadcastFlags.Normal);
                        Destroy(gameObject.GetComponent<StealProccess>());
                    }
                }
            }

            if (progress <= 0f)
            {
                if (hidden_Inventory)
                {
                    if (itemname == Global._usb079itemname)
                    {
                        if (target.GameObject.GetComponent<UsbHolder>() == null)
                        {
                            stealer.ClearBroadcasts();
                            stealer.Broadcast(10, "<color=#228b22>Кража не удалась, " + target.Nickname + " не имеет " + itemname + "</color>", Broadcast.BroadcastFlags.Normal);
                        }
                        else
                        {
                            Destroy(target.GameObject.GetComponent<UsbHolder>());
                            gameObject.AddComponent<UsbHolder>();
                            stealer.ClearBroadcasts();
                            stealer.Broadcast(10, "<color=#ff0000>Вы украли " + itemname + "</color>", Broadcast.BroadcastFlags.Normal);
                        }
                    }
                    else if (itemname == Global._knifeitemname)
                    {
                        if (target.GameObject.GetComponent<KnifeHolder>() == null)
                        {
                            stealer.ClearBroadcasts();
                            stealer.Broadcast(10, "<color=#228b22>Кража не удалась, " + target.Nickname + " не имеет " + itemname + "</color>", Broadcast.BroadcastFlags.Normal);
                        }
                        else
                        {
                            if (gameObject.GetComponent<KnifeHolder>() != null)
                            {
                                stealer.ClearBroadcasts();
                                stealer.Broadcast(10, "<color=#228b22>Вы не смогли украсть " + itemname + ", так как у вас уж есть " + itemname + "</color>", Broadcast.BroadcastFlags.Normal);
                            }
                            else
                            {
                                Destroy(target.GameObject.GetComponent<KnifeHolder>());
                                gameObject.AddComponent<KnifeHolder>();
                                stealer.ClearBroadcasts();
                                stealer.Broadcast(10, "<color=#ff0000>Вы украли " + itemname + "</color>", Broadcast.BroadcastFlags.Normal);
                            }
                        }
                    }
                }
                else
                {
                    if (target.Inventory.items.Where(x => x.id == itemtype).FirstOrDefault() == default)
                    {
                        stealer.ClearBroadcasts();
                        stealer.Broadcast(10, "<color=#228b22>Кража не удалась, " + target.Nickname + " не имеет " + itemname + "</color>", Broadcast.BroadcastFlags.Normal);
                    }
                    else
                    {
                        if (!ChanceToStealItem(itemtype))
                        {
                            stealer.ClearBroadcasts();
                            stealer.Broadcast(10, "<color=#228b22>У вас не получилось украсть " + itemname + "</color>", Broadcast.BroadcastFlags.Normal);
                            if (iscurrentitem)
                            {
                                target.ClearBroadcasts();
                                target.Broadcast(10, "<color=#ff0000>Вы чувствуете, как " + stealer.Nickname + " пытался выбить у вас из рук " + itemname + "</color>", Broadcast.BroadcastFlags.Normal);
                            }
                            else
                            {
                                target.ClearBroadcasts();
                                target.Broadcast(10, "<color=#228b22>Вы чувствуете странное движение в том месте, где вы храните " + itemname + "</color>", Broadcast.BroadcastFlags.Normal);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < target.Inventory.items.Count; i++)
                            {
                                if (target.Inventory.items[i].id == itemtype)
                                    target.Inventory.items.Remove(target.Inventory.items[i]);
                            }

                            stealer.AddItem(itemtype);
                            stealer.ClearBroadcasts();
                            stealer.Broadcast(10, "<color=#ff0000>Вы украли " + itemname + "</color>", Broadcast.BroadcastFlags.Normal);
                            if (iscurrentitem)
                            {
                                string bc = "<color=#ff0000>Игрок " + stealer.Nickname + " выбил у вас из рук " + itemname;
                                if (Global.rand.Next(0, 100) == 33)
                                {
                                    bc = bc + " P.s. вы слепой долбоеб";
                                }
                                target.ClearBroadcasts();
                                target.Broadcast(10, bc + "</color>", Broadcast.BroadcastFlags.Normal);
                            }
                        }
                    }
                }
                Destroy(gameObject.GetComponent<StealProccess>());
            }

            if (Vector3.Distance(gameObject.transform.position, target.Position) > Global.distance_to_steal_between)
            {
                stealer.ClearBroadcasts();
                stealer.Broadcast(10, "<color=#228b22>Кража не удалась, " + target.Nickname + " слишком далеко</color>", Broadcast.BroadcastFlags.Normal);
                Destroy(gameObject.GetComponent<StealProccess>());
            }
        }

        private bool ChanceToStealItem(ItemType itemType)
        {
            if (itemtype == ItemType.GunCOM15)
            {
                if (target.Team == Team.CDP)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 7)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 5)
                        {
                            return true;
                        }
                    }
                }
                else if (target.Role == RoleType.Scientist)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 6)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 4)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 8)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 6)
                        {
                            return true;
                        }
                    }
                }
            }
            else if (itemtype == ItemType.GunUSP)
            {
                if (target.Team == Team.CDP)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 7)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 6)
                        {
                            return true;
                        }
                    }
                }
                else if (target.Role == RoleType.Scientist)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 6)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 5)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 8)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 7)
                        {
                            return true;
                        }
                    }
                }
            }
            else if (itemtype == ItemType.GunProject90 || itemtype == ItemType.GunMP7)
            {
                if (target.Team == Team.CDP)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 100) > 85)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 7)
                        {
                            return true;
                        }
                    }
                }
                else if (target.Role == RoleType.Scientist)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 100) > 75)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 6)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 100) > 95)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 8)
                        {
                            return true;
                        }
                    }
                }
            }
            else if (itemtype == ItemType.GunE11SR)
            {
                if (target.Team == Team.CDP)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 100) > 87)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 100) > 85)
                        {
                            return true;
                        }
                    }
                }
                else if (target.Role == RoleType.Scientist)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 100) > 77)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 100) > 75)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 100) > 97)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 100) > 95)
                        {
                            return true;
                        }
                    }
                }
            }
            else if (itemtype == ItemType.GunLogicer)
            {
                if (target.Team == Team.CDP)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 100) > 85)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 100) > 87)
                        {
                            return true;
                        }
                    }
                }
                else if (target.Role == RoleType.Scientist)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 100) > 75)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 100) > 77)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 100) > 95)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 100) > 97)
                        {
                            return true;
                        }
                    }
                }
            }
            else if (itemtype == ItemType.MicroHID)
            {
                if (target.Team == Team.CDP)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 7)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 100) > 85)
                        {
                            return true;
                        }
                    }
                }
                else if (target.Role == RoleType.Scientist)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 6)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 100) > 75)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 8)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 100) > 95)
                        {
                            return true;
                        }
                    }
                }
            }
            else if (itemtype.ToString().ToLower().Contains("card") || itemtype == ItemType.KeycardChaosInsurgency)
            {
                if (target.Team == Team.CDP)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 6)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 4)
                        {
                            return true;
                        }
                    }
                }
                else if (target.Role == RoleType.Scientist)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 5)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 3)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 7)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 5)
                        {
                            return true;
                        }
                    }
                }
            }
            else if (itemtype == ItemType.Radio || itemtype == ItemType.GrenadeFrag || itemtype == ItemType.GrenadeFlash)
            {
                if (target.Team == Team.CDP)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 3)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 6)
                        {
                            return true;
                        }
                    }
                }
                else if (target.Role == RoleType.Scientist)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 2)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 5)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 7)
                        {
                            return true;
                        }
                    }
                }
            }
            else if (itemtype == ItemType.WeaponManagerTablet)
            {
                if (target.Team == Team.CDP)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 2)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 4)
                        {
                            return true;
                        }
                    }
                }
                else if (target.Role == RoleType.Scientist)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 1)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 3)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 3)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 5)
                        {
                            return true;
                        }
                    }
                }
            }
            else if (itemtype == ItemType.Medkit)
            {
                if (target.Team == Team.CDP)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 3)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 2)
                        {
                            return true;
                        }
                    }
                }
                else if (target.Role == RoleType.Scientist)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 2)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 1)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 3)
                        {
                            return true;
                        }
                    }
                }
            }
            else if (itemtype == ItemType.Flashlight)
            {
                if (target.Team == Team.CDP)
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 2)
                        {
                            return true;
                        }
                    }
                }
                else if (target.Role == RoleType.Scientist)
                {
                    if (iscurrentitem)
                    {
                        return true;
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 1)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (iscurrentitem)
                    {
                        if (Global.rand.Next(0, 10) > 1)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Global.rand.Next(0, 10) > 3)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void OnDestroy()
        {
            gameObject.AddComponent<CooldownSteal>();
        }
    }
}
