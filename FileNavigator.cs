using System;

namespace SiteBuilder;

public class FileNavigator
{
    public const string DIST = @"DIST\";
    public const string PARTIALS = @"_PARTIALS";
    public const string HTML_EXT = @".html";

    private readonly string? _baseDir;

    public FileNavigator(string baseDir)
    {
        this._baseDir = baseDir;
    }

    public void Run()
    {
        var destinationDir = PrepareDistFolder(this._baseDir!);

        CopyRecursive(this._baseDir!, destinationDir);
    }

    private void CopyRecursive(string currentDir, string destinationDir)
    {
        var directories = Directory.EnumerateDirectories(currentDir);
        foreach (string d in directories)
        {
            if (!Ignore(d, destinationDir))
            {
                CopyRecursive(d, destinationDir);
            }
        }

        CopyFiles(currentDir, destinationDir);
    }

    private bool Ignore(string path, string destinationDir)
    {
        return path.ToLower().StartsWith(destinationDir.ToLower()) ||
                path.ToLower().Contains(PARTIALS.ToLower());
    }

    private void CopyFiles(string folder, string destinationDir)
    {
        var files = Directory.EnumerateFiles(folder);

        foreach (string f in files)
        {
            if (Path.GetFileName(f).ToLower().StartsWith("sitebuilder")) {
                continue;
            }

            if (Path.GetExtension(f).ToLower() == HTML_EXT)
            {
                CopyCompiledFile(f, this._baseDir!, destinationDir);
            }
            else
            {
                CopyInertFile(f, this._baseDir!, destinationDir);
            }
        }
    }

    private void CopyInertFile(string sourcePath, string baseDir, string destinationDir)
    {
        var relPath = Path.GetRelativePath(baseDir, sourcePath);
        var destinationPath = Path.Combine(destinationDir, relPath);

        Console.WriteLine($"Copy {sourcePath} to {destinationPath}");

        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);
        File.Copy(sourcePath, destinationPath);
    }

    private void CopyCompiledFile(string sourcePath, string baseDir, string destinationDir) {
        var relPath = Path.GetRelativePath(baseDir, sourcePath);
        var destinationPath = Path.Combine(destinationDir, relPath);
        var partialsDir = Path.Combine(this._baseDir!, PARTIALS);

        var compiler = new PartialReplacer(partialsDir);
        compiler.CopyAndCompile(sourcePath, destinationPath);
    }

    private static string PrepareDistFolder(string baseDir)
    {
        var destination = Path.Combine(baseDir, DIST);

        if (Directory.Exists(destination))
        {
            Directory.Delete(destination, true);
        }

        Directory.CreateDirectory(destination);

        return destination;
    }

}