using BankLoanSystem.Core.Interfaces.Repositories;
using BankLoanSystem.Core.Models.Entities;
using BankLoanSystem.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Infrastructure.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly LoanDbContext _dbContext;

        public LoanRepository(LoanDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Loan>> GetAllAsync()
        {
            return await _dbContext.Loans
                .Include(l => l.AppUser)
                .Include(l => l.LoanType)
                .Where(l => !l.IsDeleted)
                .ToListAsync();
        }

        public async Task<Loan?> GetByIdAsync(int id)
        {
            return await _dbContext.Loans
                .Include(l => l.AppUser)
                .Include(l => l.LoanType)
                .Where(l => !l.IsDeleted)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<List<Loan>> GetAllByUserAsync(string appuserId)
        {
            return await _dbContext.Loans
                .Include(l => l.AppUser)
                .Include(l => l.LoanType)
                .Where(l => !l.IsDeleted && l.AppUserId == appuserId)
                .ToListAsync();
        }

        public async Task<List<Loan>> GetAllByTypeAsync(int loanTypeId)
        {
            return await _dbContext.Loans
                .Include(l => l.AppUser)
                .Include(l => l.LoanType)
                .Where(l => !l.IsDeleted && l.LoanTypeId == loanTypeId)
                .ToListAsync();
        }

        public async Task<Loan> CreateAsync(Loan loan)
        {
            await _dbContext.Loans.AddAsync(loan);
            await _dbContext.SaveChangesAsync();
            return loan;
        }

        public async Task<Loan?> UpdateAsync(int id, Loan loan)
        {
            var existingLoan = await GetByIdAsync(id);
            if (existingLoan == null)
            {
                return null;
            }

            _dbContext.Loans.Update(existingLoan);

            await _dbContext.SaveChangesAsync();

            return existingLoan;
        }

        public async Task<Loan?> DeleteAsync(int id)
        {
            var existingLoan = await GetByIdAsync(id);
            if (existingLoan == null)
            {
                return null;
            }

            existingLoan.IsDeleted = true;

            await _dbContext.SaveChangesAsync();

            return existingLoan;
        }
    }
}