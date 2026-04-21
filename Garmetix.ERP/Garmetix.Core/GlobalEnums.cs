 

//namespace Garmetix.Core
//{
//    public enum VoucherType { Payment, Receipt, Expense, Contra, Journal, Sales, Purchase }

//    public enum LedgerType { Asset, Cash, BankAccount, Loan, Expenses, DirectExpenses, IndirectExpenses, Income, DirectIncome, InDirectIncome, Purchase, Sale, StockItem, Employee, CapitalAccount }

//    public enum LedgerCategory { Credit, Debit, Income, Expenses, Assets, Bank, Loan, Purchase, Sale, Vendor, Customer, UnCategory, Employees, Stock, Debitor, Creditor }

//    public enum PartyType { Customer, Supplier, Employee, Vendor, Debitor, Creditor, Others }

//    public enum PaymentMode { Cash, Cheque, NEFT, RTGS, UPI, NetBanking, IMPS, DD, ATM, Swipe, CreditNote, Other }

//    public enum AccountType { Saving, Current, CashCredit, OverDraft, Others, Loan, CF }

//    public enum TransactionType { Deposit, Withdraw }
//}
namespace Garmetix.Core
{
  public  enum CompanyType
    {
        Proprietorship, // Proprietorship
        Partnership,
        PrivateLimited,
        PublicLimited,
        LLP, // Limited Liability Partnership
        Others
    }
    public enum StoreCategory
    {
        Cloths,
        Garments,
        Readymade,
        Furniture,
        FuelStation,
        General,
        Retail,
        Wholesale,
        Distributor,
        Others
    }
    public enum VoucherType { Payment, Receipt, Expense }
    public enum LedgerType { Assest, Cash, BankAccount, Loan, Expenses, DirectExpenses, IndirectExpenses, Income, DirectIncome, InDirectExpenses, Purcahase, Sale, StockItem, Employee, CaptialAccount }
    public enum LedgerCategory { Credit, Debit, Income, Expenses, Assets, Bank, Loan, Purchase, Sale, Vendor, Customer, UnCategory, Employees, Stock, Debitor, Creditor }
    public enum PartyType { Customer, Supplier, Employee, Vendor, Debitor, Creditor, Others }
    public enum Status { Pending, Ongoing, Running, Approved, Success, Error, Failed, InProgress, Started, Ended, Processing, Waiting, Rejected, Completed, Cancelled, PartiallyApproved, PartiallyRejected, PartiallyCompleted, Unknown }
    public enum AccountType { Saving, Current, CashCredit, OverDraft, Others, Loan, CF }
    public enum TransactionType { Deposit, Withdraw }
    public enum TransactionMode { Cash, Cheque, NEFT, RTGS, UPI, NetBanking, IMPS, DD, ATM, Swipe, Other }
    public enum PaymentMode { Cash, Cheque, NEFT, RTGS, UPI, NetBanking, IMPS, DD, ATM, Swipe, Other }
}