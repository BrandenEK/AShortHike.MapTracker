using AShortHike.ModdingAPI;
using UnityEngine;
using UnityEngine.UI;

namespace AShortHike.MapTracker;

public class MapTracker : ShortHikeMod
{
    public MapTracker() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    private RectTransform _mapObject = null;
    private RectTransform _characterObject = null;

    private Transform _canvasObject = null;

    protected override void OnInitialize()
    {
        LogHandler.Error($"{ModInfo.MOD_NAME} has been initialized");
    }

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }

    private void ToggleMap()
    {
        LogHandler.Info("Toggling map");

        if (_mapObject == null)
            CreateMap();

        _mapObject.gameObject.SetActive(!_mapObject.gameObject.activeSelf);
    }

    private void UpdatePlayerPosition()
    {

    }

    private void CreateMap()
    {
        if (_canvasObject == null)
            _canvasObject = CreateCanvas().transform;

        RectTransform borderImage = CreateImage("Border", _canvasObject, new Vector2(910, 910), null, new Color(242 / (float)255, 238 / (float)255, 203 / (float)255));
        RectTransform mapImage = CreateImage("Map", borderImage, new Vector2(900, 900), null, Color.red);
        RectTransform character = CreateImage("Character", mapImage, new Vector2(50, 50), null, Color.yellow);

        _mapObject = borderImage;
        _characterObject = character;
    }

    private RectTransform CreateImage(string name, Transform parent, Vector2 size, Sprite sprite, Color color)
    {
        RectTransform rect = new GameObject(name).AddComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.sizeDelta = size;

        Image image = rect.gameObject.AddComponent<Image>();
        image.color = color;
        image.sprite = sprite;

        return rect;
    }

    private Canvas CreateCanvas()
    {
        GameObject obj = new("Better canvas");
        obj.layer = LayerMask.NameToLayer("UI");
        obj.transform.parent = Main.TransformHolder;

        Canvas canvas = obj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = obj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.referencePixelsPerUnit = 100;

        return canvas;
    }
}