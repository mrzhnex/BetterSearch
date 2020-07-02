using UnityEngine;
using USB079;
using RemoteAdmin;
using System.Collections.Generic;
using CraftKnife;
using EXILED;
using EXILED.Extensions;
using System.Linq;

namespace BetterSearch
{
    internal class SetEvents
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
        public void OnCallCommand(ConsoleCommandEvent ev)
        {
            if (!Global.can_use_commands)
            {
                ev.ReturnMessage = "Дождитесь начала раунда!";
                return;
            }
            string command = ev.Command.Split(new char[]
            {
                ' '
            })[0].ToLower();

            if (command == "search")
            {
                if (ev.Player.GetTeam() == Team.SCP)
                {
                    ev.ReturnMessage = Global._isscp;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<Search>() != null)
                {
                    ev.ReturnMessage = Global._already_search + ev.Player.gameObject.GetComponent<Search>().target.nicknameSync.Network_myNickSync;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<CooldownSearch>() != null)
                {
                    ev.ReturnMessage = Global._cooldown_search + ev.Player.gameObject.GetComponent<CooldownSearch>().cooldown.ToString();
                    return;
                }
                if (ev.Player.gameObject.GetComponent<DestroyProccess>() != null)
                {
                    ev.ReturnMessage = Global._already_destroy + Global.items[ev.Player.gameObject.GetComponent<DestroyProccess>().curType];
                    return;
                }
                ReferenceHub target = null;
                if (Physics.Raycast((ev.Player.gameObject.GetComponent<Scp049PlayerScript>().plyCam.transform.forward * 1.01f) + ev.Player.gameObject.transform.position, ev.Player.gameObject.GetComponent<Scp049PlayerScript>().plyCam.transform.forward, out RaycastHit hit, Global.distance_to_search))
                {
                    if (hit.transform.GetComponent<QueryProcessor>() == null)
                    {
                        ev.ReturnMessage = Global._toolong;
                        return;
                    }
                    else
                    {
                        if (Player.GetPlayer(hit.transform.gameObject) == null || Player.GetPlayer(hit.transform.gameObject).GetTeam() == Team.SCP)
                        {
                            ev.ReturnMessage = Global._targetisscp;
                            return;
                        }
                        else
                        {
                            target = Player.GetPlayer(hit.transform.gameObject);
                        }

                        target.ClearBroadcasts();
                        target.Broadcast(10, "Вас обыскивает " + ev.Player.nicknameSync.Network_myNickSync + ", не двигайтесь в течении: " + Global.time_search, true);
                        ev.Player.gameObject.AddComponent<Search>();
                        ev.Player.gameObject.GetComponent<Search>().target = target;
                        ev.Player.gameObject.GetComponent<Search>().localposTarget = target.GetPosition();
                        ev.ReturnMessage = Global._success_start_search + target.nicknameSync.Network_myNickSync + Global._success_start_search2 + Global.time_search.ToString();
                        return;
                    }
                }
            }
            else if (command == "searchf")
            {
                if (ev.Player.GetTeam() == Team.SCP)
                {
                    ev.ReturnMessage = Global._isscp;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<Search>() != null)
                {
                    ev.ReturnMessage = Global._already_search + ev.Player.gameObject.GetComponent<Search>().target.nicknameSync.Network_myNickSync;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<CooldownSearchF>() != null)
                {
                    ev.ReturnMessage = Global._cooldown_search + ev.Player.gameObject.GetComponent<CooldownSearchF>().cooldown.ToString();
                    return;
                }
                ReferenceHub target = null;
                if (Physics.Raycast((ev.Player.gameObject.GetComponent<Scp049PlayerScript>().plyCam.transform.forward * 1.01f) + ev.Player.gameObject.transform.position, ev.Player.gameObject.GetComponent<Scp049PlayerScript>().plyCam.transform.forward, out RaycastHit hit, Global.distance_to_searchf))
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
                        if (Player.GetPlayer(hit.transform.gameObject) == null || Player.GetPlayer(hit.transform.gameObject).GetTeam() == Team.SCP)
                        {
                            ev.ReturnMessage = Global._targetisscp;
                            return;
                        }
                        else
                        {
                            target = Player.GetPlayer(hit.transform.gameObject);
                        }

                        int count = 1;
                        
                        foreach (Inventory.SyncItemInfo item in target.inventory.items)
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
                        if (target.gameObject.GetComponent<UsbHolder>() != null)
                        {
                            answer = answer + count.ToString() + ") " + Global.hidden_item + "\n";
                            stealmenu.Add(count, new Dictionary<ItemType, string> { { ItemType.None, Global.hidden_item } });
                            count += 1;
                        }
                        if (target.gameObject.GetComponent<KnifeHolder>() != null)
                        {
                            answer = answer + count.ToString() + ") " + Global._knifeitemname + "\n";
                            stealmenu.Add(count, new Dictionary<ItemType, string> { { ItemType.None, Global._knifeitemname } });
                            count += 1;
                        }
                        if (ev.Player.gameObject.GetComponent<StealMenu>() == null)
                        {
                            ev.Player.gameObject.AddComponent<StealMenu>();
                        }
                        if (!Global.IsFullRp)
                            ev.Player.gameObject.AddComponent<CooldownSearchF>();
                        ev.Player.gameObject.GetComponent<StealMenu>().itemsToSteal = stealmenu;
                        ev.Player.gameObject.GetComponent<StealMenu>().target = target;
                        ev.Player.gameObject.GetComponent<StealMenu>().globalsearch = false;
                        ev.Player.gameObject.GetComponent<StealMenu>().myitems = false;
                        ev.ReturnMessage = "При быстром обыске у " + target.nicknameSync.Network_myNickSync + " вы нашли: " + "\n" + answer;
                        return;
                    }
                }
            }
            else if (command == "steal")
            {
                if (ev.Player.GetTeam() == Team.SCP)
                {
                    ev.ReturnMessage = Global._isscp;
                    return;
                }
                if (ev.Player.GetRole() == RoleType.Spectator)
                {
                    ev.ReturnMessage = Global._isspectator;
                    return;
                }
                if (ev.Player.IsHandCuffed())
                {
                    ev.ReturnMessage = Global._ishandcuff;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<Search>() != null)
                {
                    ev.ReturnMessage = Global._try_steal_with_search;
                    return;
                }
                if (ev.Command.Split(new char[]{ ' ' }).Length < 2)
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                if (!int.TryParse(ev.Command.Split(new char[] { ' ' })[1].ToLower(), out int id))
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                if (id > Global.maximum_items)
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<StealMenu>() == null)
                {
                    ev.ReturnMessage = Global._not_search_try_steal;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<StealMenu>().myitems)
                {
                    ev.ReturnMessage = Global._cannotstealandimpoud;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<StealMenu>().globalsearch)
                {
                    ev.ReturnMessage = Global._cannotstealonlyimpoud;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<StealProccess>() != null)
                {
                    ev.ReturnMessage = Global._already_steal + ev.Player.gameObject.GetComponent<StealProccess>().itemname;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<CooldownSteal>() != null)
                {
                    ev.ReturnMessage = Global._cooldown_steal + ev.Player.gameObject.GetComponent<CooldownSteal>().cooldown.ToString();
                    return;
                }
                if (ev.Player.gameObject.GetComponent<DestroyProccess>() != null)
                {
                    ev.ReturnMessage = Global._already_destroy + Global.items[ev.Player.gameObject.GetComponent<DestroyProccess>().curType];
                    return;
                }
                ReferenceHub target = Player.GetPlayer(ev.Player.gameObject.GetComponent<StealMenu>().target.GetPlayerId());

                if (target == null)
                {
                    Object.Destroy(ev.Player.gameObject.GetComponent<StealMenu>());
                    ev.ReturnMessage = Global._somethingwrong;
                    return;
                }
                if (id > ev.Player.gameObject.GetComponent<StealMenu>().itemsToSteal.Count)
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                Dictionary<ItemType, string> stealitem = ev.Player.gameObject.GetComponent<StealMenu>().itemsToSteal[id];
                ItemType curType = new List<ItemType>(stealitem.Keys)[0];
                string curName = new List<string>(stealitem.Values)[0];
                bool hidden_inventory = false;
                bool iscurrentitem = false;
                if (curType == ItemType.None && curName == Global.hidden_item)
                {
                    ev.ReturnMessage = Global._hiddenitem_try_steal;
                    return;
                }
                if (curType == ItemType.None)
                {
                    if (curName == Global._usb079itemname && target.gameObject.GetComponent<UsbHolder>() == null)
                    {
                        ev.ReturnMessage = Global._nothaveitem;
                        return;
                    }
                    if (curName == Global._knifeitemname && target.gameObject.GetComponent<KnifeHolder>() == null)
                    {
                        ev.ReturnMessage = Global._nothaveitem;
                        return;
                    }

                    hidden_inventory = true;
                }
                else
                {
                    if (target.inventory.items.Where(x => x.id == curType).FirstOrDefault() == default)
                    {
                        ev.ReturnMessage = Global._nothaveitem;
                        return;
                    }
                    if (target.GetCurrentItem().id == curType)
                    {
                        iscurrentitem = true;
                    }
                }
                if (iscurrentitem)
                {
                    target.ClearBroadcasts();
                    target.Broadcast(5, "Игрок " + ev.Player.nicknameSync.Network_myNickSync + " пытался украсть у вас из рук " + curName, true);
                }
                ev.Player.gameObject.AddComponent<StealProccess>();
                ev.Player.gameObject.GetComponent<StealProccess>().target = target;
                ev.Player.gameObject.GetComponent<StealProccess>().itemtype = curType;
                ev.Player.gameObject.GetComponent<StealProccess>().itemname = curName;
                ev.Player.gameObject.GetComponent<StealProccess>().hidden_inventory = hidden_inventory;
                ev.Player.gameObject.GetComponent<StealProccess>().iscurrentitem = iscurrentitem;
                ev.ReturnMessage = Global._success_start_steal + curName;
                return;
            }
            else if (command == "impoud")
            {
                if (ev.Player.GetTeam() == Team.SCP)
                {
                    ev.ReturnMessage = Global._isscp;
                    return;
                }
                if (ev.Player.GetRole() == RoleType.Spectator)
                {
                    ev.ReturnMessage = Global._isspectator;
                    return;
                }
                if (ev.Player.IsHandCuffed())
                {
                    ev.ReturnMessage = Global._ishandcuff;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<Search>() != null)
                {
                    ev.ReturnMessage = Global._try_impoud_with_search;
                    return;
                }
                if (ev.Command.Split(new char[] { ' ' }).Length < 2)
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                if (!int.TryParse(ev.Command.Split(new char[] { ' ' })[1].ToLower(), out int id))
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                if (id > Global.maximum_items)
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<StealMenu>() == null)
                {
                    ev.ReturnMessage = Global._not_search_try_steal;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<StealMenu>().myitems)
                {
                    ev.ReturnMessage = Global._cannotstealandimpoud;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<StealMenu>().globalsearch == false)
                {
                    ev.ReturnMessage = Global._cannotimpoudstealonly;
                    return;
                }
                if (ev.Player.inventory.items.Where(x => x.id == ItemType.Disarmer).FirstOrDefault() == default)
                {
                    ev.ReturnMessage = Global._nothavecuritem + Global.items[ItemType.Disarmer];
                    return;
                }
                bool destroy = false;
                if (ev.Command.Split(new char[] { ' ' }).Length > 2)
                {
                    if (ev.Command.Split(new char[]{ ' '})[2].ToLower() == "destroy")
                    {
                        destroy = true;
                    }
                }
                ReferenceHub target = Player.GetPlayer(ev.Player.gameObject.GetComponent<StealMenu>().target.GetPlayerId());

                if (target == null)
                {
                    Object.Destroy(ev.Player.gameObject.GetComponent<StealMenu>());
                    ev.ReturnMessage = Global._somethingwrong;
                    return;
                }
                if (id > ev.Player.gameObject.GetComponent<StealMenu>().itemsToSteal.Count)
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                Dictionary<ItemType, string> impouditem = ev.Player.gameObject.GetComponent<StealMenu>().itemsToSteal[id];
                ItemType curType = new List<ItemType>(impouditem.Keys)[0];
                string curName = new List<string>(impouditem.Values)[0];
                bool hidden_inventory = false;
                if (curType == ItemType.None)
                {
                    if (curName == Global._usb079itemname && target.gameObject.GetComponent<UsbHolder>() == null)
                    {
                        ev.ReturnMessage = Global._nothaveitem;
                        return;
                    }
                    if (curName == Global._knifeitemname && target.gameObject.GetComponent<KnifeHolder>() == null)
                    {
                        ev.ReturnMessage = Global._nothaveitem;
                        return;
                    }

                    hidden_inventory = true;
                }
                else
                {
                    if (target.inventory.items.Where(x => x.id == curType).FirstOrDefault() == default)
                    {
                        ev.ReturnMessage = Global._nothaveitem;
                        return;
                    }
                }

                if (hidden_inventory)
                {
                    if (curName == Global._usb079itemname)
                    {
                        if (target.gameObject.GetComponent<UsbHolder>() == null)
                        {
                            ev.Player.ClearBroadcasts();
                            ev.Player.Broadcast(10, "Конфискация не удалась, " + target.nicknameSync.Network_myNickSync + " не имеет " + curName, true);
                        }
                        else
                        {
                            Object.Destroy(target.gameObject.GetComponent<UsbHolder>());
                            ev.Player.ClearBroadcasts();
                            ev.Player.Broadcast(10, "Вы уничтожили " + curName, true);
                            target.ClearBroadcasts();
                            target.Broadcast(10, ev.Player.nicknameSync.Network_myNickSync + " уничтожил " + curName, true);
                        }
                    }
                    else if (curName == Global._knifeitemname)
                    {
                        if (target.gameObject.GetComponent<KnifeHolder>() == null)
                        {
                            ev.ReturnMessage = "Конфискация не удалась, " + target.nicknameSync.Network_myNickSync + " не имеет " + curName;
                            return;
                        }
                        else
                        {
                            if (ev.Player.gameObject.GetComponent<KnifeHolder>() != null)
                            {
                                ev.ReturnMessage = "Вы не смогли конфисковать " + curName + ", так как у вас уж есть " + curName;
                                return;
                            }
                            else
                            {
                                Object.Destroy(target.gameObject.GetComponent<KnifeHolder>());
                                ev.Player.ClearBroadcasts();
                                ev.Player.Broadcast(10, "Вы уничтожили " + curName, true);
                                target.ClearBroadcasts();
                                target.Broadcast(10, ev.Player.nicknameSync.Network_myNickSync + " уничтожил " + curName, true);
                            }
                        }
                    }
                }
                else
                {
                    if (target.inventory.items.Where(x => x.id == curType).FirstOrDefault() == default)
                    {
                        ev.ReturnMessage = "Конфискация не удалась, " + target.nicknameSync.Network_myNickSync + " не имеет " + curName;
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < target.inventory.items.Count; i++)
                        {
                            if (target.inventory.items[i].id == curType)
                                target.inventory.items.Remove(target.inventory.items[i]);
                        }
                        if (destroy)
                        {
                            target.ClearBroadcasts();
                            target.Broadcast(10, ev.Player.nicknameSync.Network_myNickSync + " уничтожил " + curName, true);
                        }
                        else
                        {
                            ev.Player.AddItem(curType);
                            target.ClearBroadcasts();
                            target.Broadcast(10, ev.Player.nicknameSync.Network_myNickSync + " конфисковал у вас " + curName, true);
                        }
                    }
                }

                if (hidden_inventory || destroy)
                {
                    ev.ReturnMessage = "Вы уничтожили " + curName;
                    return;
                }
                else
                {
                    ev.ReturnMessage = "Вы конфисковали " + curName;
                    return;
                }

            }
            else if (command == "myitems")
            {
                Dictionary<int, Dictionary<ItemType, string>> stealmenu = new Dictionary<int, Dictionary<ItemType, string>>();
                string answer = "";
                int count = 1;
                foreach (Inventory.SyncItemInfo item in ev.Player.inventory.items)
                {
                    answer = answer + count.ToString() + ") " + Global.items[item.id] + "\n";
                    stealmenu.Add(count, new Dictionary<ItemType, string> { { item.id, Global.items[item.id] } });
                    count = count + 1;
                }
                if (ev.Player.gameObject.GetComponent<UsbHolder>() != null)
                {
                    answer = answer + count.ToString() + ") " + Global._usb079itemname + "\n";
                    stealmenu.Add(count, new Dictionary<ItemType, string> { { ItemType.None, Global._usb079itemname } });
                    count = count + 1;
                }
                if (ev.Player.gameObject.GetComponent<KnifeHolder>() != null)
                {
                    answer = answer + count.ToString() + ") " + Global._knifeitemname + "\n";
                    stealmenu.Add(count, new Dictionary<ItemType, string> { { ItemType.None, Global._knifeitemname } });
                    count = count + 1;
                }
                if (ev.Player.gameObject.GetComponent<StealMenu>() == null)
                {
                    ev.Player.gameObject.AddComponent<StealMenu>();
                }
                ev.Player.gameObject.GetComponent<StealMenu>().itemsToSteal = stealmenu;
                ev.Player.gameObject.GetComponent<StealMenu>().target = ev.Player;
                ev.Player.gameObject.GetComponent<StealMenu>().globalsearch = true;
                ev.Player.gameObject.GetComponent<StealMenu>().myitems = true;
                ev.ReturnMessage = "Полный список ваших вещей: " + "\n" + answer;
                return;
            }
            else if (command == "destroy")
            {
                if (ev.Player.GetRole() == RoleType.Spectator)
                {
                    ev.ReturnMessage = Global._isspectator;
                    return;
                }
                if (ev.Player.IsHandCuffed())
                {
                    ev.ReturnMessage = Global._ishandcuff;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<Search>() != null)
                {
                    ev.ReturnMessage = Global._try_destroy_with_search;
                    return;
                }
                if (ev.Command.Split(new char[] { ' ' }).Length < 2)
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                if (!int.TryParse(ev.Command.Split(new char[] { ' ' })[1].ToLower(), out int id))
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                if (id > Global.maximum_items)
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<StealMenu>() == null)
                {
                    ev.ReturnMessage = Global._not_search_try_steal;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<StealMenu>().myitems == false)
                {
                    ev.ReturnMessage = Global._cannot_destoy_other_people_items;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<DestroyProccess>() != null)
                {
                    ev.ReturnMessage = Global._already_destroy + Global.items[ev.Player.gameObject.GetComponent<DestroyProccess>().curType];
                    return;
                }
                if (ev.Player.gameObject.GetComponent<CooldownDestroy>() != null)
                {
                    ev.ReturnMessage = Global._cooldown_destroy + ev.Player.gameObject.GetComponent<CooldownDestroy>().cooldown.ToString();
                    return;
                }
                if (id > ev.Player.gameObject.GetComponent<StealMenu>().itemsToSteal.Count)
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
                Dictionary<ItemType, string> stealitem = ev.Player.gameObject.GetComponent<StealMenu>().itemsToSteal[id];
                ItemType curType = new List<ItemType>(stealitem.Keys)[0];
                string curName = new List<string>(stealitem.Values)[0];
                bool hidden_inventory = false;
                if (curType == ItemType.None)
                {
                    if (curName == Global._usb079itemname && ev.Player.gameObject.GetComponent<UsbHolder>() == null)
                    {
                        ev.ReturnMessage = Global._nothaveitem;
                        return;
                    }
                    if (curName == Global._knifeitemname && ev.Player.gameObject.GetComponent<KnifeHolder>() == null)
                    {
                        ev.ReturnMessage = Global._nothaveitem;
                        return;
                    }
                    hidden_inventory = true;
                }
                else
                {
                    if (ev.Player.inventory.items.Where(x => x.id == curType).FirstOrDefault() == default)
                    {
                        ev.ReturnMessage = Global._nothaveitem;
                        return;
                    }
                }

                ev.Player.gameObject.AddComponent<DestroyProccess>();
                ev.Player.gameObject.GetComponent<DestroyProccess>().curType = curType;
                ev.Player.gameObject.GetComponent<DestroyProccess>().hidden_inventory = hidden_inventory;
                ev.ReturnMessage = Global._success_start_destroy + curName + "." + Global._dontmove + Global.time_destroy;
                return;
            }
            else if (command == "searchh")
            {
                if (ev.Player.GetTeam() == Team.SCP && ev.Player.GetRole() != RoleType.Scp049)
                {
                    ev.ReturnMessage = Global._isscp;
                    return;
                }
                if (ev.Player.gameObject.GetComponent<Search>() != null)
                {
                    ev.ReturnMessage = Global._already_search + ev.Player.gameObject.GetComponent<Search>().target.nicknameSync.Network_myNickSync;
                    return;
                }
                ReferenceHub target = null;
                if (Physics.Raycast((ev.Player.gameObject.GetComponent<Scp049PlayerScript>().plyCam.transform.forward * 1.01f) + ev.Player.gameObject.transform.position, ev.Player.gameObject.GetComponent<Scp049PlayerScript>().plyCam.transform.forward, out RaycastHit hit, Global.distance_to_searchf))
                {
                    if (hit.transform.GetComponent<QueryProcessor>() == null)
                    {
                        ev.ReturnMessage = Global._toolong;
                        return;
                    }
                    else
                    {
                        if (Player.GetPlayer(hit.transform.gameObject) == null || Player.GetPlayer(hit.transform.gameObject).GetTeam() == Team.SCP)
                        {
                            ev.ReturnMessage = Global._targetisscp;
                            return;
                        }
                        else
                        {
                            target = Player.GetPlayer(hit.transform.gameObject);
                        }

                        if (ev.Player.GetRole() == RoleType.ClassD)
                        {
                            ev.ReturnMessage = "Вы осмотрели игрока " + target.nicknameSync.Network_myNickSync + ". Его состояние здоровья вы оцениваете как " + Global.health[Global.rand.Next(GetHealthState((int)target.GetHealth(), target.playerStats.maxHP) - 1, GetHealthState((int)target.GetHealth(), target.playerStats.maxHP) + 2)];
                            return;
                        }
                        else if (ev.Player.GetRole() == RoleType.Scientist)
                        {
                            ev.ReturnMessage = "Вы осмотрели игрока " + target.nicknameSync.Network_myNickSync + ". Его состояние здоровья вы оцениваете как " + Global.health[GetHealthState((int)target.GetHealth(), target.playerStats.maxHP)] + " (" + target.GetHealth() + "/" + target.playerStats.maxHP + ")";
                            return;
                        }
                        else
                        {
                            ev.ReturnMessage = "Вы осмотрели игрока " + target.nicknameSync.Network_myNickSync + ". Его состояние здоровья вы оцениваете как " + Global.health[GetHealthState((int)target.GetHealth(), target.playerStats.maxHP)];
                            return;
                        }
                    }
                }
            }
        }
        public void OnRoundStart()
        {
            Global.can_use_commands = true;
        }
        public void OnWaitingForPlayers()
        {
            try
            {
                Global.IsFullRp = Plugin.Config.GetBool("IsFullRp");
            }
            catch (System.Exception ex)
            {
                Log.Info("Catch an exception while getting boolean value from config file: " + ex.Message);
                Global.IsFullRp = false;
            }
            Log.Info(nameof(Global.IsFullRp) + ": " + Global.IsFullRp);
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