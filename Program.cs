namespace SiteBuilder;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Compiling website...");

        #if(DEBUG)
        var baseDir = @"C:\Knut\Projects\dotnet\SiteBuilder\TEST_FILES\";
        #else
        var baseDir = Environment.CurrentDirectory;
        #endif

        var navigator = new FileNavigator(baseDir);
        navigator.Run();
    }
}
