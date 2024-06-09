
namespace AutoDataGenerator;

class Program
{
    static void Main(string[] args)
    {

        // The input dir is the current directory
        string inputDir = Directory.GetCurrentDirectory();
        string outputDir;
        if (args.Length < 1)
        {
            outputDir = Console.ReadLine();
        }
        else
        {
            outputDir = args[0];
        }

        if (inputDir == null || !Directory.Exists(inputDir))
        {
            Console.WriteLine("Invalid input directory.");
            return;
        }

        if (outputDir == null || !Directory.Exists(outputDir))
        {
            Console.WriteLine("Invalid output directory.");
            return;
        }

        GenerateFF12Data(inputDir, Path.Combine(outputDir, "ff12_open_world"));
        GenerateLRData(inputDir, Path.Combine(outputDir, "lrff13"));
    }

    private static void GenerateFF12Data(string inputDir, string outputDir)
    {
        inputDir = inputDir.Replace("AutoDataGenerator", "FF12Rando");
        if (!Directory.Exists(outputDir))
        {
            Console.WriteLine("No output directory for FF12.");
            return;
        }

        Console.WriteLine("Generating FF12 data...");
        FF12MultiworldGenerator generator = new(inputDir, outputDir);
        generator.Generate();
    }

    private static void GenerateLRData(string inputDir, string outputDir)
    {
        inputDir = inputDir.Replace("AutoDataGenerator", "LRRando");
        if (!Directory.Exists(outputDir))
        {
            Console.WriteLine("No output directory for LR.");
            return;
        }

        Console.WriteLine("Generating LR data...");
        LRMultiworldGenerator generator = new(inputDir, outputDir);
        generator.Generate();
    }
}
