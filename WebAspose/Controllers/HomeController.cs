using Aspose.Cells;
using Aspose.Cells.Charts;
using Aspose.Cells.Drawing;
using Aspose.Cells.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace WebAspose.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly ILogger<HomeController> _logger;

    // Home Controller
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    
    [HttpGet]
    [Route("Morpheus")]
    public async Task<IActionResult> Test2()
    {
        var obj = new ClassA
        {
            Property1 = "Test",
            Property2 = 123,
            Property3 = DateTime.Now
        };

        // generate a hash
        var hash = Morpheus.Serialize(obj);

        // has this object changed
        var isEqual = Morpheus.Compare(obj, hash);

        return Ok(isEqual);
    }


    [HttpGet]
    [Route("Compare")]
    public async Task<IActionResult> Compare()
    {
        var objA = new ClassA
        {
            Property1 = "Test", 
            Property2 = 123,
            Property3 = DateTime.Now
        };
        
        var objB = new ClassB
        {
            Property1 = "Test", 
            Property2 = 123
        };
        
        var areEqual = Morpheus.CompareObjects(objA, objB);
        return Ok(areEqual);
    }

    [HttpGet]
    [Route("GetExcelAspose")]
    public async Task<IActionResult> GetExcelAspose()
    {
        // Crea un nuovo workbook
        var workbook = new Workbook();

        // Accedi al primo foglio di lavoro
        var sheet = workbook.Worksheets[0];

        // Aggiungi dati al foglio di lavoro
        sheet.Cells["A1"].PutValue("Categoria");
        sheet.Cells["B1"].PutValue("Valore");

        // Aggiungi molte categorie e valori
        for (var i = 0; i < 10; i++)
        {
            sheet.Cells[$"A{i + 2}"].PutValue($"Categoria {i + 1}");
            sheet.Cells[$"B{i + 2}"].PutValue(new Random().Next(10, 120)); // Valori crescenti
        }

        // Aggiungi un grafico a torta
        var chartIndex = sheet.Charts.Add(ChartType.Pie, 15, 10, 35, 25);
        var chart = sheet.Charts[chartIndex];

        // Imposta l'intervallo di dati per il grafico
        chart.NSeries.Add("B2:B11", true); // Valori
        chart.NSeries.CategoryData = "A2:A11"; // Categorie

        // Imposta il titolo del grafico
        chart.Title.Text = "Esempio di Grafico a Torta";

        // Imposta il titolo del grafico
        chart.Title.Text = "Esempio di Grafico a Torta";
        // Salva il grafico come immagine in memoria
        using var ms = new MemoryStream();
        chart.ToImage(ms, new ImageOrPrintOptions
        {
            ImageType = ImageType.Png, // Formato immagine
            VerticalResolution = 300, // Risoluzione verticale
            HorizontalResolution = 300, // Risoluzione orizzontale
        });
        
        // Restituisci l'immagine come risultato
        return File(
            ms.ToArray(),
            "image/png",
            $"chart_{DateTime.Now:yyyyMMddHHmmss}.png"
        );

        //// Salva il file Excel in memoria in modo asincrono
        //using var ms = new MemoryStream();
        //await workbook.SaveAsync(ms, SaveFormat.Xlsx); // Salvataggio asincrono

        //ms.Position = 0; // Resetta la posizione dello stream
        
        //// Restituisci il file come download
        //return File(
        //    ms.ToArray(),
        //    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //    $"excel_{DateTime.Now:yyyyMMdd}.xlsx"
        //);
    }

    

}