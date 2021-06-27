using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;

public class EncounterData
{
    public string Name => _name;
    private string _name = "";

    public string StartingStage => _startingStage;
    private string _startingStage = "";

    private Dictionary<string, EncounterStageData> _stageMap = new Dictionary<string, EncounterStageData>();
    private Dictionary<string, InteractionData> _allInteractions = new Dictionary<string, InteractionData>();

    public EncounterData(XElement encounterXML)
    {
        _name = encounterXML.Attribute("name") != null ? encounterXML.Attribute("name").Value : "";
        _startingStage = encounterXML.Attribute("starting_stage") != null ? encounterXML.Attribute("starting_stage").Value : "";

        InteractionData currentInteraction;
        foreach(XElement interactionXML in encounterXML.Element("Interactions").Elements("Interaction"))
        {
            currentInteraction = new InteractionData(interactionXML);
            if (_allInteractions.ContainsKey(currentInteraction.Identifier))
            {
                Debug.LogError("Interaction["+currentInteraction.Identifier+"] has already been added to the interaction list. Skipping.");
                continue;
            }
            else
            {
                _allInteractions.Add(currentInteraction.Identifier, currentInteraction);
            }
        }

        EncounterStageData stageData;
        foreach(XElement stageXML in encounterXML.Element("Stages").Element("InteractionStages").Elements("InteractionStage"))
        {
            stageData = new InteractionStageData(stageXML, _allInteractions);
            _stageMap.Add(stageData.Identifier, stageData);
        }
        foreach (XElement stageXML in encounterXML.Element("Stages").Element("SkillCheckStages").Elements("SkillCheckStage"))
        {
            stageData = new SkillCheckStageData(stageXML);
            _stageMap.Add(stageData.Identifier, stageData);
        }
    }

    public EncounterStageData GetStageData(string inStageIdentifier)
    {
        return _stageMap.ContainsKey(inStageIdentifier) ? _stageMap[inStageIdentifier] : null;
    }
}

