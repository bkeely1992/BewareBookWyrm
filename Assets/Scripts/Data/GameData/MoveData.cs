using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

public class MoveData
{
    public string Name => _moveName;
    private string _moveName = "";

    public string StatName => _statName;
    private string _statName = "";
    
    private string _successStage = "";
    private string _failStage = "";

    public List<MoveComponentData> MoveComponents => _moveComponents;
    private List<MoveComponentData> _moveComponents = new List<MoveComponentData>();

    public MoveData(XElement moveXML)
    {
        _statName = moveXML.Attribute("stat_name") != null ? moveXML.Attribute("stat_name").Value : "";
        _moveName = moveXML.Attribute("name") != null ? moveXML.Attribute("name").Value : "";
        _successStage = moveXML.Attribute("success_stage") != null ? moveXML.Attribute("success_stage").Value : "";
        _failStage = moveXML.Attribute("fail_stage") != null ? moveXML.Attribute("fail_stage").Value : "";

        MoveComponentData currentMoveComponent;
        foreach(XElement componentXML in moveXML.Element("Components").Elements("Component"))
        {
            currentMoveComponent = new MoveComponentData(componentXML);
            _moveComponents.Add(currentMoveComponent);
        }
    }
}

