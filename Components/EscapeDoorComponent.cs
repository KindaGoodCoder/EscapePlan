using LabApi.Features.Wrappers;
using PlayerRoles;
using UnityEngine;
// using Log = LabApi.Features.Console.Logger;

namespace EscapePlan.Components
{
    public class DoorEscapeComponent : MonoBehaviour
    //Escape handler for the Gate A Escape Door
    {
        private void OnTriggerEnter(Collider collider)
        {
            if (!Player.TryGet(collider.gameObject, out Player player)) return;
            RoleTypeId escapeRole;
            
            switch (player.Role)
            {
                case RoleTypeId.Scientist: escapeRole = player.IsDisarmed ? RoleTypeId.ChaosConscript : RoleTypeId.NtfSpecialist; break;
                case RoleTypeId.ClassD:    escapeRole = player.IsDisarmed ? RoleTypeId.NtfPrivate     : RoleTypeId.ChaosConscript;break;
                case var _ when player.IsDisarmed && Config.DetainedMilitantsEscapes.Contains(player.Role):
                    EscapePlan.MilitantEscapes.Add(player); //PlayerChangedRoleArgs.OldRole is broken. This bandaid fix adds the escaped militant player to a list. The main class checks the list
                    escapeRole = player.Team == Team.ChaosInsurgency ? Config.DetainedChaosEscapeRole : Config.DetainedFoundationEscapeRole;
                    break;
                default: return;
            }
            
            if (escapeRole == RoleTypeId.ChaosConscript) {player.SetRole(escapeRole,RoleChangeReason.Escaped); return;}
            
            player.SetRole(escapeRole, RoleChangeReason.Escaped, RoleSpawnFlags.AssignInventory);
            player.Position = EscapePlan.SurfacePosition + new Vector3(15, -9, Random.Range(-41, -46));
        }
    }
}