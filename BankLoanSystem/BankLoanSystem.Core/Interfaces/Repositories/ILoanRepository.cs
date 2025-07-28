using BankLoanSystem.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Core.Interfaces.Repositories
{
    public interface ILoanRepository
    {
        Task<List<Loan>> GetAllAsync();
        Task<Loan?> GetByIdAsync(int id);
        Task<List<Loan>> GetAllByUserAsync(string appuserId);
        Task<List<Loan>> GetAllByTypeAsync(int loanTypeId);
        Task<Loan> CreateAsync(Loan loan);
        Task<Loan?> UpdateAsync(int id, Loan loan);
        Task<Loan?> DeleteAsync(int id);
    }
}
