using LabApi.Loader.Features.Plugins;
using System.Linq;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using MapGeneration;
using PlayerRoles;
using UnityEngine;
using Version = System.Version;

namespace EscapePlan
{
    public class EscapePlan : Plugin<Config>
    {
        public override string Name => "Escape Plan";
        public override string Description => "Adds extra functionality to the escaping mechanics";
        public override string Author => "Goodman";
        public override Version Version => new(2, 0, 0, 0);
        public override Version RequiredApiVersion => new(1, 0, 0);

        private static Vector3 _surfacePosition;
        private Bounds _newEscapeArea;

        public override void Enable()
        {
            PlayerEvents.Escaped += OnPlayerEscape;
            ServerEvents.RoundStarted += OnRoundStarted;
            ServerEvents.RoundRestarted += OnRoundRestarted;
        }

        public override void Disable()
        {
            PlayerEvents.Escaped -= OnPlayerEscape;
            ServerEvents.RoundStarted -= OnRoundStarted;
            ServerEvents.RoundRestarted -= OnRoundRestarted;
        }
        
        private void OnRoundStarted()
        {
            _surfacePosition = Room.Get(RoomName.Outside).First().Position;
            
            //If the Gate A escape door is enabled, spawn it in to handle Gate A escapes
            if (Config.EscapeArea == null)
                return;
            EscapeAreaData newEscapeArea = Config.EscapeArea;
            Map.AddEscapeZone(_newEscapeArea = new Bounds(Room.Get(newEscapeArea.EscapeRoom).First().Transform.TransformPoint(newEscapeArea.PositionOffset), Vector3.one * 4f));
        }

        private void OnRoundRestarted() => Map.RemoveEscapeZone(_newEscapeArea);

        private void OnPlayerEscape(PlayerEscapedEventArgs args)
        {
            Player player = args.Player;
            if (Config.EscapeesSpawnAtEscapeGate)
            {
                float randomZPosition = Random.Range(-40, -45);
                player.Position = _surfacePosition + 
                    (args.EscapeZone == Escape.DefaultEscapeZone ? new Vector3(130, -4, randomZPosition)
                        : new Vector3(20, -9, randomZPosition));
            }

            GiveItemsFromList(Config.RewardItems);

            if (args.NewRole == RoleTypeId.ChaosConscript) {
                player.SetAmmo(ItemType.Ammo762x39, Config.CiAmmo);
                GiveItemsFromList(Config.CiItems);
                return;
            }
            
            player.SetAmmo(ItemType.Ammo9x19, Config.Ntf9mmAmmo);
            player.SetAmmo(ItemType.Ammo556x45, Config.Ntf556Ammo);

            GiveItemsFromList(Config.NtfItems);

            void GiveItemsFromList(ItemType[] itemList) => itemList.ForEach(item => player.AddItem(item));
        }
    }
}
