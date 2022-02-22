using System;
using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }
        public int UserID { get; set; }
        [Required]
        [Display(Name = "Deposit/Withdraw")]
        public double Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}