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
    public class LoanTypeRepository : ILoanTypeRepository
    {
        private readonly LoanDbContext _dbContext;

        public LoanTypeRepository(LoanDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<LoanType>> GetAllAsync()
        {
            return await _dbContext.LoanTypes
                .Where(lt => !lt.IsDeleted)
                .ToListAsync();
        }

        public async Task<LoanType?> GetByIdAsync(int id)
        {
            return await _dbContext.LoanTypes
                .Where(lt => !lt.IsDeleted)
                .FirstOrDefaultAsync(lt => lt.Id == id);
        }

        public async Task<LoanType> CreateAsync(LoanType loanType)
        {
            await _dbContext.LoanTypes.AddAsync(loanType);
            await _dbContext.SaveChangesAsync();
            return loanType;
        }

        public async Task<LoanType?> UpdateAsync(int id, LoanType loanType)
        {
            var existingLoanType = await GetByIdAsync(id);
            if (existingLoanType == null)
            {
                return null;
            }

            // Explicitly mark as modified
            _dbContext.LoanTypes.Update(existingLoanType);

            await _dbContext.SaveChangesAsync();

            return existingLoanType;
        }

        public async Task<LoanType?> DeleteAsync(int id)
        {
            var existingLoanType = await GetByIdAsync(id);
            if (existingLoanType == null)
            {
                return null;
            }

            existingLoanType.IsDeleted = true;

            await _dbContext.SaveChangesAsync();

            return existingLoanType;
        }
    }
}

