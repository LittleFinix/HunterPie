using HunterPie.Core.Domain.Interfaces;
using System;
using System.Collections;
using System.IO;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace HunterPie.Core.System.Common.Registry;

public class FileRegistry : ILocalRegistry
{
    private FileInfo _file;

    public FileRegistry() : this("HunterPie")
    {
        
    }
    
    public FileRegistry(string name)
    {
        string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        dir = Path.Join(dir, "HunterPie");

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        _file = new FileInfo(Path.Join(dir, $"{name}.yml"));
    }
    
    private static readonly Deserializer _des = new();
    private static readonly Serializer _ses = new();

    private YamlDocument? _cachedDocument;
    
    private YamlMappingNode Load()
    {
        if (_cachedDocument is null)
        {
            if (_file.Exists)
            {
                YamlStream stream = new();
                using StreamReader txt = _file.OpenText();
                stream.Load(txt);

                if (stream.Documents.Count > 0)
                    _cachedDocument = stream.Documents[0];
            }
            
            _cachedDocument ??= new(new YamlMappingNode());
        }

        return ((YamlMappingNode)_cachedDocument.RootNode);
    }

    private void Save()
    {
        YamlStream stream = new(_cachedDocument);
        using StreamWriter txt = _file.CreateText();
        stream.Save(txt);
    }

    public void Set<T>(string name, T value)
    {
        YamlMappingNode doc = Load();

        if (value is IList values)
        {
            YamlSequenceNode seq = new();
            seq.Tag = '!' + typeof(byte).FullName;
            seq.Style = SequenceStyle.Flow;

            var en = values.GetEnumerator();
            while (en.MoveNext())
            {
                seq.Add(Convert.ToString(en.Current));
            }

            doc.Children[name] = seq;
        }
        else
        {
            YamlScalarNode node = new()
            {
                Value = Convert.ToString(value) ?? string.Empty,
                Tag = '!' + typeof(T).FullName
            };

            doc.Children[name] = node;
        }

        Save();
    }

    public bool Exists(string name)
    {
        YamlMappingNode doc = Load();
        return doc.Children.ContainsKey(name);
    }

    public object Get(string name)
    {
        YamlMappingNode doc = Load();
        var node = doc.Children[name];
        
        var type = Type.GetType(node.Tag.Value[1..]);
        
        switch (node)
        {
            case YamlSequenceNode sequenceNode:
                var array = Array.CreateInstance(type, sequenceNode.Children.Count);

                for (int i = 0; i < array.Length; i++)
                {
                    var value = ((YamlScalarNode)sequenceNode.Children[i]).Value;
                    array.SetValue(Convert.ChangeType(value, type), i);
                }

                return array;
            
            case YamlScalarNode scalarNode:
                return Convert.ChangeType(scalarNode.Value, type);
            
            default:
                throw new InvalidDataException();
        }
    }

    public T Get<T>(string name) => (T) Get(name);

    public void Delete(string name)
    {
        YamlMappingNode doc = Load();
        doc.Children.Remove(name);
        Save();
    }
}