using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static EncounterManager;

class SkillCheckStageData : EncounterStageData
{
    public string Identifier => _identifier;
    private string _identifier = "";

    public float BaseTime => _baseTime;
    private float _baseTime = 0f;

    public List<SkillCheckComponentData> SkillCheckComponents => _skillCheckComponents;
    private List<SkillCheckComponentData> _skillCheckComponents = new List<SkillCheckComponentData>();

    public string SuccessTransitionIdentifier => _successTransitionIdentifier;
    private string _successTransitionIdentifier = "";

    public SkillCheckStageData(XElement stageXML) : base(stageXML)
    {
        //Get the identifier
        _identifier = stageXML.Attribute("identifier") != null ? stageXML.Attribute("identifier").Value : "";

        _baseTime = stageXML.Attribute("base_time") != null ? float.TryParse(stageXML.Attribute("base_time").Value, out _baseTime) ? _baseTime : 0f : 0f;

        _successTransitionIdentifier = stageXML.Attribute("success_transition_identifier") != null ? stageXML.Attribute("success_transition_identifier").Value : "";

        //Get the components
        if (stageXML.Element("Components") != null)
        {
            foreach(XElement componentXML in stageXML.Element("Components").Elements("Component"))
            {
                _skillCheckComponents.Add(new SkillCheckComponentData(componentXML));
            }
        }
    }
}

