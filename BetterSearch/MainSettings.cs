using Exiled.API.Features;

namespace BetterSearch
{
    public class MainSettings : Plugin<Config>
    {
        public override string Name => nameof(BetterSearch);
        public SetEvents SetEvents { get; set; }

        public override void OnEnabled()
        {
            Global.IsFullRp = Config.IsFullRp;
            Log.Info(nameof(Global.IsFullRp) + ": " + Global.IsFullRp);
            SetEvents = new SetEvents();
            Exiled.Events.Handlers.Server.RoundStarted += SetEvents.OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers += SetEvents.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.SendingConsoleCommand += SetEvents.OnSendingConsoleCommand;
            Log.Info(Name + " on");
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= SetEvents.OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= SetEvents.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= SetEvents.OnSendingConsoleCommand;
            Log.Info(Name + " off");
        }
    }
}