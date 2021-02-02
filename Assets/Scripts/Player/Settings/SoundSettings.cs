using System.IO;
using System.Xml.Serialization;

public class SoundSettings
{
    public int MasterVolume { get; set; }
    public int MusicVolume { get; set; }
    public int VfxVolume { get; set; }

    public string ToXML()
    {
        using (StringWriter sw = new StringWriter())
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            
        }
        return "";
    }
}
