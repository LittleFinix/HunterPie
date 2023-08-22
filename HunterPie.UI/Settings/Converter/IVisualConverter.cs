using System.Reflection;

namespace HunterPie.UI.Settings.Converter;

public interface IVisualConverter
{
    public FrameworkElement Build(object parent, PropertyInfo childInfo);
}
