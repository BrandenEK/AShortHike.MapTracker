using AShortHike.ModdingAPI;
using AShortHike.ModdingAPI.Files;
using QuickUnityTools.Input;
using UnityEngine;
using UnityEngine.UI;

namespace AShortHike.MapTracker;

public class MapTracker : ShortHikeMod
{
    public MapTracker() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    private Sprite _mapImage, _characterImage;

    protected override void OnInitialize()
    {
        FileHandler.LoadDataAsSprite("map.png", out _mapImage);
        FileHandler.LoadDataAsSprite("character.png", out _characterImage);
    }

    public void OnEarlyUpdate()
    {
        PlayerInput.obeysPriority = false;
        if (Input.GetKeyDown(KeyCode.M) || PlayerInput.rightBumper.isPressed && PlayerInput.button1.ConsumePress())
        {
            ToggleMap();
        }
        PlayerInput.obeysPriority = true;
    }

    protected override void OnUpdate()
    {
        if (Map.gameObject.activeSelf)
        {
            UpdatePlayerPosition();
        }
    }

    private void ToggleMap()
    {
        LogHandler.Info("Toggling map");

        Map.gameObject.SetActive(!Map.gameObject.activeSelf);
    }

    private void UpdatePlayerPosition()
    {
        RectTransform parent = Character.parent as RectTransform;

        Vector3 playerPosition = PlayerPosition;
        Vector2 normalizedPosition = new Vector2(NormalizePoint(playerPosition.x, MAP_XBOUNDS), NormalizePoint(playerPosition.z, MAP_YBOUNDS));
        Vector2 scaledPosition = new Vector2(normalizedPosition.x * parent.sizeDelta.x, normalizedPosition.y * parent.sizeDelta.y);
        Vector2 offsetPosition = new Vector2(scaledPosition.x - parent.sizeDelta.x / 2, scaledPosition.y - parent.sizeDelta.y / 2);

        Character.anchoredPosition = offsetPosition;
    }

    private float NormalizePoint(float position, Vector2 bounds)
    {
        return Mathf.Clamp01((position - bounds.x) / (bounds.y - bounds.x));
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

    private GameUserInput x_playerInput;
    private GameUserInput PlayerInput
    {
        get
        {
            if (x_playerInput == null)
                x_playerInput = GameObject.Find("PlayerInput")?.GetComponent<GameUserInput>();
            return x_playerInput;
        }
    }

    private RectTransform x_character;
    private RectTransform Character
    {
        get
        {
            if (x_character != null)
                return x_character;

            RectTransform character = CreateImage("Character", Map.GetChild(0), new Vector2(50, 50), _characterImage, Color.white);

            LogHandler.Warning("Created new character object");
            return x_character = character;
        }
    }

    private RectTransform x_map;
    private RectTransform Map
    {
        get
        {
            if (x_map != null)
                return x_map;

            float height = Screen.height * 0.9f;
            float width = (_mapImage?.rect.width ?? 100) * height / (_mapImage?.rect.height ?? 100);

            RectTransform border = CreateImage("Border", Canvas, new Vector2(width + 10, height + 10), null, new Color(242 / (float)255, 238 / (float)255, 203 / (float)255));
            CreateImage("Map", border, new Vector2(width, height), _mapImage, Color.white);
            border.gameObject.SetActive(false);

            LogHandler.Warning("Created new map object");
            return x_map = border;
        }
    }

    private Transform x_canvas;
    private Transform Canvas
    {
        get
        {
            if (x_canvas != null)
                return x_canvas;

            GameObject obj = new("Better canvas");
            obj.layer = LayerMask.NameToLayer("UI");
            obj.transform.parent = Main.TransformHolder;

            Canvas canvas = obj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler scaler = obj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.referencePixelsPerUnit = 100;

            LogHandler.Warning("Created new canvas object");
            return x_canvas = canvas.transform;
        }
    }

    private Vector3 PlayerPosition => Singleton<GameServiceLocator>.instance.levelController.player.gameObject.transform.position;

    private static readonly Vector2 MAP_XBOUNDS = new(-150, 1100);
    private static readonly Vector2 MAP_YBOUNDS = new(-100, 1500);
}