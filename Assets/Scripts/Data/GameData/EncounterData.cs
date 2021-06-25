using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

public class EncounterData
{
    public string Name => _name;
    private string _name = "";

    public string StartingStage => _startingStage;
    private string _startingStage = "";

    private Dictionary<string, EncounterStageData> _stageMap = new Dictionary<string, EncounterStageData>();

    public EncounterData(XElement encounterXML)
    {
        _name = encounterXML.Attribute("name") != null ? encounterXML.Attribute("name").Value : "";
        _startingStage = encounterXML.Attribute("starting_stage") != null ? encounterXML.Attribute("starting_stage").Value : "";

        EncounterStageData stageData;
        foreach(XElement stageXML in encounterXML.Element("Stages").Elements("Stage"))
        {
            stageData = new EncounterStageData(stageXML);
            _stageMap.Add(stageData.Identifier, stageData);
        }
    }

    public EncounterStageData GetStageData(string inStageIdentifier)
    {
        return _stageMap.ContainsKey(inStageIdentifier) ? _stageMap[inStageIdentifier] : null;
    }
}

