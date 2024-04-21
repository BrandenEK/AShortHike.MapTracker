using BepInEx;

namespace AShortHike.MapTracker;

[BepInPlugin(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_VERSION)]
[BepInDependency("AShortHike.ModdingAPI", "1.0.0")]
public class Main : BaseUnityPlugin
{
    public static MapTracker MapTracker { get; private set; }

    private void Awake()
    {
        MapTracker = new MapTracker();
    }
}
