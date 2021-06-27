using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

public class EncounterStageData
{
    public string Identifier => _identifier;
    protected string _identifier = "";

    public EncounterStageData(XElement stageXML)
    {
        _identifier = stageXML.Attribute("identifier") != null ? stageXML.Attribute("identifier").Value : "";
    }

}

