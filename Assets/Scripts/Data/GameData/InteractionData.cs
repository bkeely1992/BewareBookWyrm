using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using static EncounterManager;

public class InteractionData
{
    public string Name => _moveName;
    private string _moveName = "";

    public string StatName => _statName;
    private string _statName = "";

    public string Identifier => _identifier;
    private string _identifier = "";

    public string TransitionIdentifier => _transitionIdentifier;
    private string _transitionIdentifier = "";

    public EncounterTransition TransitionType => _transitionType;
    private EncounterTransition _transitionType = EncounterTransition.invalid;

    public InteractionData(XElement interactionXML)
    {
        _identifier = interactionXML.Attribute("identifier") != null ? interactionXML.Attribute("identifier").Value : "";

        _statName = interactionXML.Attribute("stat_name") != null ? interactionXML.Attribute("stat_name").Value : "";
        _moveName = interactionXML.Attribute("name") != null ? interactionXML.Attribute("name").Value : "";
        _transitionIdentifier = interactionXML.Attribute("transition_identifier") != null ? interactionXML.Attribute("transition_identifier").Value : "";
        
        _transitionType = interactionXML.Attribute("transition_type") != null && Enum.TryParse<EncounterTransition>(interactionXML.Attribute("transition_type").Value, out _transitionType) ? _transitionType : EncounterTransition.invalid;

        if(_transitionType == EncounterTransition.invalid)
        {
            Debug.LogError("An invalid transition has been set for move[" + Identifier + "].");
        }
    }
}

