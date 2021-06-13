using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;

public class StatData
{
    public string Name => _name;
    private string _name = "";

    public Color Colour => _colour;
    private Color _colour = Color.black;

    public StatData(XElement statXML)
    {
        _name = statXML.Attribute("name") != null ? statXML.Attribute("name").Value : "";
        if(statXML.Element("Colour") != null)
        {
            float r = -1, g = -1, b = -1;
            r = statXML.Element("Colour").Attribute("r") != null ? float.TryParse(statXML.Element("Colour").Attribute("r").Value, out r) ? r : -1f : -1f;
            g = statXML.Element("Colour").Attribute("g") != null ? float.TryParse(statXML.Element("Colour").Attribute("g").Value, out g) ? g : -1f : -1f;
            b = statXML.Element("Colour").Attribute("b") != null ? float.TryParse(statXML.Element("Colour").Attribute("b").Value, out b) ? b : -1f : -1f;


            _colour = new Color(r,g,b);
        }
    }
}

