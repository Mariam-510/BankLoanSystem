using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BankLoanSystem.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankLoanSystem.Infrastructure;

public partial class Loan
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    public int Duration { get; set; }

    public int Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    [StringLength(255)]
    public string NationalIdPath { get; set; } = null!;

    [StringLength(255)]
    public string SalarySlipPath { get; set; } = null!;

    public string AppUserId { get; set; } = null!;

    public int LoanTypeId { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("AppUserId")]
    [InverseProperty("Loans")]
    public virtual AppUser? AppUser { get; set; } = null!; 

    [ForeignKey("LoanTypeId")]
    [InverseProperty("Loans")]
    public virtual LoanType? LoanType { get; set; } = null!;
}
