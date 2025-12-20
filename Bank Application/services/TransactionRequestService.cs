//using Bank_Application.Approvals;
//using Bank_Application.Data;
//using Bank_Application.Models;

//public class TransactionRequestService
//{
//    private readonly AppDbContext _context;
//    private readonly ApprovalHandler _approvalChain;
//   // private readonly INotificationService _notification;

//    public TransactionRequestService(
//        AppDbContext context
//      //  INotificationService notification)
//    {
//        _context = context;
//       // _notification = notification;

//        _approvalChain = new AutoApprovalHandler();
//        _approvalChain.SetNext(new ManagerApprovalHandler(context));
//    }

//    public async Task RequestAsync(TransactionLog transaction)
//    {
//        _context.TransactionLogs.Add(transaction);
//        await _context.SaveChangesAsync();

//        var context = new ApprovalContext
//        {
//            Transaction = transaction
//        };

//        await _approvalChain.HandleAsync(context);

//        if (context.IsApproved)
//        {
//            await ExecuteTransactionAsync(transaction);
//            await _notification.NotifyAsync("TransactionApproved", transaction.TransactionLogId);
//        }
//        else
//        {
//            await _notification.NotifyAsync("TransactionPendingApproval", transaction.TransactionLogId);
//        }
//    }
//}
