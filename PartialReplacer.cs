using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SiteBuilder;

public class PartialReplacer
{
    private const string EXTENSION = ".part";
    private readonly string? _partialsDir;

    public PartialReplacer(string partialsDir)
    {
        this._partialsDir = partialsDir;
    }

    public void CopyAndCompile(string sourcePath, string destinationPath)
    {
        Console.WriteLine($" Compile {sourcePath} to  {destinationPath}");

        var inputHtml = File.ReadAllText(sourcePath);
        var outputHtml = ReplaceAllPartials(inputHtml);

        File.WriteAllText(destinationPath, outputHtml);
    }

    private string ReplaceAllPartials(string html)
    {
        var lastIndex = 0;
        var sb = new StringBuilder();

        // Partial tag format: {{{ partial: test1 }}}
        string pattern = @"\{{3}\s*partial:\s*([_A-Za-z0-9]+)\s*\}{3}";
        foreach (Match match in Regex.Matches(html, pattern, RegexOptions.IgnoreCase))
        {
            var partialName = match.Groups[1].Value;
            var partialPath = Path.Combine(this._partialsDir + @"\" + partialName + EXTENSION);
            var partialHtml = File.ReadAllText(partialPath);

            // Copy content until start of partial tag
            sb.Append(html.Substring(lastIndex, match.Index - lastIndex));
            // Replace partial tag with html
            sb.Append(partialHtml);
            // Move index past end of partial tag.
            lastIndex = match.Index + match.Length;
        }
        sb.Append(html.Substring(lastIndex)); // Get remaining content after last partial tag.

        return sb.ToString();
    }
}