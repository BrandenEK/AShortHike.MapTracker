using AShortHike.ModdingAPI;

namespace AShortHike.MapTracker;

public class MapTracker : ShortHikeMod
{
    public MapTracker() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    protected override void OnInitialize()
    {
        LogHandler.Error($"{ModInfo.MOD_NAME} has been initialized");
    }
}