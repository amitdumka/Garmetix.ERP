using System;
using Garmetix.Core.Models;

namespace Garmetix.Core.Auth
{
    public enum LoginRole { Admin, StoreManager, Salesman, Accountant, RemoteAccountant, Member, PowerUser }
    public enum UserType { Admin, Owner, StoreManager, Sales, Accountant, CA, Guest, PowerUser, Employees }
    public enum AppOperation { Company, StoreGroup, Store, All, None }

    public class AppUser : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Store hashed, never plain text
        public string? PinHash { get; set; } // For the quick POS PIN Unlock

        public LoginRole Role { get; set; }
        public UserType UserType { get; set; }
        public AppOperation AppOperation { get; set; } = AppOperation.Store;

        public Guid? RemoteUserId { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? StoreGroupId { get; set; }
        public Guid? StoreId { get; set; }
        public bool Admin { get; set; } = false;
    }
}