namespace Bank_Application.services
{
    using Bank_Application.Data;
    using Bank_Application.Models;
    using Microsoft.EntityFrameworkCore;
    using QuestPDF.Fluent;
    using QuestPDF.Helpers;
    using QuestPDF.Infrastructure;
    using Bank_Application.DTOs;
    
    public class TransactionReportService : ITransactionReportService
    {
        private readonly AppDbContext _context;

        public TransactionReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> GenerateDailyReportAsync(DateTime reportDate)
        {

            var logs = await _context.TransactionLogs
                .Include(t => t.TransactionType)
                .Include(t => t.Client)
                .Where(t => t.TransactionDate.HasValue &&
                            t.TransactionDate.Value.Date == reportDate.Date)
                .OrderBy(t => t.TransactionDate)
                .ToListAsync();

            var logIds = logs.Select(l => l.TransactionLogId).ToList();


            var approvals = await _context.TransactionApprovals
                .Include(a => a.ApprovedBy)
                .Where(a => logIds.Contains(a.TransactionLogId))
                .ToListAsync();


            var reportItems = logs.Select(t =>
            {
                var approval = approvals
                    .FirstOrDefault(a => a.TransactionLogId == t.TransactionLogId);

                return new TransactionReportItem
                {
                    TransactionDate = t.TransactionDate!.Value,
                    TransactionType = t.TransactionType?.TypeName ?? "Unknown",
                    SenderAccount = t.SenderAccountId?.ToString() ?? "-",
                    ReceiverAccount = t.ReceiverAccountId?.ToString() ?? "-",
                    Amount = t.Amount ?? 0,
                    ClientName = t.Client != null
                        ? $"{t.Client.FirstName} {t.Client.LastName}"
                        : "-",
                    ApprovalStatus = approval?.Status ?? "N/A",
                    ApprovedBy = approval?.ApprovedBy != null
                        ? $"{approval.ApprovedBy.FirstName} {approval.ApprovedBy.LastName}"
                        : "-"
                };
            }).ToList();


            var document = new TransactionReportDocument(reportItems);
            using var stream = new MemoryStream();
            document.GeneratePdf(stream);

            return stream.ToArray();
        }
    }






        public class TransactionReportDocument : IDocument
        {
            private List<TransactionReportItem> _items;

            public TransactionReportDocument(List<TransactionReportItem> items)
            {
                _items = items;
            }

            public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

            public void Compose(IDocumentContainer container)
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);
                    page.Header().Text("تقرير المعاملات اليومية").FontSize(20).Bold();
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("التاريخ");
                            header.Cell().Text("نوع العملية");
                            header.Cell().Text("الحساب المرسل");
                            header.Cell().Text("الحساب المستلم");
                            header.Cell().Text("المبلغ");
                            header.Cell().Text("العميل");
                            header.Cell().Text("حالة الموافقة");
                            header.Cell().Text("الموافق");
                        });

                        foreach (var item in _items)
                        {
                            table.Cell().Text(item.TransactionDate.ToString("yyyy-MM-dd HH:mm"));
                            table.Cell().Text(item.TransactionType);
                            table.Cell().Text(item.SenderAccount);
                            table.Cell().Text(item.ReceiverAccount);
                            table.Cell().Text(item.Amount.ToString("N2"));
                            table.Cell().Text(item.ClientName);
                            table.Cell().Text(item.ApprovalStatus);
                            table.Cell().Text(item.ApprovedBy);
                        }
                    });
                });
            }
        }

    }

