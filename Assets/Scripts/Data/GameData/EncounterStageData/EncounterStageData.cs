using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

public class EncounterStageData
{
    public string Identifier => _identifier;
    private string _identifier = "";

    public string Name => _name;
    private string _name = "";

    public string Description => _description;
    private string _description = "";

    public List<MoveData> AvailableMoves => _availableMoves;
    private List<MoveData> _availableMoves = new List<MoveData>();

    public EncounterStageData(XElement stageXML)
    {
        _identifier = stageXML.Attribute("identifier") != null ? stageXML.Attribute("identifier").Value : "";
        _name = stageXML.Attribute("name") != null ? stageXML.Attribute("name").Value : "";
        _description = stageXML.Element("Description") != null ? stageXML.Element("Description").Value : "";

        MoveData currentMoveData;
        foreach(XElement moveXML in stageXML.Element("AvailableMoves").Elements("AvailableMove"))
        {
            currentMoveData = new MoveData(moveXML);
            _availableMoves.Add(currentMoveData);
        }
    }

}

