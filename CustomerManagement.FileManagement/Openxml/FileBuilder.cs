using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CustomerManagement.FileManagement.Openxml;

public class FileBuilder
{
    private const string ExceptionSheetNotFound = "Sheet not found.";
    private const string ExceptionWorksheetPartNotFound = "Worksheet part not found.";
    private const string ExceptionSheetDataNotFound = "Sheet data not found.";
    private const string ExceptionSheetPropertiesNotFound = "Sheet properties not found.";
    private const string ExceptionOutlinePropertiesNotFound = "Outline properties not found.";
    private const string ExceptionSheetViewsNotFound = "Sheet views not found.";
    private const string ExceptionMergeCellsNotFound = "Merge cells not found.";

    public FileBuilder()
    {
    }

    public static void SetWorksheetFeatures(WorksheetPart worksheetPart)
    {
        Worksheet worksheet = new();

        // 1. SheetProperties (optional)
        SheetProperties sheetProperties = new(new OutlineProperties());
        // 2. SheetDimension (required)
        SheetDimension sheetDimension = new() { Reference = "A1" };
        // 3. SheetViews (optional)
        SheetViews sheetViews = new(new SheetView() { WorkbookViewId = 0 });
        // 4. SheetFormatProperties (optional)
        SheetFormatProperties sheetFormatProperties = new() { DefaultRowHeight = 15 };
        // 5. Columns (optional)
        Columns columns = new(new Column() { Min = 1, Max = 1, Width = 20, CustomWidth = true });
        // 6. SheetData (required)
        SheetData sheetData = new();
        // 7. SheetProtection (optional)
        // 8. AutoFilter (optional)
        // 9. mergecells (optional)
        MergeCells mergeCells = new(new MergeCell() { Reference = "A1" });
        // 10. ConditionalFormatting (optional)
        // 11. DataValidations (optional)
        // 12. Hyperlinks (optional)
        // 13. PrintOptions (optional)
        // 14. PageMargins (optional)
        // 15. PageSetup (optional)
        // 16. HeaderFooter (optional)
        // 17. Drawing (optional)
        // 18. LegacyDrawing (optional)
        // 19. CustomSheetViews (optional)
        // 20. ExtensionList (optional)

        // Assemble in correct order
        worksheet.Append(sheetProperties);
        worksheet.Append(sheetDimension);
        worksheet.Append(sheetViews);
        worksheet.Append(sheetFormatProperties);
        worksheet.Append(columns);
        worksheet.Append(sheetData);
        worksheet.Append(mergeCells);

        worksheetPart.Worksheet = worksheet;
        worksheetPart.Worksheet.Save();
    }

    public static Stylesheet CreateStyleSheet()
    {
        Fonts fonts = new(
            //0 Default
            new Font(
                new FontSize() { Val = 11 },
                new Color() { Theme = 1 },
                new FontName() { Val = "Calibri" }),
            //1
            new Font(
                new FontSize() { Val = 11 },
                new FontName() { Val = "Arial" },
                new Bold()),
            //1
            new Font(
                new FontSize() { Val = 11 },
                new FontName() { Val = "Arial" })
        );

        Fills fills = new(
            new Fill(new PatternFill() { PatternType = PatternValues.None }),
            new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }),
            new Fill(
                new PatternFill()
                {
                    PatternType = PatternValues.Solid,
                    ForegroundColor = new ForegroundColor { Rgb = HexBinaryValue.FromString("BFBFBF") },
                    BackgroundColor = new BackgroundColor { Indexed = 64 }
                })
        );

        Borders borders = new(
            //0 Default
            new Border(),
            //1
            new Border(
                new LeftBorder(),
                new RightBorder(),
                new TopBorder() { Style = BorderStyleValues.Medium, Color = new Color { Auto = true } },
                new BottomBorder() { Style = BorderStyleValues.Medium, Color = new Color { Auto = true } },
                new DiagonalBorder()),
            //2
            new Border(
                new LeftBorder(),
                new RightBorder(),
                new TopBorder { Style = BorderStyleValues.Thin, Color = new Color { Auto = true } },
                new BottomBorder() { Style = BorderStyleValues.Double, Color = new Color { Auto = true } },
                new DiagonalBorder())
        );

        // Number indexes must match with position on each property (fonts, fills, borders, ...) 
        CellFormats cellFormats = new(
            new CellFormat(), //0 Default
            new CellFormat() //1
            {
                FontId = 1,
                FillId = 0,
                BorderId = 0,
                ApplyFont = true
            },
            new CellFormat() //2
            {
                FontId = 2,
                FillId = 0,
                BorderId = 0,
                ApplyFont = true
            }
        );

        var styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

        return styleSheet;
    }

    public static Sheet CreateSheet(WorkbookPart workbookPart, WorksheetPart worksheetPart, Sheets sheets, UInt32Value sheetId, string name)
    {
        Sheet sheet = new()
        {
            Id = workbookPart.GetIdOfPart(worksheetPart),
            SheetId = sheetId,
            Name = name
        };

        sheets.Append(sheet);
        return sheet;
    }

    public static void CreateTextCell(WorkbookPart workbookPart, UInt32Value sheetId, int columnIndex, int rowIndex,
        string value, CellValues dataType, UInt32Value styleIndex)
    {
        string columnLetter = GetColumnLetter(columnIndex - 1);
        string cellReference = columnLetter + rowIndex;

        Sheet? sheet =
            workbookPart.Workbook.Sheets?.Elements<Sheet>().FirstOrDefault(s => s.SheetId == sheetId)
            ?? throw new ArgumentException(ExceptionSheetNotFound);

        if (sheet == null) return;

        WorksheetPart worksheetPart =
            (WorksheetPart)workbookPart.GetPartById(sheet.Id!)
            ?? throw new ArgumentException(ExceptionWorksheetPartNotFound);

        SheetData sheetData =
            worksheetPart.Worksheet.GetFirstChild<SheetData>()
            ?? throw new ArgumentException(ExceptionSheetDataNotFound);

        Row? row = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex?.Value == rowIndex);

        if (row == null)
        {
            row = new() { RowIndex = (uint)rowIndex };
            sheetData.Append(row);
        }

        Cell? cell = row.Elements<Cell>().FirstOrDefault(f => f.CellReference == cellReference);

        if (cell == null)
        {
            cell = new Cell
            {
                CellReference = cellReference,
                CellValue = new CellValue(value),
                DataType = dataType,
                StyleIndex = styleIndex
            };

            row.Append(cell);
        }
        else
        {
            cell.CellValue = new CellValue(value);
            cell.DataType = dataType;
            cell.StyleIndex = styleIndex;
        }

        workbookPart.Workbook.Save();
    }

    public static string GetColumnLetter(int columnIndex)
    {
        string columnName = string.Empty;

        while (columnIndex >= 0)
        {
            columnName = (char)('A' + (columnIndex % 26)) + columnName;
            columnIndex = (columnIndex / 26) - 1;
        }

        return columnName;
    }

}
