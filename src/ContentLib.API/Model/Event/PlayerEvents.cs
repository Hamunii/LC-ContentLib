using ContentLib.API.Model.Entity.Player;

namespace ContentLib.API.Model.Event;


    public interface IPlayerEvent : IGameEvent
    {
        IPlayer Player { get; }
    }

    public abstract class PlayerSpawnEvent : IPlayerEvent
    {
        public bool IsCancelled { get; set; }


        public abstract IPlayer Player { get; }
    }

    public abstract class PlayerJumpEvent : IPlayerEvent
    {
        public abstract IPlayer Player { get; }

        public bool IsCancelled { get; set; }
    }
