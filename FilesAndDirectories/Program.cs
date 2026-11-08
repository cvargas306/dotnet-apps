using System.Globalization;
using System.Text;
using Newtonsoft.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);   

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

GenerateSalesSummaryReport(salesFiles, storesDirectory, Path.Combine(salesTotalDir, "salesSummary.txt"));

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;
    
    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {      
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);
    
        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
    
        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data?.Total ?? 0;
    }
    
    return salesTotal;
}

void GenerateSalesSummaryReport(IEnumerable<string> salesFiles, string storesDirectory, string reportPath)
{
    var usCulture = CultureInfo.GetCultureInfo("en-US");
    var details = new StringBuilder();
    double total = 0;

    foreach (var file in salesFiles)
    {
        // Only include the daily sales files
        if (Path.GetFileName(file) != "sales.json")
            continue;

        string json = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(json);
        double fileTotal = data?.Total ?? 0;
        total += fileTotal;

        // Show "201/sales.json" instead of just "sales.json"
        string name = Path.GetRelativePath(storesDirectory, file);
        details.AppendLine($"  {name}: {fileTotal.ToString("C", usCulture)}");
    }

    var report = new StringBuilder();
    report.AppendLine("Sales Summary");
    report.AppendLine("----------------------------");
    report.AppendLine($" Total Sales: {total.ToString("C", usCulture)}");
    report.AppendLine();
    report.AppendLine(" Details:");
    report.Append(details);

    File.WriteAllText(reportPath, report.ToString());
}

record SalesData (double Total);