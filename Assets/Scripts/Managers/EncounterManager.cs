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
        new_interactions,
        encounter_end,
        invalid
    }

    public Text stageDescriptionText;
    public List<EncounterInteraction> availableMoves = new List<EncounterInteraction>();
    public GameObject availableMovePrefab, chooseMoveGroupObject;
    public Transform availableMoveParent;

    public GameObject performMoveGroupObject;
    public List<SkillCheckComponent> skillCheckComponents = new List<SkillCheckComponent>();

    private EncounterData _currentEncounter = null;
    private EncounterStageData _currentStage = null;
    private InteractionData _activeInteraction = null;
    private int _activeComponentIndex = 0;

    private Dictionary<string, SkillCheckStageData> _transitionSkillChecks = new Dictionary<string, SkillCheckStageData>();
    private Dictionary<string, InteractionStageData> _transitionInteractions = new Dictionary<string, InteractionStageData>();

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

    private void LoadStage(string stageIdentifier)
    {
        _currentStage = _currentEncounter.GetStageData(stageIdentifier);
        if (_currentStage == null)
        {
            Debug.LogError("Could not load stage[" + stageIdentifier + "], missing from data.");
            gameObject.SetActive(false);
            return;
        }

        foreach (EncounterInteraction availableMove in availableMoves)
        {
            Destroy(availableMove.gameObject);
        }
        availableMoves.Clear();

        if(_currentStage is InteractionStageData)
        {
            LoadInteractionStage((InteractionStageData)_currentStage);
        }
        else if(_currentStage is SkillCheckStageData)
        {
            LoadSkillCheckStage((SkillCheckStageData)_currentStage);
        }
    }

    private void LoadInteractionStage(InteractionStageData stageData)
    {
        //Set the stage text
        stageDescriptionText.text = stageData.Description;

        //Set the moves
        GameObject availableMoveObject;
        EncounterInteraction currentEncounterMove;
        foreach (InteractionData interactionData in stageData.AvailableInteractions)
        {
            availableMoveObject = Instantiate(availableMovePrefab, availableMoveParent);
            currentEncounterMove = availableMoveObject.GetComponent<EncounterInteraction>();

            if (currentEncounterMove)
            {
                currentEncounterMove.SetInteractionData(interactionData);
                availableMoves.Add(currentEncounterMove);
            }
            else
            {
                Debug.LogError("Encounter move could not be found. Could not set its corresponding data.");
            }
        }
    }

    private void LoadSkillCheckStage(SkillCheckStageData stageData)
    {
        for (int i = 0; i < stageData.SkillCheckComponents.Count; i++)
        {
            skillCheckComponents[i].componentData = stageData.SkillCheckComponents[i];
            skillCheckComponents[i].moveText.text = stageData.SkillCheckComponents[i].Text;
            skillCheckComponents[i].moveText.color = Color.black;
        }
        skillCheckComponents[0].moveText.color = Color.white;
        _activeComponentIndex = 0;
    }

    public void ChooseInteraction(string inIdentifier)
    {
        EncounterInteraction matchingEncounterMove = availableMoves.Single(m => m.Identifier == inIdentifier);
        if(matchingEncounterMove != null)
        {
            chooseMoveGroupObject.SetActive(false);
            performMoveGroupObject.SetActive(true);
            _activeInteraction = matchingEncounterMove.interactionData;

            //Load the move contents
            switch (_activeInteraction.TransitionType)
            {
                case EncounterTransition.new_interactions:
                    InteractionStageData transitionInteraction = _transitionInteractions.ContainsKey(_activeInteraction.TransitionIdentifier) ? _transitionInteractions[_activeInteraction.TransitionIdentifier] : null;

                    if(transitionInteraction != null)
                    {
                        LoadInteractionStage(transitionInteraction);
                    }
                    else
                    {
                        //Error
                        Debug.LogError("Interaction transition[" + _activeInteraction.TransitionIdentifier + "] for move[" + _activeInteraction.Identifier + "] is not active in the data. Cannot transition from current interaction.");
                        return;
                    }

                    break;
                case EncounterTransition.skill_check:

                    SkillCheckStageData transitionSkillCheck = _transitionSkillChecks.ContainsKey(_activeInteraction.TransitionIdentifier) ? _transitionSkillChecks[_activeInteraction.TransitionIdentifier] : null;

                    if(transitionSkillCheck != null)
                    {
                        LoadSkillCheckStage(transitionSkillCheck);
                    }
                    else
                    {
                        //Error
                        Debug.LogError("Skill check transition[" + _activeInteraction.TransitionIdentifier + "] for move[" + _activeInteraction.Identifier + "] is not active in the data. Cannot transition from current interaction.");
                        return;
                    }


                    break;
                case EncounterTransition.encounter_end:
                    //Return to the map
                    Debug.Log("Return to the map.");
                    break;
            }
        }
    }
}
