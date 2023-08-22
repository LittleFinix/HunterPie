using HunterPie.Core.Logger;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace HunterPie.Core.Client.Localization;

public class Localization
{

    private readonly XmlDocument? document;
    private static Localization? _instance;

    public static Localization Instance
    {
        get
        {
            if (_instance is null)
                _instance = new();

            return _instance;
        }
    }

    private Localization()
    {
        string englishXml = Path.Combine(ClientInfo.LanguagesPath, "en-us.xml");

        document = new();
        
        if (File.Exists(englishXml))
            document.Load(englishXml);
        else
            Log.Error("Failed to load en-us localization");

        string xmlPath = Path.Combine(ClientInfo.LanguagesPath, ClientConfig.Config.Client.Language);

        if (!File.Exists(xmlPath))
        {
            Log.Error($"Failed to find {Path.GetFileNameWithoutExtension(xmlPath)} localization");
            return;
        }

        try
        {
            // Merges selected custom localization with the default en-us one
            // to avoid missing strings
            if (ClientConfig.Config.Client.Language != "en-us.xml")
            {
                XmlDocument otherLanguage = new();
                otherLanguage.Load(xmlPath);

                XmlNodeList englishNodes = document.DocumentElement!.SelectNodes("//*")!;
                foreach (XmlNode node in otherLanguage.DocumentElement!.SelectNodes("//*")!)
                {
                    string id = node.Attributes!["Id"]?.Value!;

                    if (id is null)
                        continue;

                    string path = GetFullParentPath(node);
                    XmlNode match = document.DocumentElement.SelectSingleNode($"//{path}/*[@Id='{id}']")!;

                    if (match is null)
                        continue;

                    if (match.Attributes?["String"] is not null)
                        match.Attributes!["String"]!.Value = node.Attributes["String"]?.Value ?? match.Attributes["String"]!.Value;

                    if (match.Attributes?["Description"] is not null)
                        match.Attributes!["Description"]!.Value = node.Attributes["Description"]?.Value ?? match.Attributes["Description"]!.Value;
                }
            }
        }
        catch (Exception err)
        {
            Log.Error(err.ToString());
        }

        Log.Info($"Loaded localization {Path.GetFileNameWithoutExtension(xmlPath)}");
    }

    private static string GetFullParentPath(XmlNode node, string path = "")
    {
        return node.ParentNode?.Name == null || node.ParentNode.Name == "#document"
            ? path
            : GetFullParentPath(node.ParentNode, $"{node.ParentNode.Name}/{path}");
    }

    public static XmlNode? Find(params string[] keys)
    {
        if (keys.Length == 0)
            return null;

        string attr = keys[^1];
        string query;

        if (attr.StartsWith('/'))
            query = attr;
        else
            query = "//Strings/" + string.Join('/', keys[..^1]) + $"[@Id='{attr}']";
        
        return Instance.document?.SelectSingleNode(query);
    }
    
    public static string FindString(params string[] keys)
    {
        return Find(keys)?.Attributes?["String"]?.Value ?? string.Join('.', keys);
    }
    public static string FindDescription(params string[] keys)
    {
        return Find(keys)?.Attributes?["Description"]?.Value ?? string.Join('.', keys);
    }

    [Obsolete("Use Find, FindString, FindDescription, or GetEnumString")]
    public static XmlNode? Query(string query) => Instance.document?.SelectSingleNode(query);
    
    [Obsolete("Use Find, FindString, FindDescription, or GetEnumString")]
    public static string QueryString(string query) => Query(query)?.Attributes?["String"]?.Value ?? query;
    
    [Obsolete("Use Find, FindString, FindDescription, or GetEnumString")]
    public static string QueryDescription(string query) => Query(query)?.Attributes?["Description"]?.Value ?? query;

    public static string GetEnumString<T>(T enumValue)
    {
        MemberInfo memberInfo = enumValue.GetType()
                                         .GetMember(enumValue.ToString()!)
                                         .First();

        LocalizationAttribute? attribute = memberInfo.GetCustomAttribute<LocalizationAttribute>();

#pragma warning disable CS0618 // Type or member is obsolete
        return attribute is null ? enumValue.ToString()! : QueryString(attribute.XPath)!;
#pragma warning restore CS0618 // Type or member is obsolete
    }
}