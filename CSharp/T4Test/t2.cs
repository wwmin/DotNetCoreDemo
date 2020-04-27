using System.Collections.Generic;
using System.Xml;

partial class Catalog
{
    private XmlNode thisNode;
    public Catalog(XmlNode node)
    {
        thisNode = node;
    }

    public IEnumerable<Artist> Artist
    {
        get
        {
            foreach (XmlNode node in thisNode.SelectNodes("artist"))
                yield return new Artist(node);
        }
    }
}

partial class Artist
{
    private XmlNode thisNode;
    public Artist(XmlNode node)
    {
        thisNode = node;
    }

    public IEnumerable<Song> Song
    {
        get
        {
            foreach (XmlNode node in thisNode.SelectNodes("song"))
                yield return new Song(node);
        }
    }
    public string id { get { return thisNode.Attributes["id"].Value; } }
    public string name { get { return thisNode.Attributes["name"].Value; } }
}

partial class Song
{
    private XmlNode thisNode;
    public Song(XmlNode node)
    {
        thisNode = node;
    }

    public string Text { get { return thisNode.InnerText; } }
    public string id { get { return thisNode.Attributes["id"].Value; } }
}
partial class Catalog
{
    public Catalog(string fileName)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(fileName);
        thisNode = doc.SelectSingleNode("catalog");
    }
}
