using System.Text;

namespace XmlToMdForJustTheDocs;

public class MdGenerator
{
    public StringBuilder _sb = new StringBuilder();
    
    public void H1(string value)
    {
        _sb.AppendLine($"# {value}");
    }

    public void H2(string value)
    {
        _sb.AppendLine($"## {value}");
    }

    public void H3(string value)
    {
        _sb.AppendLine($"### {value}");
    }

    public void H4(string value)
    {
        _sb.AppendLine($"#### {value}");
    }

    public void H5(string value)
    {
        _sb.AppendLine($"##### {value}");
    }

    public void H6(string value)
    {
        _sb.AppendLine($"###### {value}");
    }

    public void AltH1(string value)
    {
        _sb.AppendLine($"{value}\n======");
    }

    public void AltH2(string value)
    {
        _sb.AppendLine($"{value}\n------");
    }

    public void Separator()
    {
        _sb.AppendLine("---");
    }

    public void Italics(string value)
    {
        _sb.AppendLine($"_{value}_");
    }

    public void Bold(string value)
    {
        _sb.AppendLine($"**{value}**");
    }

    public void Strikethrough(string value)
    {
        _sb.AppendLine($"~~{value}~~");
    }

    public void Quote(string value)
    {
        _sb.AppendLine($"> {value}");
    }

    public void OrderedList(IEnumerable<string> value, int subList = 0)
    {
        var sb = new StringBuilder();
        var i = 1;
        foreach (var item in value)
        {
            for (var j = 0; j < subList; j++)
            {
                sb.Append("..");
            }

            sb.AppendLine($"{i}. {item}");
        }

        _sb.AppendLine(sb.ToString());
    }

    public void UnorderedList(IEnumerable<string> value, int subList = 0)
    {
        var sb = new StringBuilder();
        foreach (var item in value)
        {
            for (var j = 0; j < subList; j++)
            {
                sb.Append("⋅⋅");
            }

            sb.AppendLine($"* {item}");
        }

        _sb.AppendLine(sb.ToString());
    }

    public void Link(string text, string url)
    {
        _sb.AppendLine($"[{text}]({url})");
    }

    public void Link(string url)
    {
        _sb.AppendLine($"[{url}]({url})");
    }

    public void Code(string code)
    {
        _sb.AppendLine($"```csharp\n{code}\n```");
    }

    public void Table(IEnumerable<string> headers, params IEnumerable<string>[] items)
    {
        var sb = new StringBuilder();
        var headersList = headers.ToList();
        sb.AppendLine(string.Join(" | ", headersList));
        sb.AppendLine(string.Join(" | ", headersList.Select(x => "---")));
        foreach (var enumerable in items)
        {
            sb.AppendLine(string.Join(" | ", enumerable));
        }

        _sb.AppendLine(sb.ToString());
    }

    public void Property( string value, string name)
    {
        _sb.AppendLine($"{name}: {value}");
    }

    public void Property(bool value, string name)
    {
        _sb.AppendLine($"{name}: {(value ? "true" : "false")}");
    }

    public void Property(int value, string name)
    {
        _sb.AppendLine($"{name}: {value}");
    }

    public void JtdToHeader(Jtd jtd)
    {
        Separator();

        Property(jtd.Layout, "layout");
        if (jtd.Title != null)
        {
            Property(jtd.Title, "title");
        }

        if (jtd.NavigationOrder != null)
        {
            Property(jtd.NavigationOrder.Value, "nav_order");
        }

        if (jtd.HasChildren != null)
        {
            Property(jtd.HasChildren.Value, "has_children");
        }

        if (jtd.Permalink != null)
        {
            Property(jtd.Permalink, "permalink");
        }

        Separator();
    }

    public void AppendLine()
    {
        _sb.AppendLine();
    }

    public override string ToString()
    {
        return _sb.ToString();
    }

    public void Text(string? text)
    {
        if (text != null)
        {
            _sb.AppendLine(text.Trim());
        }
    }
    
    public void IndentText(string? text)
    {
        if (text != null)
        {
            _sb.AppendLine("\n⋅⋅⋅" + text.Trim());
        }
    }
}