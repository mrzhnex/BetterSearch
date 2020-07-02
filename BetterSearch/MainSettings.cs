using EXILED;

namespace BetterSearch
{
    public class MainSettings : Plugin
    {
        public override string getName => "BetterSearch";
        private SetEvents SetEvents;

        public override void OnEnable()
        {
            SetEvents = new SetEvents();
            Events.RoundStartEvent += SetEvents.OnRoundStart;
            Events.WaitingForPlayersEvent += SetEvents.OnWaitingForPlayers;
            Events.ConsoleCommandEvent += SetEvents.OnCallCommand;
            Log.Info(getName + " on");
        }

        public override void OnDisable()
        {
            Events.RoundStartEvent -= SetEvents.OnRoundStart;
            Events.WaitingForPlayersEvent -= SetEvents.OnWaitingForPlayers;
            Events.ConsoleCommandEvent -= SetEvents.OnCallCommand;
            Log.Info(getName + " off");
        }

        public override void OnReload() { }
    }
}