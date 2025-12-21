namespace Bank_Application.services
{
    public interface ITransactionReportService
    {
        Task<byte[]> GenerateDailyReportAsync(DateTime reportDate);
    }
}
