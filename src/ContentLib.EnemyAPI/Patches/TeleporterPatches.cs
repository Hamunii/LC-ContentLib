using System;

namespace ContentLib.EnemyAPI.Patches
{
    public class TeleporterPatches
    {
        private static readonly object _lock = new();
        private static TeleporterPatches _instance;
        private ShipTeleporter _shipTeleporter;

        private TeleporterPatches() { 
            On.ShipTeleporter.Awake += ShipTeleporterOnAwake;
        }

        public static TeleporterPatches Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new TeleporterPatches();
                        }
                    }
                }
                return _instance;
            }
        }

        public ShipTeleporter ShipTeleporter
        {
            get
            {
                if (_shipTeleporter == null)
                {
                    throw new InvalidOperationException("ShipTeleporter is not initialized yet.");
                }
                return _shipTeleporter;
            }
            private set
            {
                if (_shipTeleporter == null) 
                {
                    _shipTeleporter = value;
                }
            }
        }
     


        private void ShipTeleporterOnAwake(On.ShipTeleporter.orig_Awake orig, ShipTeleporter self)
        {
            orig(self); 
            ShipTeleporter = self; 
        }
    }
}