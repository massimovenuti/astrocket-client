using System.Text;
using UnityEngine;
using Luminosity.IO;
using System.Xml;

public class SaveAndLoadSettings
{
    public static void Save()
    {
        StringBuilder output = new StringBuilder();
        InputSaverXML saver = new InputSaverXML(output);
        InputManager.Save(saver);

        XmlDocument xml = new XmlDocument();
        xml.LoadXml(output.ToString());
        Debug.Log(output.ToString());
    }

    public static void Load()
    {

    }
}
