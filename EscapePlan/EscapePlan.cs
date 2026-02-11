using System;
using System.Collections.Generic;
using InventorySystem;
using InventorySystem.Configs;
using LabApi.Events.CustomHandlers;
using LabApi.Loader.Features.Plugins;
using PlayerRoles;

namespace EscapePlan
{
    public sealed class EscapePlan : Plugin<Config>
    {
        public override string Name => "EscapePlan";
        public override string Description => "Tweaks and modifications to the civilian escape mechanics";
        public override string Author => "Goodman";
        public override Version Version =>  new Version(1, 0, 0);
        public override Version RequiredApiVersion => new Version(1, 1, 0);

        public static Config EscapePlanConfigs { get; private set; }
        private CustomEventsHandler _customEventsHandler;
    
        private static readonly InventoryRoleInfo ConscriptDefaultLoadout;
        private static readonly InventoryRoleInfo SpecialistDefaultLoadout;

        static EscapePlan()
        {
            ConscriptDefaultLoadout = StartingInventories.DefinedInventories[RoleTypeId.ChaosConscript];
            SpecialistDefaultLoadout = StartingInventories.DefinedInventories[RoleTypeId.NtfSpecialist];
        }

        public override void Enable()
        {
            EscapePlanConfigs = Config;
            
            CustomHandlersManager.RegisterEventsHandler(_customEventsHandler = new EventsHandler());
            
            if (Config.UseDefaultEscapeeLoadout)
                return;
        
            StartingInventories.DefinedInventories[RoleTypeId.ChaosConscript] = 
                new InventoryRoleInfo(Config.CiConscriptLoadout,
                    new Dictionary<ItemType, ushort> { {ItemType.Ammo762x39, Config.Ci762Ammo} }
                );
        
            StartingInventories.DefinedInventories[RoleTypeId.NtfSpecialist] =
                new InventoryRoleInfo(Config.NtfSpecialistLoadout,
                    new Dictionary<ItemType, ushort>
                    {
                        { ItemType.Ammo556x45, Config.NtfSpecialist556Ammo },
                        { ItemType.Ammo9x19, Config.NtfSpecialist9mmAmmo }
                    }
                );
            
            LabApi.Features.Console.Logger.Debug("EscapePlan Plugin Enabled");
        }

        public override void Disable()
        {
            CustomHandlersManager.UnregisterEventsHandler(_customEventsHandler);
            _customEventsHandler = null;
            EscapePlanConfigs = null;
            
            StartingInventories.DefinedInventories[RoleTypeId.ChaosConscript] = ConscriptDefaultLoadout;
            StartingInventories.DefinedInventories[RoleTypeId.NtfSpecialist] = SpecialistDefaultLoadout;
        }
    }
}