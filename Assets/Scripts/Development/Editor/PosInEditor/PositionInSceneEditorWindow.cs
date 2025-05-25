using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class PositionsModel
{
    // Editor prefs для всех проектов один, поэтому надо создавать отдельный ключ для каждого проекта
    // сейчас используется PlayerSettings.productName, если поменять название проекта, то позиции сбросятся
    // тогда в коде заменить PlayerSettings.productName на PROJECT_NAME и дать константе прошлое имя 
    private static readonly string SAVE_KEY = PlayerSettings.productName + SAVE_KEY_POSTFIX;
    private const string PROJECT_NAME = "";
    private const string SAVE_KEY_POSTFIX = "PositionInScene";

    public event System.Action<ScenePositions> OnRewriteData;

    public ScenesPositions data;

    public PositionsModel()
    {
        LoadData();
    }

    public bool TryGetFirst(out ScenePositions scenePositions)
    {
        scenePositions = null;

        if (data != null && data.IsValid)
        {
            scenePositions = data.forScenes[0];
            return true;
        }
        return false;
    }
    public void AddData(ScenePositions scenePosition, PositionInScene item)
    {
        var data = this.data.forScenes.First((x) => x.sceneName == scenePosition.sceneName);

        if (data.positionsInScene == null)
            data.positionsInScene = new();

        data.positionsInScene.Add(item);

        RewriteData(data);
    }
    public void AddData(ScenePositions scenePosition)
    {
        if (data == null)
            data = new();
        if (data.forScenes == null)
            data.forScenes = new();

        data.forScenes.Add(scenePosition);

        RewriteData(scenePosition);
    }
    public void Remove(string scenePosition, PositionInScene item)
    {
        var data = this.data.forScenes.First((x) => x.sceneName == scenePosition);

        data.positionsInScene.Remove(item);

        RewriteData(data);
    }
    public void Remove(ScenePositions scenePosition)
    {
        data.forScenes.Remove(scenePosition);

        RewriteData(scenePosition);
    }
    public IEnumerable<ScenePositions> Get()
    {
        if (data != null && data.forScenes != null)
            foreach (var item in data.forScenes)
            {
                yield return item;
            }
    }

    private void RewriteData(ScenePositions data)
    {
        var json = JsonUtility.ToJson(this.data);

        EditorPrefs.SetString(SAVE_KEY, json);

        OnRewriteData?.Invoke(data);
    }
    public void LoadData()
    {
        // Clear All PlayerPrefs не чистит EditorPrefs
        var jsonString = EditorPrefs.GetString(SAVE_KEY);
        data = JsonUtility.FromJson<ScenesPositions>(jsonString);
    }
}

public class PositionInSceneEditorWindow : EditorWindow
{
    private EnumField modeField;
    private TextField goNameField;
    private TextField descriptionField;
    private ScrollView positionsScrollView;
    private Box positionItemsParent;

    private PositionsModel model;


    [MenuItem("Window/My/Position In Scene Editor Window")]
    public static void ShowMyEditor()
    {
        EditorWindow wnd = GetWindow<PositionInSceneEditorWindow>();
        wnd.titleContent = new GUIContent("Position In Scene");
    }
    private void OnEnable()
    {
        // ВАЖНО! модель инициализируется не в объявлениях полей, т.к. EditorWindow - ScriptableObject,
        // а ScriptableObject инициализируется в отдельном потоке, а модель связана с юнити api,
        // который нельзя вызывать не в основном потоке
        model = new();
        model.OnRewriteData += RebuildSceneItems;
        model.OnRewriteData += RebuildPositionsItem;

        model.LoadData();

        RebuildSceneItems(null);

        if (model.TryGetFirst(out var scenePositions))
        {
            RebuildPositionsItem(scenePositions);
        }
    }
    private void OnDisable()
    {
        model.OnRewriteData -= RebuildSceneItems;
        model.OnRewriteData -= RebuildPositionsItem;
    }
    private void OnGUI()
    {
        if (modeField == null)
        {
            return;
        }
        var value = (PositionInSceneMode)modeField.value;

        goNameField.visible = value != PositionInSceneMode.Position;
    }
    private void RebuildPositionsItem(ScenePositions positionsInScene)
    {
        positionsScrollView.Clear();

        var newItemLabel = new Label($"{positionsInScene.sceneName} Items");
        newItemLabel.style.alignSelf = Align.Center;
        newItemLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        positionsScrollView.Add(newItemLabel);

        if (positionsInScene.IsValid)
            foreach (var posItem in positionsInScene.positionsInScene)
                SetupMoveItem(positionsScrollView, positionsInScene, posItem);

        SetupAddNewPositionItem(positionsScrollView, positionsInScene);

        positionItemsParent.Add(positionsScrollView);
    }
    private void RebuildSceneItems(ScenePositions _)
    {
        var root = rootVisualElement;
        root.style.flexDirection = FlexDirection.Row;
        root.Clear();

        var itemsListBox = new Box();
        itemsListBox.style.flexGrow = 1f;
        itemsListBox.style.flexShrink = 0f;
        itemsListBox.style.flexBasis = 0f;
        itemsListBox.style.flexDirection = FlexDirection.Column;

        positionItemsParent = new Box();
        positionItemsParent.style.flexGrow = 3f;
        positionItemsParent.style.flexShrink = 0f;
        positionItemsParent.style.flexBasis = 0f;


        positionsScrollView = new ScrollView(ScrollViewMode.Vertical);
        //scroll.contentContainer.style.flexDirection = FlexDirection.Row;
        //scroll.contentContainer.style.flexWrap = Wrap.Wrap;
        //scroll.style.width = 250;
        //scroll.style.height = 400;


        model.LoadData();

        SetupSceneHeader(itemsListBox);

        foreach (var sceneItem in model.Get())
        {
            CreateSceneItem(itemsListBox, sceneItem);
        }
        CreateAddNewSceneItem(itemsListBox);

        root.Add(itemsListBox);
        root.Add(positionItemsParent);
    }
    private void CreateAddNewSceneItem(VisualElement parent)
    {
        for (int i = 0; i < 3; i++)
        {
            var space = new Label();
            parent.Add(space);
        }

        var title = new Label("Scene Name: ");
        parent.Add(title);

        var sceneNameField = new TextField();
        parent.Add(sceneNameField);

        var saveSceneItemButton = new Button();
        saveSceneItemButton.text = "Create New Scene Item";
        saveSceneItemButton.clicked += () =>
        {
            if (string.IsNullOrWhiteSpace(sceneNameField.value) == false)
            {
                var newSceneItem = new ScenePositions()
                {
                    sceneName = sceneNameField.value,
                };
                model.AddData(newSceneItem);
            }
        };

        parent.Add(saveSceneItemButton);
    }
    private void SetupMoveItem(VisualElement parent, ScenePositions scenePositions, PositionInScene data)
    {
        var descriptionText = data.mode == PositionInSceneMode.Position
            ? "Point :"
            : "GameObject: ";

        var descriptionField = new TextField(descriptionText);
        descriptionField.value = data.description;
        parent.Add(descriptionField);

        var itemElement = new VisualElement();
        itemElement.style.flexDirection = FlexDirection.Row;
        itemElement.focusable = true;

        var saveItemButton = new Button
        {
            text = "Move"
        };
        saveItemButton.style.flexGrow = 1f;
        if (data.mode == PositionInSceneMode.Position)
        {
            saveItemButton.clicked += () =>
            {
                SceneView.lastActiveSceneView.LookAt(data.position, data.rotation, 0);
            };
        }
        else
        {
            saveItemButton.clicked += () =>
            {
                var go = GameObject.Find(data.gameobjectName);
                if (!go)
                    Debug.LogError($"GameObject с именем {data.description} не найден в этой сцене!");
                else
                {
                    var newSize = 5f;
                    if (go.TryGetComponent(out MeshRenderer renderer))
                    {
                        newSize = renderer.bounds.size.magnitude / 2;
                    }
                    SceneView.lastActiveSceneView.LookAt(go.transform.position,
                        Quaternion.Euler(30, 0, 0), newSize);
                }
            };
        }

        itemElement.Add(saveItemButton);


        var remove = new Button();
        remove.text = "-";
        remove.clicked += () =>
        {
            model.Remove(scenePositions.sceneName, data);
        };
        itemElement.Add(remove);


        parent.Add(itemElement);
        parent.Add(new Label());
        parent.Add(new Label());
    }

    private void SetupAddNewPositionItem(VisualElement parent, ScenePositions scenePositions)
    {
        var newItemLabel = new Label("Create New Item");
        newItemLabel.style.alignSelf = Align.Center;
        newItemLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        parent.Add(newItemLabel);

        descriptionField = new TextField("Description: ");
        modeField = new EnumField("Mode: ", PositionInSceneMode.Position);
        goNameField = new TextField("GameObject name: ");
        parent.Add(descriptionField);
        parent.Add(modeField);
        parent.Add(goNameField);

        var saveItemButton = new Button();
        saveItemButton.text = "Save";
        saveItemButton.clicked += () =>
        {
            if (string.IsNullOrWhiteSpace(descriptionField.value) == false)
            {
                var newPosItem = new PositionInScene()
                {
                    description = descriptionField.value,
                    mode = (PositionInSceneMode)modeField.value,
                    position = SceneView.lastActiveSceneView.camera.transform.position,
                    rotation = SceneView.lastActiveSceneView.camera.transform.rotation,
                    gameobjectName = goNameField.value
                };
                model.AddData(scenePositions, newPosItem);
            }
        };

        parent.Add(saveItemButton);
    }

    private void SetupSceneHeader(Box parent)
    {
        var listLabel = new Label("Scenes");
        listLabel.style.alignSelf = Align.Center;
        listLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        parent.Add(listLabel);
    }

    private void CreateSceneItem(VisualElement parent, ScenePositions itemData)
    {
        var itemElement = new VisualElement();
        itemElement.style.flexDirection = FlexDirection.Row;
        itemElement.focusable = true;

        var remove = new Button();
        remove.text = "-";
        remove.clicked += () =>
        {
            model.Remove(itemData);
        };
        itemElement.Add(remove);

        var nameButton = new Button();
        nameButton.text = itemData.sceneName;
        nameButton.style.flexGrow = 1f;
        nameButton.clicked += () =>
        {
            RebuildPositionsItem(itemData);
        };
        itemElement.Add(nameButton);

        parent.Add(itemElement);
    }
}