using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

public class MoveComponentData
{
    public string Text => _text;
    private string _text = "";

    public float BaseTime => _baseTime;
    private float _baseTime = 0f;

    public MoveComponentData(XElement componentXML)
    {
        _text = componentXML.Value.Replace("\n","");
        _baseTime = componentXML.Attribute("base_time") != null ? float.TryParse(componentXML.Attribute("base_time").Value, out _baseTime) ? _baseTime : 0f : 0f;
    }
}

