using System.Linq;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Extensions;
using LabApi.Features.Wrappers;
using MapGeneration;
using Mirror;
using PlayerRoles;
using UnityEngine;
// using Log = LabApi.Features.Console.Logger;

namespace EscapePlan
{
    public class EventsHandler : CustomEventsHandler
    {
        private static Config Config => EscapePlan.EscapePlanConfigs;
        private static GameObject _doorPrefab;
        
        private Bounds _customEscapeBounds;
        
        public override void OnServerRoundStarted()
        {
            _doorPrefab ??= NetworkClient.prefabs.Values.First(gameObject => gameObject.name == "LCZ BreakableDoor");
            
            if (!Config.EscapeArea.Enabled)
                return;
            
            EscapeAreaData escapeArea = Config.EscapeArea;
            Vector3 escapeAreaPosition = Room.Get(RoomName.Outside).First().Transform.TransformPoint(escapeArea.PositionOffset);
            
            Map.AddEscapeZone(_customEscapeBounds = new Bounds(escapeAreaPosition, escapeArea.BoundsSize));
            
            BreakableDoor door = BreakableDoor.Get(
                Object.Instantiate(_doorPrefab, escapeAreaPosition, Quaternion.Euler(escapeArea.DoorRotationOffset))
                .GetComponent<Interactables.Interobjects.BreakableDoor>()
            )!;
            door.IgnoreDamageSources = ~DoorDamageType.None;
            door.Lock(DoorLockReason.Warhead, true);
            
            NetworkServer.Spawn(door.GameObject);
        }
        
        public override void OnServerRoundRestarted() => Map.RemoveEscapeZone(_customEscapeBounds);

        public override void OnPlayerEscaped(PlayerEscapedEventArgs args)
        {
            Player player = args.Player;
        
            if (Config.EscapeesSpawnAtEscapeGate)
            {
                RoleTypeId locationSpawnRole = RoleTypeId.None;
            
                if (args.EscapeZone == Escape.DefaultEscapeZone)
                    locationSpawnRole = RoleTypeId.NtfSpecialist;
                else if (args.EscapeZone == _customEscapeBounds)
                    locationSpawnRole = RoleTypeId.ChaosConscript;
            
                if (locationSpawnRole.TryGetRandomSpawnPoint(out Vector3 spawnPoint, out _))
                    player.Position = spawnPoint;
            }

            if (args.NewRole != RoleTypeId.NtfPrivate || Config.UseDefaultEscapeeLoadout)
                return;
        
            //Private Escapees don't have a dedicated escape role with a starting inventory like Conscripts or Specialists
            Config.NtfPrivateEscapeeAdditionalItems.ForEach(item => player.AddItem(item, ItemAddReason.StartingItem));
            player.SetAmmo(ItemType.Ammo9x19, Config.NtfPrivate9mmAmmo);
            player.SetAmmo(ItemType.Ammo556x45, Config.NtfPrivate556Ammo);
        }

        public override void OnPlayerEscaping(PlayerEscapingEventArgs ev)
        {
            if (!ev.Player.IsDisarmed || !Config.DetainedMilitantsEscapees.Contains(ev.OldRole))
                return;
            
            ev.EscapeScenario = Escape.EscapeScenarioType.Custom;
            switch (ev.OldRole.GetFaction())
            {
                case Faction.FoundationStaff:
                    ev.NewRole = Config.DetainedFoundationEscapeRole;
                    break;
                case Faction.FoundationEnemy:
                    ev.NewRole = Config.DetainedChaosEscapeRole;
                    break;
                default:
                    ev.IsAllowed = false;
                    break;
            }
        }
    }
}