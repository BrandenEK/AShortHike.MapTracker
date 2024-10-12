using HarmonyLib;

namespace AShortHike.MapTracker;

/// <summary>
/// Process map input before player input
/// </summary>
[HarmonyPatch(typeof(Player), nameof(Player.Update))]
class Player_Update_Patch
{
    public static void Prefix()
    {
        Main.MapTracker.OnEarlyUpdate();
    }
}
