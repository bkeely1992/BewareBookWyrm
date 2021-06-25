using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using static EncounterManager;

public class MoveData
{
    public string Name => _moveName;
    private string _moveName = "";

    public string StatName => _statName;
    private string _statName = "";

    public string Identifier => _identifier;
    private string _identifier = "";

    private string _successTransitionIdentifier = "";
    private string _failTransitionIdentifier = "";

    private EncounterTransition _transition = EncounterTransition.invalid;

    public MoveData(XElement moveXML)
    {
        _statName = moveXML.Attribute("stat_name") != null ? moveXML.Attribute("stat_name").Value : "";
        _moveName = moveXML.Attribute("name") != null ? moveXML.Attribute("name").Value : "";
        _successTransitionIdentifier = moveXML.Attribute("success_transition_identifier") != null ? moveXML.Attribute("success_transition_identifier").Value : "";
        _failTransitionIdentifier = moveXML.Attribute("fail_transition_identifier") != null ? moveXML.Attribute("fail_transition_identifier").Value : "";
        _identifier = moveXML.Attribute("identifier") != null ? moveXML.Attribute("identifier").Value : "";
        _transition = moveXML.Attribute("transition_type") != null && Enum.TryParse<EncounterTransition>(moveXML.Attribute("transition_type").Value, out _transition) ? _transition : EncounterTransition.invalid;

        if(_transition == EncounterTransition.invalid)
        {
            Debug.LogError("An invalid transition has been set for move[" + Identifier + "].");
        }


    }
}

