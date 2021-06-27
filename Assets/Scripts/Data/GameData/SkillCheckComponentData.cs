using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static EncounterManager;

public class SkillCheckComponentData
{
    public string Text => _text;
    private string _text = "";

    public string FailTransitionIdentifier => _failTransitionIdentifier;
    private string _failTransitionIdentifier = "";

    public SkillCheckComponentData(XElement componentXML)
    {
        _text = componentXML.Value.Replace("\n","");

        _failTransitionIdentifier = componentXML.Attribute("fail_transition_identifier") != null ? componentXML.Attribute("fail_transition_identifier").Value : "";
    }
}

