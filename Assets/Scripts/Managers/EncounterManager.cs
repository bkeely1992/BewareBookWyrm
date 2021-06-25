using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EncounterManager : MonoBehaviour
{
    public enum EncounterTransition
    {
        skill_check,
        new_moves,
        encounter_end,
        invalid
    }

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
        _currentEncounter = GameDataManager.Instance.GetEncounterData(inEncounterName);

        if(_currentEncounter == null)
        {
            Debug.LogError("Could not load encounter["+inEncounterName+"], missing from data.");
            gameObject.SetActive(false);
            return;
        }

        //Load the first stage
        LoadStage(_currentEncounter.StartingStage);

        
    }

    //public void CheckForMatch()
    //{
    //    switch (_state)
    //    {
    //        case EncounterState.choose_move:
    //            CheckForChooseMoveMatch();
    //            break;
    //        case EncounterState.perform_move:
    //            CheckForPerformMoveMatch();

    //            break;
    //    }
    //}

    //private void CheckForPerformMoveMatch()
    //{
    //    if (TypeManager.Instance.WrittenInput == moveComponents[_activeComponentIndex].componentData.Text)
    //    {
    //        moveComponents[_activeComponentIndex].moveText.color = Color.black;
    //        _activeComponentIndex++;
    //        if(_activeComponentIndex >= moveComponents.Count)
    //        {
    //            //Switch back to choose move
    //            _state = EncounterState.choose_move;
    //            performMoveGroupObject.SetActive(false);
    //            chooseMoveGroupObject.SetActive(true);
    //            TypeManager.Instance.ClearText();

    //            //Load the next interaction
    //        }
    //        else
    //        {
    //            moveComponents[_activeComponentIndex].moveText.color = Color.white;
    //            TypeManager.Instance.ClearText();
    //        }
    //    }
        
    //}

    private void LoadStage(string stageIdentifier)
    {
        _currentStage = _currentEncounter.GetStageData(stageIdentifier);
        if (_currentStage == null)
        {
            Debug.LogError("Could not load stage[" + stageIdentifier + "], missing from data.");
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

        if(_currentStage is ChooseMoveStageData)
        {
            LoadChooseMoveStage((ChooseMoveStageData)_currentStage);
        }
        else if(_currentStage is SkillCheckStageData)
        {
            LoadSkillCheckStage((SkillCheckStageData)_currentStage);
        }
    }

    private void LoadChooseMoveStage(ChooseMoveStageData stageData)
    {
        //Set the moves
        GameObject availableMoveObject;
        EncounterMove currentEncounterMove;
        foreach (MoveData moveData in _currentStage.AvailableMoves)
        {
            availableMoveObject = Instantiate(availableMovePrefab, availableMoveParent);
            currentEncounterMove = availableMoveObject.GetComponent<EncounterMove>();

            if (currentEncounterMove)
            {
                currentEncounterMove.SetMoveData(moveData);
                availableMoves.Add(currentEncounterMove);
            }
            else
            {
                Debug.LogError("Encounter move could not be found. Could not set its corresponding move data.");
            }
        }
    }

    private void LoadSkillCheckStage(SkillCheckStageData stageData)
    {

    }

    public void ChooseMove(string inIdentifier)
    {
        EncounterMove matchingEncounterMove = availableMoves.Single(m => m.Identifier == inIdentifier);
        if(matchingEncounterMove != null)
        {
            chooseMoveGroupObject.SetActive(false);
            performMoveGroupObject.SetActive(true);

            //Load the move contents
            if (matchingEncounterMove.moveData.IsSkillCheck)
            {
                _activeMove = matchingEncounterMove.moveData;
                for (int i = 0; i < _activeMove.MoveComponents.Count; i++)
                {
                    moveComponents[i].componentData = _activeMove.MoveComponents[i];
                    moveComponents[i].moveText.text = _activeMove.MoveComponents[i].Text;
                    moveComponents[i].moveText.color = Color.black;
                }
                moveComponents[0].moveText.color = Color.white;
                _activeComponentIndex = 0;
            }
            else if(matchingEncounterMove)

        }
    }
}
