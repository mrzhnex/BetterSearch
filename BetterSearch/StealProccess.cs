using UnityEngine;
using USB079;
using CraftKnife;
using EXILED.Extensions;
using System.Linq;

namespace BetterSearch
{
    class StealProccess : MonoBehaviour
    {
        public ReferenceHub target;
        private ReferenceHub stealer;
        public ItemType itemtype;
        public string itemname;
        public bool hidden_inventory;
        public bool iscurrentitem;
        private float timer = 0f;
        private readonly float timeIsUp = 1.0f;
        private float progress = Global.time_steal;

        public void Start()
        {
            stealer = Player.GetPlayer(gameObject);
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
                    if (target.gameObject.GetComponent<UsbHolder>() == null)
                    {
                        stealer.ClearBroadcasts();
                        stealer.Broadcast(10, "Кража не удалась, " + target.nicknameSync.Network_myNickSync + " не имеет " + itemname, true);
                        Destroy(gameObject.GetComponent<StealProccess>());
                    }
                }
                else
                {
                    if (target.inventory.items.Where(x => x.id == itemtype).FirstOrDefault() == default)
                    {
                        stealer.ClearBroadcasts();
                        stealer.Broadcast(10, "Кража не удалась, " + target.nicknameSync.Network_myNickSync + " не имеет " + itemname, true);
                        Destroy(gameObject.GetComponent<StealProccess>());
                    }
                }
            }

            if (progress <= 0f)
            {
                if (hidden_inventory)
                {
                    if (itemname == Global._usb079itemname)
                    {
                        if (target.gameObject.GetComponent<UsbHolder>() == null)
                        {
                            stealer.ClearBroadcasts();
                            stealer.Broadcast(10, "<color=#228b22>Кража не удалась, " + target.nicknameSync.Network_myNickSync + " не имеет " + itemname + "</color>", true);
                        }
                        else
                        {
                            Destroy(target.gameObject.GetComponent<UsbHolder>());
                            gameObject.AddComponent<UsbHolder>();
                            stealer.ClearBroadcasts();
                            stealer.Broadcast(10, "<color=#ff0000>Вы украли " + itemname + "</color>", true);
                        }
                    }
                    else if (itemname == Global._knifeitemname)
                    {
                        if (target.gameObject.GetComponent<KnifeHolder>() == null)
                        {
                            stealer.ClearBroadcasts();
                            stealer.Broadcast(10, "<color=#228b22>Кража не удалась, " + target.nicknameSync.Network_myNickSync + " не имеет " + itemname + "</color>", true);
                        }
                        else
                        {
                            if (gameObject.GetComponent<KnifeHolder>() != null)
                            {
                                stealer.ClearBroadcasts();
                                stealer.Broadcast(10, "<color=#228b22>Вы не смогли украсть " + itemname + ", так как у вас уж есть " + itemname + "</color>", true);
                            }
                            else
                            {
                                Destroy(target.gameObject.GetComponent<KnifeHolder>());
                                gameObject.AddComponent<KnifeHolder>();
                                stealer.ClearBroadcasts();
                                stealer.Broadcast(10, "<color=#ff0000>Вы украли " + itemname + "</color>", true);
                            }
                        }
                    }
                }
                else
                {
                    if (target.inventory.items.Where(x => x.id == itemtype).FirstOrDefault() == default)
                    {
                        stealer.ClearBroadcasts();
                        stealer.Broadcast(10, "<color=#228b22>Кража не удалась, " + target.nicknameSync.Network_myNickSync + " не имеет " + itemname + "</color>", true);
                    }
                    else
                    {
                        if (!ChanceToStealItem(itemtype))
                        {
                            stealer.ClearBroadcasts();
                            stealer.Broadcast(10, "<color=#228b22>У вас не получилось украсть " + itemname + "</color>", true);
                            if (iscurrentitem)
                            {
                                target.ClearBroadcasts();
                                target.Broadcast(10, "<color=#ff0000>Вы чувствуете, как " + stealer.nicknameSync.Network_myNickSync + " пытался выбить у вас из рук " + itemname + "</color>", true);
                            }
                            else
                            {
                                target.ClearBroadcasts();
                                target.Broadcast(10, "<color=#228b22>Вы чувствуете странное движение в том месте, где вы храните " + itemname + "</color>", true);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < target.inventory.items.Count; i++)
                            {
                                if (target.inventory.items[i].id == itemtype)
                                    target.inventory.items.Remove(target.inventory.items[i]);
                            }

                            stealer.AddItem(itemtype);
                            stealer.ClearBroadcasts();
                            stealer.Broadcast(10, "<color=#ff0000>Вы украли " + itemname + "</color>", true);
                            if (iscurrentitem)
                            {
                                string bc = "<color=#ff0000>Игрок " + stealer.nicknameSync.Network_myNickSync + " выбил у вас из рук " + itemname;
                                if (Global.rand.Next(0, 100) == 33)
                                {
                                    bc = bc + " P.s. вы слепой долбоеб";
                                }
                                target.ClearBroadcasts();
                                target.Broadcast(10, bc + "</color>", true);
                            }
                        }
                    }
                }
                Destroy(gameObject.GetComponent<StealProccess>());
            }

            if (Vector3.Distance(gameObject.transform.position, target.GetPosition()) > Global.distance_to_steal_between)
            {
                stealer.ClearBroadcasts();
                stealer.Broadcast(10, "<color=#228b22>Кража не удалась, " + target.nicknameSync.Network_myNickSync + " слишком далеко</color>", true);
                Destroy(gameObject.GetComponent<StealProccess>());
            }
        }

        private bool ChanceToStealItem(ItemType itemType)
        {
            if (itemtype == ItemType.GunCOM15)
            {
                if (target.GetTeam() == Team.CDP)
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
                else if (target.GetRole() == RoleType.Scientist)
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
                if (target.GetTeam() == Team.CDP)
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
                else if (target.GetRole() == RoleType.Scientist)
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
                if (target.GetTeam() == Team.CDP)
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
                else if (target.GetRole() == RoleType.Scientist)
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
                if (target.GetTeam() == Team.CDP)
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
                else if (target.GetRole() == RoleType.Scientist)
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
                if (target.GetTeam() == Team.CDP)
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
                else if (target.GetRole() == RoleType.Scientist)
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
                if (target.GetTeam() == Team.CDP)
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
                else if (target.GetRole() == RoleType.Scientist)
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
                if (target.GetTeam() == Team.CDP)
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
                else if (target.GetRole() == RoleType.Scientist)
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
                if (target.GetTeam() == Team.CDP)
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
                else if (target.GetRole() == RoleType.Scientist)
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
                if (target.GetTeam() == Team.CDP)
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
                else if (target.GetRole() == RoleType.Scientist)
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
                if (target.GetTeam() == Team.CDP)
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
                else if (target.GetRole() == RoleType.Scientist)
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
                if (target.GetTeam() == Team.CDP)
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
                else if (target.GetRole() == RoleType.Scientist)
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
