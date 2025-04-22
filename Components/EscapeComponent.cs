using LabApi.Features.Wrappers;
using PlayerRoles;
using UnityEngine;
// using Log = LabApi.Features.Console.Logger;

namespace EscapePlan.Components
{
    public class DefaultEscapeComponent : MonoBehaviour
    {
        private void OnTriggerEnter(Collider collider)
        {
            //Gate B escape handler. Will spawn Chaos Conscripts at Gate B and handle detained militant escapes
            if (!Player.TryGet(collider.gameObject, out Player player)) return;

            if (player.Role == RoleTypeId.ClassD || (player.Role == RoleTypeId.Scientist && player.IsDisarmed))
            {
                if (!EscapePlan.GateBEscapes.Contains(player)) EscapePlan.GateBEscapes.Add(player);
                return;
            }
            
            if (!Config.DetainedMilitantsEscapes.Contains(player.Role) || !player.IsDisarmed) return;
            
            EscapePlan.MilitantEscapes.Add(player);
            if (player.Team == Team.ChaosInsurgency) {player.SetRole(Config.DetainedChaosEscapeRole, RoleChangeReason.Escaped); return;}

            EscapePlan.GateBEscapes.Add(player);
            player.SetRole(Config.DetainedFoundationEscapeRole, RoleChangeReason.Escaped, RoleSpawnFlags.AssignInventory);
        }
    }
}