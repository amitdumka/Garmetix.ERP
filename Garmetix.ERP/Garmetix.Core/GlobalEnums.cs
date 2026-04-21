 

namespace Garmetix.Core
{
    public enum VoucherType { Payment, Receipt, Expense, Contra, Journal, Sales, Purchase }

    public enum LedgerType { Asset, Cash, BankAccount, Loan, Expenses, DirectExpenses, IndirectExpenses, Income, DirectIncome, InDirectIncome, Purchase, Sale, StockItem, Employee, CapitalAccount }

    public enum LedgerCategory { Credit, Debit, Income, Expenses, Assets, Bank, Loan, Purchase, Sale, Vendor, Customer, UnCategory, Employees, Stock, Debitor, Creditor }

    public enum PartyType { Customer, Supplier, Employee, Vendor, Debitor, Creditor, Others }

    public enum PaymentMode { Cash, Cheque, NEFT, RTGS, UPI, NetBanking, IMPS, DD, ATM, Swipe, CreditNote, Other }

    public enum AccountType { Saving, Current, CashCredit, OverDraft, Others, Loan, CF }

    public enum TransactionType { Deposit, Withdraw }
}