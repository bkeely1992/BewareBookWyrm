using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EncounterManager : MonoBehaviour
{
    public enum EncounterState
    {
        choose_move, perform_move
    }
    private EncounterState _state = EncounterState.choose_move;

    public Text stageDescriptionText;
    public List<EncounterMove> availableMoves = new List<EncounterMove>();
    public GameObject availableMovePrefab, chooseMoveGroupObject;
    public Transform availableMoveParent;

    public GameObject performMoveGroupObject;
    public List<MoveComponent> moveComponents = new List<MoveComponent>();

    private EncounterData _currentEncounter = null;
    private EncounterStageData _currentStage = null;
    private MoveData _activeMove = null;
    private int _activeComponentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEncounter(string inEncounterName)
    {
        _state = EncounterState.choose_move;
        _currentEncounter = GameDataManager.Instance.GetEncounterData(inEncounterName);

        if(_currentEncounter == null)
        {
            Debug.LogError("Could not load encounter["+inEncounterName+"], missing from data.");
            gameObject.SetActive(false);
            return;
        }

        //Load the first stage
        LoadStage(_currentEncounter.StartingStage);

        //Add listener for the typing method
        TypeManager.OnTyped += CheckForMatch;
    }

    public void CheckForMatch()
    {
        switch (_state)
        {
            case EncounterState.choose_move:
                CheckForChooseMoveMatch();
                break;
            case EncounterState.perform_move:
                CheckForPerformMoveMatch();

                break;
        }

        
    }

    private void CheckForChooseMoveMatch()
    {
        foreach (EncounterMove availableMove in availableMoves)
        {
            if (TypeManager.Instance.WrittenInput == availableMove.moveData.Name)
            {
                chooseMoveGroupObject.SetActive(false);
                performMoveGroupObject.SetActive(true);
                TypeManager.Instance.ClearText();

                //Load the move contents
                _activeMove = availableMove.moveData;
                for (int i = 0; i < _activeMove.MoveComponents.Count; i++)
                {
                    moveComponents[i].componentData = _activeMove.MoveComponents[i];
                    moveComponents[i].moveText.text = _activeMove.MoveComponents[i].Text;
                    moveComponents[i].moveText.color = Color.black;
                }
                moveComponents[0].moveText.color = Color.white;
                _state = EncounterState.perform_move;
                _activeComponentIndex = 0;
            }
        }
    }

    private void CheckForPerformMoveMatch()
    {
        if (TypeManager.Instance.WrittenInput == moveComponents[_activeComponentIndex].componentData.Text)
        {
            moveComponents[_activeComponentIndex].moveText.color = Color.black;
            _activeComponentIndex++;
            if(_activeComponentIndex >= moveComponents.Count)
            {
                //Switch back to choose move
                _state = EncounterState.choose_move;
                performMoveGroupObject.SetActive(false);
                chooseMoveGroupObject.SetActive(true);
                TypeManager.Instance.ClearText();

                //Load the next interaction
            }
            else
            {
                moveComponents[_activeComponentIndex].moveText.color = Color.white;
                TypeManager.Instance.ClearText();
            }
        }
        
    }

    private void LoadStage(string stageName)
    {
        _currentStage = _currentEncounter.GetStageData(stageName);
        if (_currentStage == null)
        {
            Debug.LogError("Could not load stage[" + stageName + "], missing from data.");
            gameObject.SetActive(false);
            return;
        }

        //Set the stage text
        stageDescriptionText.text = _currentStage.Description;

        foreach (EncounterMove availableMove in availableMoves)
        {
            Destroy(availableMove.gameObject);
        }
        availableMoves.Clear();

        //Set the moves
        GameObject availableMoveObject;
        EncounterMove currentEncounterMove;
        Color moveColour;
        foreach (MoveData moveData in _currentStage.AvailableMoves)
        {
            availableMoveObject = Instantiate(availableMovePrefab, availableMoveParent);
            currentEncounterMove = availableMoveObject.GetComponent<EncounterMove>();
            currentEncounterMove.moveText.text = moveData.Name;
            moveColour = GameDataManager.Instance.GetStatData(moveData.StatName).Colour;
            currentEncounterMove.moveText.color = moveColour;
            currentEncounterMove.moveData = moveData;
            availableMoves.Add(currentEncounterMove);
        }
    }
}
