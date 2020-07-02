using System;
using System.Collections.Generic;

namespace BetterSearch
{
    public static class Global
    {
        public static Random rand = new Random();
        public static int maximum_items = 10;

        public static bool IsFullRp = false;

        public static float distance_to_search = 6.0f;
        public static float distance_to_searchf = 10.0f;
        public static float distance_to_search_not_move = 1f;
        public static float distance_to_steal_between = 3.5f;

        public static string _toolong = "Вы не смотрите на обыскиваемого человека, либо находитесь слишком близко к нему";

        //rework
        public static float cooldown_searchf = 30.0f;
        public static float cooldown_search = 10.0f;
        public static float time_search_to_steal = 120.0f;
        public static float cooldown_steal = 60.0f;
        public static float cooldown_destroy = 10.0f;

        public static float time_search = 8.0f;
        public static float time_steal = 2.5f;
        public static float time_destroy = 15.0f;

        public static string _success_start_search = "Вы начали просматривать вещи, которые имеет ";
        public static string _success_start_search2 = ", не двигайтесь в течении секунд: ";

        public static string _already_search = "Вы уже обыскиваете ";
        public static string _cooldown_search = "Вы сможете обыскивать через: ";


        public static Dictionary<ItemType, string> items = new Dictionary<ItemType, string>()
        {
            {ItemType.KeycardChaosInsurgency, "Устройство взлома повстанцев хаоса" },
            {ItemType.GunCOM15, "Пистолет" },
            {ItemType.KeycardContainmentEngineer, "Ключ карта инженера" },
            {ItemType.Disarmer, "Наручники" },
            {ItemType.GunE11SR, "Винтовка Epsilon-11" },
            {ItemType.KeycardFacilityManager, "Ключ карта менеджера комплекса" },
            {ItemType.GrenadeFlash, "Светошумовая граната" },
            {ItemType.Flashlight, "Фонарик" },
            {ItemType.GrenadeFrag, "Осколочная граната" },
            {ItemType.KeycardGuard, "Ключ карта охранника" },
            {ItemType.KeycardJanitor, "Ключ карта уборщика" },
            {ItemType.GunLogicer, "Пулемет" },
            {ItemType.KeycardScientistMajor, "Ключ карта старшего научного сотрудника" },
            {ItemType.Medkit, "Аптечка" },
            {ItemType.SCP018, "SCP-018" },
            {ItemType.SCP207, "SCP-207" },
            {ItemType.SCP500, "SCP-500" },
            {ItemType.SCP268, "SCP-268" },
            {ItemType.Painkillers, "Обезболивающее" },
            {ItemType.Adrenaline, "Адреналин" },
            {ItemType.MicroHID, "Micro HID" },
            {ItemType.GunMP7, "Пистолет пулемет" },
            {ItemType.KeycardNTFCommander, "Ключ карта командира МОГ" },
            {ItemType.KeycardNTFLieutenant, "Ключ карта лейтенанта МОГ" },
            {ItemType.KeycardO5, "Ключ карта члена совета О5" },
            {ItemType.GunProject90, "Пистолет пулемет" },
            {ItemType.Radio, "Рация" },
            {ItemType.KeycardScientist, "Ключ карта младшего научнего сотрудника" },
            {ItemType.KeycardSeniorGuard, "Ключ карта кадета МОГ" },
            {ItemType.GunUSP, "Пистолет" },
            {ItemType.WeaponManagerTablet, "Планшет" },
            {ItemType.KeycardZoneManager, "Ключ карта менеджера зоны" },
        };
        public static Dictionary<int, string> health = new Dictionary<int, string>()
        {
            {0, "Здоровый" },
            {1, "Здоровый" },
            {2, "Слегка ранен" },
            {3, "Ранен" },
            {4, "Сильно ранен" },
            {5, "При смерти" },
            {6, "При смерти" }
        };

        public static string _hiddenitem_try_steal = "Вы не можете украсть то, о чем не имеете представления";

        public static string hidden_item = "???";

        public static string _wrongusage = "Неправильное использование команды";
        public static string _not_search_try_steal = "Сперва обыщите цель!";

        public static string _cooldown_steal = "Вы сможете красть через: ";
        public static string _usb079itemname = "USB флешка с SCP-079";
        public static string _knifeitemname = "Заточка";
        public static string _try_steal_with_search = "Вы не можете красть пока обыскиваете";
        public static string _already_steal = "Вы уже крадете ";

        public static string _somethingwrong = "Упс! Что то пошло не так...";

        public static string _nothaveitem = "У цели уже нет этого предмета";
        public static string _success_start_steal = "Вы пытаетесь украсть ";

        public static string _isscp = "Вы не можете совершить это действие, так как вы - SCP";
        public static string _isspectator = "Вы не можете совершить это действие, так как вы - Наблюдатель";
        public static string _ishandcuff = "Вы не можете совершить это действие, так как вы связаны";

        public static string _targetisscp = "Вы не можете совершить это действие, так как цель - SCP";

        public static string _cannotstealonlyimpoud = "Вы не можете красть, только конфисковать/уничтожать (.impoud id)";

        public static string _try_impoud_with_search = "Вы не можете конфисковывать пока обыскиваете";
        public static string _cannotimpoudstealonly = "Вы не можете конфисковывать/уничтожать, только красть (.steal id)";

        public static string _nothavecuritem = "У вас нет: ";
        public static string _cannotstealandimpoud = "Вы не можете красть/конфисковывать свои же вещи, только уничтожать (.destroy id)";

        public static string _success_start_destroy = "Вы пытаетесь уничтожить ";
        public static string _dontmove = " Не двигайтесь в течении: ";

        public static string _try_destroy_with_search = "Вы не можете уничтожать пока обыскиваете";
        public static string _cannot_destoy_other_people_items = "Вы не можете уничтожать предметы других игроков с помощью этой команды";

        public static string _cooldown_destroy = "Вы сможете уничтожать через: ";
        public static string _already_destroy = "Вы уже уничтожаете ";
        internal static bool can_use_commands;
    }
}