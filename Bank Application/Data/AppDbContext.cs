using Bank_Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Transactions;

namespace Bank_Application.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountStatus> AccountStatuses { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientAccount> ClientAccounts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ScheduledTransaction> ScheduledTransactions { get; set; }
        public DbSet<SubAccount> SubAccounts { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<AccountTypeFeature> AccountTypeFeatures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //الميزة تابعة لاكثر من النوع والنوع له اكثر من ميزة
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AccountTypeFeature>()
        .HasOne(atf => atf.AccountType)
        .WithMany(at => at.AccountTypeFeatures)
        .HasForeignKey(atf => atf.AccountTypeId);

            modelBuilder.Entity<AccountTypeFeature>()
                .HasOne(atf => atf.Feature)
                .WithMany(f => f.AccountTypeFeatures)
                .HasForeignKey(atf => atf.FeatureId);
            // علاقة many to many بين Client و Account
            modelBuilder.Entity<ClientAccount>()
                .HasKey(ca => new { ca.ClientId, ca.AccountId }); 

            modelBuilder.Entity<ClientAccount>()
                .HasOne(ca => ca.Client)
                .WithMany()
                .HasForeignKey(ca => ca.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientAccount>()
                .HasOne(ca => ca.Account)
                .WithMany(a => a.ClientAccounts)
                .HasForeignKey(ca => ca.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // العلاقة بين Account و SubAccount (one to many)
            modelBuilder.Entity<Account>()
     .HasMany(a => a.SubAccounts)
     .WithOne(sa => sa.ParentAccount)
     .HasForeignKey(sa => sa.ParentAccountId)
     .OnDelete(DeleteBehavior.Restrict);


            // علاقة Account , AccountType (one to many)
            modelBuilder.Entity<Account>()
                .HasOne(a => a.AccountType)
                .WithMany(at => at.Accounts)
                .HasForeignKey(a => a.AccountTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // علاقة Account , AccountStatus (one to many)
            modelBuilder.Entity<Account>()
                .HasOne(a => a.AccountStatus)
                .WithMany(asr => asr.Accounts)
                .HasForeignKey(a => a.AccountStatusId)
                .OnDelete(DeleteBehavior.Restrict);
           
            // علاقة Employee , Report  (موظف واحد يمتلك عدة تقارير)
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Employee)
                .WithMany(e => e.Reports)
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // علاقة Client , SupportTicket (عميل واحد يمتلك عدة تذاكر)
            modelBuilder.Entity<SupportTicket>()
                .HasOne(st => st.Client)
                .WithMany(c => c.SupportTickets)
                .HasForeignKey(st => st.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // علاقة Account , ScheduledTransaction
            modelBuilder.Entity<ScheduledTransaction>()
                .HasOne(st => st.Account)            
                .WithMany(a => a.ScheduledTransactions)  
                .HasForeignKey(st => st.AccountId)   
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TransactionLog>()
    .HasOne(tl => tl.TransactionType)
    .WithMany(tt => tt.TransactionLogs)
    .HasForeignKey(tl => tl.TransactionTypeId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionLog>()
                .HasOne(tl => tl.ReceiverAccount)        
                .WithMany(a => a.ReceivedTransactions)  
                .HasForeignKey(tl => tl.ReceiverAccountId)
                .OnDelete(DeleteBehavior.Restrict);


            // TransactionLog , Client
            modelBuilder.Entity<TransactionLog>()
                .HasOne(tl => tl.Client)
                .WithMany(c => c.TransactionLogs) 
                .HasForeignKey(tl => tl.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

           
   