using AutoMapper;
using CustomerManagement.Core.Requests.Customer;
using CustomerManagement.Core.Responses.Customer;
using CustomerManagement.Core.Shared.Helpers;
using CustomerManagement.FileManagement.Openxml;
using CustomerManagement.Infrastructure.Repositories.Interfaces;
using CustomerManagement.Models;
using CustomerManagement.Services.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CustomerManagement.Domain.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public List<Customer> GetAll() => _customerRepository.GetAll();

    public Customer? GetById(int id) => _customerRepository.GetById(id);

    public void Add(Customer customer) => _customerRepository.Add(customer);

    public CreateCustomerResponse Add(CreateCustomerRequest customerRequest)
    {
        CreateCustomerResponse createCustomerResponse = new();
        var customer = _mapper.Map<Customer>(customerRequest);
        _customerRepository.Add(customer);

        createCustomerResponse.SetCreatedResponse(customer.Id, ["Customer created"]);
        return createCustomerResponse;
    }

    public void Update(Customer customer)
    {
        _customerRepository.Update(customer);
    }

    public void Delete(int id)
    {
        _customerRepository.Delete(id);
    }

    public byte[] GetExportReport(GetExportReportRequest request)
    {
        UInt32Value sheetId = 0;

        using MemoryStream memoryStream = new();
        using (SpreadsheetDocument document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
        {
            WorkbookPart workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            FileBuilder.SetWorksheetFeatures(worksheetPart);
            Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
            worksheetPart.Worksheet = new Worksheet(new SheetData());
            List<UInt32Value> sheetIdsForColumnResizing = [];
            var currentDateTime = DateTime.Now;

            WorkbookStylesPart stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
            stylesPart.Stylesheet = FileBuilder.CreateStyleSheet();
            stylesPart.Stylesheet.Save();

            var coverPageSheet = FileBuilder.CreateSheet(workbookPart, worksheetPart, sheets, ++sheetId, "First Page");

            FileBuilder.CreateTextCell(workbookPart, coverPageSheet.SheetId, 3, 12, "Analysis for", CellValues.String, 1);
            FileBuilder.CreateTextCell(workbookPart, coverPageSheet.SheetId, 3, 13, "Analysis Period:", CellValues.String, 1);
            FileBuilder.CreateTextCell(workbookPart, coverPageSheet.SheetId, 3, 14, "Report Dated: ", CellValues.String, 1);

            FileBuilder.CreateTextCell(workbookPart, coverPageSheet.SheetId, 4, 12, "Main Company", CellValues.String, 2);
            FileBuilder.CreateTextCell(workbookPart, coverPageSheet.SheetId, 4, 13, "Nov - Dec", CellValues.String, 2);
            FileBuilder.CreateTextCell(workbookPart, coverPageSheet.SheetId, 4, 14, currentDateTime.ToString(), CellValues.String, 2);
        }

        memoryStream.Position = 0;
        var memoryStreamArray = memoryStream.ToArray();
        return memoryStreamArray;
    }

}
