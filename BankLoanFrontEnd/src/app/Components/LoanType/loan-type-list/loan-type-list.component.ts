// loan-type-list.component.ts
import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LoanTypeService } from '../../../Services/ApiServices/loan-type.service';

@Component({
  selector: 'app-loan-type-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './loan-type-list.component.html',
  styleUrls: ['./loan-type-list.component.css']
})
export class LoanTypeListComponent implements OnInit {
  loanTypes: any[] = [];
  filteredLoanTypes: any[] = [];
  paginatedLoanTypes: any[] = [];
  errorMessage = '';
  successMessage = '';
  isLoading = false;
  searchQuery = '';

  // Pagination properties
  currentPage = 1;
  itemsPerPage = 4;
  totalPages = 1;

  Math = Math;

  constructor(
    private loanTypeService: LoanTypeService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadLoanTypes();
  }

  loadLoanTypes(): void {
    this.isLoading = true;
    this.loanTypeService.getAllLoanTypes().subscribe({
      next: (data) => {
        this.loanTypes = data.data;
        this.filteredLoanTypes = [...this.loanTypes];
        this.applyFilters();
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load loan types';
        this.isLoading = false;
      }
    });
  }

  applyFilters(): void {
    this.filteredLoanTypes = this.loanTypes.filter(type => {
      const searchMatch =
        !this.searchQuery ||
        type.name.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        type.id.toString().includes(this.searchQuery);

      return searchMatch;
    });

    this.currentPage = 1;
    this.updatePagination();
  }

  clearFilters(): void {
    this.searchQuery = '';
    this.applyFilters();
  }

  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredLoanTypes.length / this.itemsPerPage);
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedLoanTypes = this.filteredLoanTypes.slice(startIndex, endIndex);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePagination();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePagination();
    }
  }

  getVisiblePages(): number[] {
    const maxVisible = 3;
    const start = Math.max(1, this.currentPage - 1);
    const end = Math.min(this.totalPages, start + maxVisible - 1);
    return Array.from({ length: end - start + 1 }, (_, i) => start + i);
  }

  editLoanType(id: number): void {
    this.router.navigate(['/loan-types/edit'], { queryParams: { id: id } });
  }

  deleteLoanType(id: number): void {
    if (confirm('Are you sure you want to delete this loan type?')) {
      this.isLoading = true;
      this.loanTypeService.deleteLoanType(id).subscribe({
        next: () => {
          this.successMessage = 'Loan type deleted successfully';
          this.loadLoanTypes();
          setTimeout(() => {
            this.successMessage = '';
          }, 1500);
        },
        error: (error) => {
          this.errorMessage = error.error?.message || 'Failed to delete loan type';
          this.isLoading = false;
        }
      });
    }
  }
}
