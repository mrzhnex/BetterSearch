using UnityEngine;
using USB079;
using RemoteAdmin;
using System.Collections.Generic;
using CraftKnife;
using System.Linq;
using Exiled.Events.EventArgs;
using Exiled.API.Features;

namespace BetterSearch
{
    public class SetEvents
    {
        private bool ChanceToShowItemWithSearchf(ItemType itemType)
        {
            if (itemType == ItemType.GunCOM15 || itemType == ItemType.GunUSP)
            {
                if (Global.rand.Next(0, 10) > 0)
                {
                    return true;
                }
            }
            else if (itemType == ItemType.GunMP7 || itemType == ItemType.GunProject90)
            {
                if (Global.rand.Next(0, 100) > 5)
                {
                    return true;
                }
            }
            else if (itemType == ItemType.KeycardChaosInsurgency || itemType.ToString().ToLower().Contains("card"))
            {
                if (Global.rand.Next(0, 10) > 6)
                {
                    return true;
                }
            }
            else if (itemType == ItemType.Flashlight)
            {
                if (Global.rand.Next(0, 10) > 5)
                {
                    return true;
                }
            }
            else if (itemType == ItemType.Radio || itemType == ItemType.Disarmer)
            {
                if (Global.rand.Next(0, 10) > 2)
                {
                    return true;
                }
            }
            else if (itemType == ItemType.GrenadeFrag)
            {
                if (Global.rand.Next(0, 10) > 4)
                {
                    return true;
                }
            }
            else if (itemType == ItemType.GrenadeFlash)
            {
                if (Global.rand.Next(0, 10) > 5)
                {
                    return true;
                }
            }
            else if (itemType == ItemType.Medkit)
            {
                if (Global.rand.Next(0, 10) > 7)
                {
                    return true;
                }
            }
            else if (itemType == ItemType.WeaponManagerTablet)
            {
                if (Global.rand.Next(0, 10) > 7)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
            return false;
        }

        internal void OnSendingConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            ev.Allow = false;
            if (!Global.can_use_commands)
            {
                ev.ReturnMessage = "Дождитесь начала раунда!";
                return;
            }

            if (ev.Name.ToLower() == "search")
            {
                if (ev.Player.Team == Team.SCP)
                {
                    ev.ReturnMessage = Global._isscp;
                    return;
                }
                if (ev.Player.GameObject.GetComponent<Search>() != null)
                {
                    ev.ReturnMessage = Global._already_search + ev.Player.GameObject.GetComponent<Search>().target.Nickname;
                    return;
                }
                if (ev.Player.GameObject.GetComponent<CooldownSearch>() != null)
                {
                    ev.ReturnMessage = Global._cooldown_search + ev.Player.GameObject.GetComponent<CooldownSearch>().cooldown.ToString();
                    return;
                }
                Player target = null;
                if (Physics.Raycast((ev.Player.PlayerCamera.forward * 1.01f) + ev.Player.GameObject.transform.position, ev.Player.PlayerCamera.forward, out RaycastHit hit, Global.distance_to_search))
                {
                    if (hit.transform.GetComponent<QueryProcessor>() == null)
                    {
                        ev.ReturnMessage = Global._toolong;
                        return;
                    }
                    else
                    {
                        if (Player.Get(hit.transform.gameObject) == null || Player.Get(hit.transform.gameObject).Team == Team.SCP)
                        {
                            ev.ReturnMessage = Global._targetisscp;
                            return;
                        }
                        else
                        {
                            target = Player.Get(hit.transform.gameObject);
                        }

                        target.ClearBroadcasts();
                        target.Broadcast(10, "Вас обыскивает " + ev.Player.Nickname + ", не двигайтесь в течении: " + Global.time_search, Broadcast.BroadcastFlags.Normal);
                        ev.Player.GameObject.AddComponent<Search>();
                        ev.Player.GameObject.GetComponent<Search>().target = target;
                        ev.Player.GameObject.GetComponent<Search>().localposTarget = target.Position;
                        ev.ReturnMessage = Global._success_start_search + target.Nickname + Global._success_start_search2 + Global.time_search.ToString();
                        return;
                    }
                }
            }
            else if (ev.Name.ToLower() == "searchf")
            {
                if (ev.Player.Team == Team.SCP)
                {
                    ev.ReturnMessage = Global._isscp;
                    return;
                }
                if (ev.Player.GameObject.GetComponent<Search>() != null)
                {
                    ev.ReturnMessage = Global._already_search + ev.Player.GameObject.GetComponent<Search>().target.Nickname;
                    return;
                }
                if (ev.Player.GameObject.GetComponent<CooldownSearchF>() != null)
                {
                    ev.ReturnMessage = Global._cooldown_search + ev.Player.GameObject.GetComponent<CooldownSearchF>().cooldown.ToString();
                    return;
                }
                Player target = null;
                if (Physics.Raycast((ev.Player.PlayerCamera.forward * 1.01f) + ev.Player.GameObject.transform.position, ev.Player.PlayerCamera.forward, out RaycastHit hit, Global.distance_to_searchf))
                {
                    if (hit.transform.GetComponent<QueryProcessor>() == null)
                    {
                        ev.ReturnMessage = Global._toolong;
                        return;
                    }
                    else
                    {
                        Dictionary<int, Dictionary<ItemType, string>> stealmenu = new Dictionary<int, Dictionary<ItemType, string>>();
                        string answer = "";
                        if (Player.Get(hit.transform.gameObject) == null || Player.Get(hit.transform.gameObject).Team == Team.SCP)
                        {
                            ev.ReturnMessage = Global._targetisscp;
                            return;
                        }
                        else
                        {
                            target = Player.Get(hit.transform.gameObject);
                        }

                        int count = 1;

                        foreach (Inventory.SyncItemInfo item in target.Inventory.items)
                        {
                            if (ChanceToShowItemWithSearchf(item.id))
                            {
                                answer = answer + count.ToString() + ") " + Global.items[item.id] + "\n";
                                stealmenu.Add(count, new Dictionary<ItemType, string> { { item.id, Global.items[item.id] } });
                            }
                            else
                            {
                                answer = answer + count.ToString() + ") " + Global.hidden_item + "\n";
                                stealmenu.Add(count, new Dictionary<ItemType, string> { { ItemType.None, Global.hidden_item } });
                            }
                            count += 1;
                        }
                        if (target.GameObject.GetComponent<UsbHolder>() != null)
                        {
                            answer = answer + count.ToString() + ") " + Global.hidden_item + "\n";
                            stealmenu.Add(count, new Dictionary<ItemType, string> { { ItemType.None, Global.hidden_item } });
                            count += 1;
                        }
                        if (target.GameObject.GetComponent<KnifeHolder>() != null)
                        {
                            answer = answer + count.ToString() + ") " + Global._knifeitemname + "\n";
                            stealmenu.Add(count, new Dictionary<ItemType, string> { { ItemType.None, Global._knifeitemname } });
                            count += 1;
                        }
                        if (ev.Player.GameObject.GetComponent<StealMenu>() == null)
                        {
                            ev.Player.GameObject.AddComponent<StealMenu>();
                        }
                        if (!Global.IsFullRp)
                            ev.Player.GameObject.AddComponent<CooldownSearchF>();
                        ev.Player.GameObject.GetComponent<StealMenu>().itemsToSteal = stealmenu;
                        ev.Player.GameObject.GetComponent<StealMenu>().target = target;
                        ev.Player.GameObject.GetComponent<StealMenu>().globalsearch = false;
                        ev.Player.GameObject.GetComponent<StealMenu>().myitems = false;
                        ev.ReturnMessage = "При быстром обыске у " + target.Nickname + " вы нашли: " + "\n" + answer;
                        return;
                    }
                }
            }
            else if (ev.Name.ToLower() == "steal")
            {
                if (ev.Player.Team == Team.SCP)
                {
                    ev.ReturnMessage = Global._isscp;
                    return;
                }
                if (ev.Player.Role == RoleType.Spectator)
                {
                    ev.ReturnMessage = Global._isspectator;
                    return;
                }
                if (ev.Player.IsCuffed)
                {
                    ev.ReturnMessage = Global._ishandcuff;
                    return;
                }
                if (ev.Player.GameObject.GetComponent<Search>() != null)
                {
                    ev.ReturnMessage = Global._try_steal_with_search;
                    return;
                }
                if (ev.Arguments.Count < 1)
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                if (!int.TryParse(ev.Arguments[0].ToLower(), out int id))
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                if (id > Global.maximum_items)
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                if (ev.Player.GameObject.GetComponent<StealMenu>() == null)
                {
                    ev.ReturnMessage = Global._not_search_try_steal;
                    return;
                }
                if (ev.Player.GameObject.GetComponent<StealMenu>().myitems)
                {
                    ev.ReturnMessage = Global._cannotstealandimpoud;
                    return;
                }
                if (ev.Player.GameObject.GetComponent<StealMenu>().globalsearch)
                {
                    ev.ReturnMessage = Global._cannotstealonlyimpoud;
                    return;
                }
                if (ev.Player.GameObject.GetComponent<StealProccess>() != null)
                {
                    ev.ReturnMessage = Global._already_steal + ev.Player.GameObject.GetComponent<StealProccess>().itemname;
                    return;
                }
                if (ev.Player.GameObject.GetComponent<CooldownSteal>() != null)
                {
                    ev.ReturnMessage = Global._cooldown_steal + ev.Player.GameObject.GetComponent<CooldownSteal>().cooldown.ToString();
                    return;
                }

                Player target = Player.Get(ev.Player.GameObject.GetComponent<StealMenu>().target.Id);

                if (target == null)
                {
                    Object.Destroy(ev.Player.GameObject.GetComponent<StealMenu>());
                    ev.ReturnMessage = Global._somethingwrong;
                    return;
                }
                if (id > ev.Player.GameObject.GetComponent<StealMenu>().itemsToSteal.Count)
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                Dictionary<ItemType, string> stealitem = ev.Player.GameObject.GetComponent<StealMenu>().itemsToSteal[id];
                ItemType curType = new List<ItemType>(stealitem.Keys)[0];
                string curName = new List<string>(stealitem.Values)[0];
                bool hidden_Inventory = false;
                bool iscurrentitem = false;
                if (curType == ItemType.None && curName == Global.hidden_item)
                {
                    ev.ReturnMessage = Global._hiddenitem_try_steal;
                    return;
                }
                if (curType == ItemType.None)
                {
                    if (curName == Global._usb079itemname && target.GameObject.GetComponent<UsbHolder>() == null)
                    {
                        ev.ReturnMessage = Global._nothaveitem;
                        return;
                    }
                    if (curName == Global._knifeitemname && target.GameObject.GetComponent<KnifeHolder>() == null)
                    {
                        ev.ReturnMessage = Global._nothaveitem;
                        return;
                    }

                    hidden_Inventory = true;
                }
                else
                {
                    if (target.Inventory.items.Where(x => x.id == curType).FirstOrDefault() == default)
                    {
                        ev.ReturnMessage = Global._nothaveitem;
                        return;
                    }
                    if (target.CurrentItem.id == curType)
                    {
                        iscurrentitem = true;
                    }
                }
                if (iscurrentitem)
                {
                    target.ClearBroadcasts();
                    target.Broadcast(5, "Игрок " + ev.Player.Nickname + " пытался украсть у вас из рук " + curName, Broadcast.BroadcastFlags.Normal);
                }
                ev.Player.GameObject.AddComponent<StealProccess>();
                ev.Player.GameObject.GetComponent<StealProccess>().target = target;
                ev.Player.GameObject.GetComponent<StealProccess>().itemtype = curType;
                ev.Player.GameObject.GetComponent<StealProccess>().itemname = curName;
                ev.Player.GameObject.GetComponent<StealProccess>().hidden_Inventory = hidden_Inventory;
                ev.Player.GameObject.GetComponent<StealProccess>().iscurrentitem = iscurrentitem;
                ev.ReturnMessage = Global._success_start_steal + curName;
                return;
            }
            else if (ev.Name.ToLower() == "impoud")
            {
                if (ev.Player.Team == Team.SCP)
                {
                    ev.ReturnMessage = Global._isscp;
                    return;
                }
                if (ev.Player.Role == RoleType.Spectator)
                {
                    ev.ReturnMessage = Global._isspectator;
                    return;
                }
                if (ev.Player.IsCuffed)
                {
                    ev.ReturnMessage = Global._ishandcuff;
                    return;
                }
                if (ev.Player.GameObject.GetComponent<Search>() != null)
                {
                    ev.ReturnMessage = Global._try_impoud_with_search;
                    return;
                }
                if (ev.Arguments.Count < 1)
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                if (!int.TryParse(ev.Arguments[0].ToLower(), out int id))
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                if (id > Global.maximum_items)
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                if (ev.Player.GameObject.GetComponent<StealMenu>() == null)
                {
                    ev.ReturnMessage = Global._not_search_try_steal;
                    return;
                }
                if (ev.Player.GameObject.GetComponent<StealMenu>().myitems)
                {
                    ev.ReturnMessage = Global._cannotstealandimpoud;
                    return;
                }
                if (ev.Player.GameObject.GetComponent<StealMenu>().globalsearch == false)
                {
                    ev.ReturnMessage = Global._cannotimpoudstealonly;
                    return;
                }
                if (ev.Player.Inventory.items.Where(x => x.id == ItemType.Disarmer).FirstOrDefault() == default)
                {
                    ev.ReturnMessage = Global._nothavecuritem + Global.items[ItemType.Disarmer];
                    return;
                }
                Player target = Player.Get(ev.Player.GameObject.GetComponent<StealMenu>().target.Id);

                if (target == null)
                {
                    Object.Destroy(ev.Player.GameObject.GetComponent<StealMenu>());
                    ev.ReturnMessage = Global._somethingwrong;
                    return;
                }
                if (id > ev.Player.GameObject.GetComponent<StealMenu>().itemsToSteal.Count)
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                Dictionary<ItemType, string> impouditem = ev.Player.GameObject.GetComponent<StealMenu>().itemsToSteal[id];
                ItemType curType = new List<ItemType>(impouditem.Keys)[0];
                string curName = new List<string>(impouditem.Values)[0];
                bool hidden_Inventory = false;
                if (curType == ItemType.None)
                {
                    if (curName == Global._usb079itemname && target.GameObject.GetComponent<UsbHolder>() == null)
                    {
                        ev.ReturnMessage = Global._nothaveitem;
                        return;
                    }
                    if (curName == Global._knifeitemname && target.GameObject.GetComponent<KnifeHolder>() == null)
                    {
                        ev.ReturnMessage = Global._nothaveitem;
                        return;
                    }

                    hidden_Inventory = true;
                }
                else
                {
                    if (target.Inventory.items.Where(x => x.id == curType).FirstOrDefault() == default)
                    {
                        ev.ReturnMessage = Global._nothaveitem;
                        return;
                    }
                }

                if (hidden_Inventory)
                {
                    if (curName == Global._usb079itemname)
                    {
                        if (target.GameObject.GetComponent<UsbHolder>() == null)
                        {
                            ev.Player.ClearBroadcasts();
                            ev.Player.Broadcast(10, "Конфискация не удалась, " + target.Nickname + " не имеет " + curName, Broadcast.BroadcastFlags.Normal);
                        }
                        else
                        {
                            Object.Destroy(target.GameObject.GetComponent<UsbHolder>());
                            ev.Player.ClearBroadcasts();
                            ev.Player.Broadcast(10, "Вы уничтожили " + curName, Broadcast.BroadcastFlags.Normal);
                            target.ClearBroadcasts();
                            target.Broadcast(10, ev.Player.Nickname + " уничтожил " + curName, Broadcast.BroadcastFlags.Normal);
                        }
                    }
                    else if (curName == Global._knifeitemname)
                    {
                        if (target.GameObject.GetComponent<KnifeHolder>() == null)
                        {
                            ev.ReturnMessage = "Конфискация не удалась, " + target.Nickname + " не имеет " + curName;
                            return;
                        }
                        else
                        {
                            if (ev.Player.GameObject.GetComponent<KnifeHolder>() != null)
                            {
                                ev.ReturnMessage = "Вы не смогли конфисковать " + curName + ", так как у вас уж есть " + curName;
                                return;
                            }
                            else
                            {
                                Object.Destroy(target.GameObject.GetComponent<KnifeHolder>());
                                ev.Player.ClearBroadcasts();
                                ev.Player.Broadcast(10, "Вы уничтожили " + curName, Broadcast.BroadcastFlags.Normal);
                                target.ClearBroadcasts();
                                target.Broadcast(10, ev.Player.Nickname + " уничтожил " + curName, Broadcast.BroadcastFlags.Normal);
                            }
                        }
                    }
                }
                else
                {
                    if (target.Inventory.items.Where(x => x.id == curType).FirstOrDefault() == default)
                    {
                        ev.ReturnMessage = "Конфискация не удалась, " + target.Nickname + " не имеет " + curName;
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < target.Inventory.items.Count; i++)
                        {
                            if (target.Inventory.items[i].id == curType)
                                target.Inventory.items.Remove(target.Inventory.items[i]);
                        }
                        ev.Player.AddItem(curType);
                        target.ClearBroadcasts();
                        target.Broadcast(10, ev.Player.Nickname + " конфисковал у вас " + curName, Broadcast.BroadcastFlags.Normal);
                    }
                }
                ev.ReturnMessage = "Вы конфисковали " + curName;
                return;
            }
            else if (ev.Name.ToLower() == "myitems")
            {
                Dictionary<int, Dictionary<ItemType, string>> stealmenu = new Dictionary<int, Dictionary<ItemType, string>>();
                string answer = "";
                int count = 1;
                foreach (Inventory.SyncItemInfo item in ev.Player.Inventory.items)
                {
                    answer = answer + count.ToString() + ") " + Global.items[item.id] + "\n";
                    stealmenu.Add(count, new Dictionary<ItemType, string> { { item.id, Global.items[item.id] } });
                    count += 1;
                }
                if (ev.Player.GameObject.GetComponent<UsbHolder>() != null)
                {
                    answer = answer + count.ToString() + ") " + Global._usb079itemname + "\n";
                    stealmenu.Add(count, new Dictionary<ItemType, string> { { ItemType.None, Global._usb079itemname } });
                    count += 1;
                }
                if (ev.Player.GameObject.GetComponent<KnifeHolder>() != null)
                {
                    answer = answer + count.ToString() + ") " + Global._knifeitemname + "\n";
                    stealmenu.Add(count, new Dictionary<ItemType, string> { { ItemType.None, Global._knifeitemname } });
                    count += 1;
                }
                if (ev.Player.GameObject.GetComponent<StealMenu>() == null)
                {
                    ev.Player.GameObject.AddComponent<StealMenu>();
                }
                ev.Player.GameObject.GetComponent<StealMenu>().itemsToSteal = stealmenu;
                ev.Player.GameObject.GetComponent<StealMenu>().target = ev.Player;
                ev.Player.GameObject.GetComponent<StealMenu>().globalsearch = true;
                ev.Player.GameObject.GetComponent<StealMenu>().myitems = true;
                ev.ReturnMessage = "Полный список ваших вещей: " + "\n" + answer;
                return;
            }
            else if (ev.Name.ToLower() == "searchh")
            {
                if (ev.Player.Team == Team.SCP && ev.Player.Role != RoleType.Scp049)
                {
                    ev.ReturnMessage = Global._isscp;
                    return;
                }
                if (ev.Player.GameObject.GetComponent<Search>() != null)
                {
                    ev.ReturnMessage = Global._already_search + ev.Player.GameObject.GetComponent<Search>().target.Nickname;
                    return;
                }
                Player target = null;
                if (Physics.Raycast((ev.Player.PlayerCamera.forward * 1.01f) + ev.Player.GameObject.transform.position, ev.Player.PlayerCamera.forward, out RaycastHit hit, Global.distance_to_searchf))
                {
                    if (hit.transform.GetComponent<QueryProcessor>() == null)
                    {
                        ev.ReturnMessage = Global._toolong;
                        return;
                    }
                    else
                    {
                        if (Player.Get(hit.transform.gameObject) == null || Player.Get(hit.transform.gameObject).Team == Team.SCP)
                        {
                            ev.ReturnMessage = Global._targetisscp;
                            return;
                        }
                        else
                        {
                            target = Player.Get(hit.transform.gameObject);
                        }

                        if (ev.Player.Role == RoleType.ClassD)
                        {
                            ev.ReturnMessage = "Вы осмотрели игрока " + target.Nickname + ". Его состояние здоровья вы оцениваете как " + Global.health[Global.rand.Next(GetHealthState((int)target.Health, target.MaxHealth) - 1, GetHealthState((int)target.Health, target.MaxHealth) + 2)];
                            return;
                        }
                        else if (ev.Player.Role == RoleType.Scientist)
                        {
                            ev.ReturnMessage = "Вы осмотрели игрока " + target.Nickname + ". Его состояние здоровья вы оцениваете как " + Global.health[GetHealthState((int)target.Health, target.MaxHealth)] + " (" + (int)target.Health + "/" + target.MaxHealth + ")";
                            return;
                        }
                        else
                        {
                            ev.ReturnMessage = "Вы осмотрели игрока " + target.Nickname + ". Его состояние здоровья вы оцениваете как " + Global.health[GetHealthState((int)target.Health, target.MaxHealth)];
                            return;
                        }
                    }
                }
            }
        }

        internal void OnRoundStarted()
        {
            Global.can_use_commands = true;
        }

        public void OnWaitingForPlayers()
        {
            Global.can_use_commands = false;
        }

        private int GetHealthState(int hp, int maxhp)
        {
            int stage = maxhp / 5; 
            if (hp < stage)
            {
                return 5;
            }
            else if (hp < (stage * 2))
            {
                return 4;
            }
            else if (hp < (stage * 3))
            {
                return 3;
            }
            else if (hp < (stage * 4))
            {
                return 2;
            }
            else
            {
                return 1;
            }

        }
    }
}