using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    private Dictionary<string, StatData> _statMap = new Dictionary<string, StatData>();
    private Dictionary<string, EncounterData> _encounterMap = new Dictionary<string, EncounterData>();
    

    public void LoadData(string configPath)
    {
        //Load the XML configuration file into text
        TextAsset textAsset = (TextAsset)Resources.Load(configPath);

        //Open the configuration file
        XElement configurationXML = XElement.Parse(textAsset.text);

        EncounterData currentEncounter;
        //Load the encounter data
        foreach(XElement encounterXML in configurationXML.Element("Encounters").Elements("Encounter"))
        {
            currentEncounter = new EncounterData(encounterXML);
            _encounterMap.Add(currentEncounter.Name, currentEncounter);
        }

        StatData currentStat;
        //Load the stats data
        foreach(XElement statXML in configurationXML.Element("Stats").Elements("Stat"))
        {
            currentStat = new StatData(statXML);
            _statMap.Add(currentStat.Name, currentStat);
        }
    }

    public EncounterData GetEncounterData(string encounterName)
    {
        return _encounterMap.ContainsKey(encounterName) ? _encounterMap[encounterName] : null;
    }

    public StatData GetStatData(string statName)
    {
        return _statMap.ContainsKey(statName) ? _statMap[statName] : null;
    }
}
