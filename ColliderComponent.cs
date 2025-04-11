using GameCore;
using Interactables;
using LabApi.Features.Wrappers;
using PlayerRoles;
using UnityEngine;
using Random = UnityEngine.Random;
using EscapePlan;
using LabApi.Events.Handlers;
using Log = LabApi.Features.Console.Logger;

namespace EscapePlan
{
    public class EscapeComponent : MonoBehaviour
    {
        private void OnTriggerEnter(Collider collider)
        {
            if (!Player.TryGet(collider.gameObject, out var player)) return;
            RoleTypeId escapeRole;
            
            switch (player.Role)
            {
                case RoleTypeId.Scientist: escapeRole = player.IsDisarmed ? RoleTypeId.ChaosConscript : RoleTypeId.NtfSpecialist; break;
                case RoleTypeId.ClassD: escapeRole = player.IsDisarmed ? RoleTypeId.NtfPrivate : RoleTypeId.ChaosConscript; break;
                default: return;
            }

            if (escapeRole == RoleTypeId.ChaosConscript) {player.SetRole(escapeRole,RoleChangeReason.Escaped); return;}
            
            player.SetRole(escapeRole, RoleChangeReason.Escaped, RoleSpawnFlags.AssignInventory);
            player.Position = new Vector3(10, 991, Random.Range(-41, -46));
        }
    }
}

