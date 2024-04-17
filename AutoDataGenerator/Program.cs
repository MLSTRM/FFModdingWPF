
namespace AutoDataGenerator;

class Program
{
    static void Main(string[] args)
    {

        string modeArg;
        string inputDir;
        string outputDir;
        if (args.Length < 3)
        {
            // Ask the user for input for the mode and output directory
            Console.WriteLine("Enter the mode (ff12) and output directory:");
            modeArg = Console.ReadLine();
            inputDir = Console.ReadLine();
            outputDir = Console.ReadLine();
        }
        else
        {
            modeArg = args[0];
            inputDir = args[1];
            outputDir = args[2];
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

        if (modeArg == "ff12")
        {
            GenerateFF12Data(inputDir, outputDir);
        }
        else
        {
            Console.WriteLine("Invalid mode.");
            return;
        }
    }

    private static void GenerateFF12Data(string inputDir, string outputDir)
    {
        Console.WriteLine("Generating FF12 data...");
        FF12MultiworldGenerator generator = new(inputDir, outputDir);
        generator.Generate();
    }
}
