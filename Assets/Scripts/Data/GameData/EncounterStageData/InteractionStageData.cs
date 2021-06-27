using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;

class InteractionStageData : EncounterStageData
{
    public List<InteractionData> AvailableInteractions => _availableInteractions;
    private List<InteractionData> _availableInteractions = new List<InteractionData>();

    public string Description => _description;
    private string _description = "";

    public InteractionStageData(XElement stageXML, Dictionary<string,InteractionData> allInteractions) : base(stageXML)
    {
        _description = stageXML.Element("Description") != null ? stageXML.Element("Description").Value : "";

        //Get the available interactions
        string interactionIdentifier = "";

        foreach(XElement availableInteractionXML in stageXML.Element("AvailableInteractions").Elements("AvailableInteraction"))
        {
            interactionIdentifier = availableInteractionXML.Attribute("identifier") != null ? availableInteractionXML.Attribute("identifier").Value : "";
            if (allInteractions.ContainsKey(interactionIdentifier))
            {
                _availableInteractions.Add(allInteractions[interactionIdentifier]);
            }
            else
            {
                Debug.LogError("Interaction["+interactionIdentifier+"] for interaction stage["+_identifier+"] has not been loaded into data. Could not use for stage.");
            }
        }
    }
}

