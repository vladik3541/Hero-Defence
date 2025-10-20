using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private Button[] unitButtons;
    [SerializeField] private GameObject _mouseIndicator, _cellIndicator;

    [SerializeField] private InputManager _InputManager;
    [SerializeField] private Grid grid;

    [SerializeField] private UnitDataBase _DataBase;

    [SerializeField] private GameObject gridVisualization;
    
    [SerializeField] private AudioClip allowPlacement;
    [SerializeField] private AudioClip dontAllowPlacement;
    [SerializeField] private Vector3 offsetUnit;
    private int _selectObject = -1;
    private List<ObjectData> _currentRace = new List<ObjectData>();
    private AudioSource _audioSource;
    private GridData floorData, furnitureData;
    private Renderer previewRenderer;

    private List<GameObject> placedGameObject = new ();

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        StopPlacement();
        floorData = new GridData();
        furnitureData = new GridData();
        previewRenderer = _cellIndicator.GetComponentInChildren<Renderer>();

        switch (RaceType.human)
        {
            case RaceType.human:
                _currentRace = _DataBase.humans;
                break;
            case RaceType.orc:
                _currentRace = _DataBase.orcs;
                break;
            case RaceType.undead:
                _currentRace = _DataBase.undeads;
                break;
        }

        for (int i = 0; i < unitButtons.Length; i++)
        { 
            int index = i;
            unitButtons[i].onClick.AddListener(() => StartPlacement(index));
        }
    }
    
    private void Update()
    {
        Vector3? mousePosNullable = _InputManager.GetSelectMapPosition();
        if (mousePosNullable == null)
        {
            _mouseIndicator.SetActive(false);
            _cellIndicator.SetActive(false);
            return;
        }

        // Не перевіряємо валідність, якщо об'єкт не вибрано
        if (_selectObject < 0 || _selectObject >= _currentRace.Count)
        {
            _mouseIndicator.SetActive(false);
            _cellIndicator.SetActive(false);
            return;
        }

        _mouseIndicator.SetActive(true);
        _cellIndicator.SetActive(true);

        Vector3 mousePos = mousePosNullable.Value;
        Vector3Int gridPosition = grid.WorldToCell(mousePos);

        bool placementValidity = CheckPlacementValidity(gridPosition, _selectObject);
        previewRenderer.material.color = placementValidity ? Color.green : Color.red;

        _mouseIndicator.transform.position = mousePos;
        _cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
    
    public void StartPlacement(int index)
    {
        StopPlacement();
        _selectObject = index;
        if(_selectObject < 0 || _selectObject >= _currentRace.Count)
        {
            Debug.Log($"No found {index}");
            return;
        }
        gridVisualization.SetActive(true);
        _cellIndicator.SetActive(true);
        _mouseIndicator.SetActive(true);
        _InputManager.OnClicked += PlaceStructure;
        _InputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (_InputManager.IsPointerOverUI())
        {
            return;
        }

        Vector3? mousePos = _InputManager.GetSelectMapPosition();
        if (mousePos == null)
        {
            // Не попали по потрібному колайдеру - не розміщуємо
            _audioSource.clip = dontAllowPlacement;
            _audioSource.Play();
            return;
        }
        
        Vector3Int gridPosition = grid.WorldToCell((Vector3)mousePos);

        bool placementValidity = CheckPlacementValidity(gridPosition, _selectObject);

        if (placementValidity == false)
        {
            _audioSource.clip = dontAllowPlacement;
            _audioSource.Play();
            return;
        }

        // Тільки якщо все ОК - розміщуємо і грає успішний звук
        _audioSource.clip = allowPlacement;
        _audioSource.Play();

        GameObject newObject = Instantiate(_currentRace[_selectObject].Prefab);
        Vector3 worldPos = grid.CellToWorld(gridPosition) + offsetUnit;
        newObject.transform.position = worldPos;

        placedGameObject.Add(newObject);

        GridData selectedData = _currentRace[_selectObject].ID == 0 ?
            floorData :
            furnitureData;
        selectedData.AddObjectAt(gridPosition,
            _currentRace[_selectObject].Size,
            _currentRace[_selectObject].ID,
            placedGameObject.Count - 1);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = _currentRace[selectedObjectIndex].ID == 0 ? 
            floorData : 
            furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, _currentRace[selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        _selectObject = -1;
        gridVisualization.SetActive(false);
        _cellIndicator.SetActive(false);
        _mouseIndicator.SetActive(false);
        _InputManager.OnClicked -= PlaceStructure;
        _InputManager.OnExit -= StopPlacement;
    }
}