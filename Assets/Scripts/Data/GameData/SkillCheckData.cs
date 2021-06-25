using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static EncounterManager;

class SkillCheckData
{
    public string Identifier => _identifier;
    private string _identifier = "";

    private string _successTransitionIdentifier = "";
    private string _failTransitionIdentifier = "";

    private EncounterTransition _transition = EncounterTransition.invalid;

    public List<MoveComponentData> MoveComponents => _moveComponents;
    private List<MoveComponentData> _moveComponents = new List<MoveComponentData>();

    public SkillCheckData(XElement skillCheckXML)
    {
        _identifier = skillCheckXML.Attribute("identifier") != null ? skillCheckXML.Attribute("identifier").Value : "";

        MoveComponentData currentMoveComponent;
        foreach (XElement componentXML in skillCheckXML.Element("Components").Elements("Component"))
        {
            currentMoveComponent = new MoveComponentData(componentXML);
            _moveComponents.Add(currentMoveComponent);
        }
    }
}

