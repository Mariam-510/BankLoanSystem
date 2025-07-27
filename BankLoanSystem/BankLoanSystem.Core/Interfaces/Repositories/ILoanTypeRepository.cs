using BankLoanSystem.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Core.Interfaces.Repositories
{
    public interface ILoanTypeRepository
    {
        Task<List<LoanType>> GetAllAsync();
        Task<LoanType?> GetByIdAsync(int id);
        Task<LoanType> CreateAsync(LoanType loanType);
        Task<LoanType?> UpdateAsync(int id, LoanType loanType);
        Task<LoanType?> DeleteAsync(int id);
    }
}
