using LabApi.Features.Wrappers;
using PlayerRoles;
using UnityEngine;
// using Log = LabApi.Features.Console.Logger;

namespace EscapePlan.Components
{
    public class DefaultEscapeComponent : MonoBehaviour
    {
        //Escape Handler for Gate B (Detained militants only)
        private void OnTriggerEnter(Collider collider)
        {
            if (!Player.TryGet(collider.gameObject, out var player) || !player.IsDisarmed) return;
            
            if (EscapePlan.Instance.Config.DetainedNtfEscapes.Contains(player.Role))
            {
                player.SetRole(RoleTypeId.ChaosConscript, RoleChangeReason.Escaped, RoleSpawnFlags.AssignInventory);
                player.Position = EscapePlan.SurfacePosition + new Vector3(Random.Range(130, 120), -4, -44.3f); //MTF Spawn);
                EscapePlan.MilitantEscapes.Add(player);
            }
            else if (EscapePlan.Instance.Config.DetainedCiEscapes.Contains(player.Role))
            {
                player.SetRole(RoleTypeId.NtfSpecialist, RoleChangeReason.Escaped);
                EscapePlan.MilitantEscapes.Add(player); //Because OnPlayerChangingRole.OldRole is currently broken, add player to militant escape list for the main class to check
            }
        }
    }
}