namespace CIncludeGenerator;

[AttributeUsage(AttributeTargets.Class)]
public class IncludeHeaderAttribute : Attribute
{
    public IncludeHeaderAttribute(string header, string library)
    {
        Header = header;
        Library = library;
    }

    public string Header { get; set; }
    
    public string Library { get; set; }
}