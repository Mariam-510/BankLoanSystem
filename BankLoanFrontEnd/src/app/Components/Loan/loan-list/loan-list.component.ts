import { Component, OnInit } from '@angular/core';
import { LoanService } from '../../../Services/ApiServices/loan.service';
import { AuthService } from '../../../Services/ApiServices/auth.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-loan-list',
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './loan-list.component.html',
  styleUrls: ['./loan-list.component.css']
})
export class LoanListComponent implements OnInit {
  loans: any[] = [];
  userLoans: any[] = [];
  filteredLoans: any[] = [];
  paginatedLoans: any[] = [];
  isLoading = false;
  errorMessage = '';
  successMessage = '';
  Math = Math;

  // Filter properties
  statusFilter = 'all';
  searchQuery = '';

  // Pagination properties
  currentPage = 1;
  itemsPerPage = 3;
  totalPages = 1;

  // Sorting properties
  sortField = 'createdAt';
  sortDirection: 'asc' | 'desc' = 'desc';

  constructor(
    private loanService: LoanService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    if (this.authService.hasRole("Admin")) {
      this.router.navigate(['/admin/dashboard']);
    }
    this.loadLoans();
  }

  loadLoans(): void {
    this.isLoading = true;
    this.loanService.getUserLoans().subscribe({
      next: (data) => {
        this.userLoans = data.data;
        this.sortLoans();
        this.filteredLoans = [...this.userLoans];
        this.applyFilters();
        this.isLoading = false;
      },
      error: (error) => {
        console.log(error);
        this.isLoading = false;
      }
    });
  }

  sortLoans(): void {
    this.userLoans.sort((a, b) => {
      const dateA = new Date(a.createdAt).getTime();
      const dateB = new Date(b.createdAt).getTime();

      if (this.sortField === 'createdAt') {
        return this.sortDirection === 'asc' ? dateA - dateB : dateB - dateA;
      }
      return 0;
    });
  }

  toggleSort(field: string): void {
    if (this.sortField === field) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortField = field;
      this.sortDirection = 'desc';
    }
    this.sortLoans();
    this.applyFilters();
  }

  applyFilters(): void {
    this.filteredLoans = this.userLoans.filter(loan => {
      const statusMatch =
        this.statusFilter === 'all' ||
        loan.status.toString() === this.statusFilter;

      const searchMatch =
        !this.searchQuery ||
        loan.id.toString().includes(this.searchQuery) ||
        loan.amount.toString().includes(this.searchQuery) ||
        loan.duration.toString().includes(this.searchQuery) ||
        (loan.loanTypeName && loan.loanTypeName.toLowerCase().includes(this.searchQuery.toLowerCase()));

      return statusMatch && searchMatch;
    });

    this.currentPage = 1;
    this.updatePagination();
  }

  clearFilters(): void {
    this.statusFilter = 'all';
    this.searchQuery = '';
    this.applyFilters();
  }

  hasActiveFilters(): boolean {
    return this.statusFilter !== 'all' || !!this.searchQuery;
  }

  removeStatusFilter(): void {
    this.statusFilter = 'all';
    this.applyFilters();
  }

  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredLoans.length / this.itemsPerPage);
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedLoans = this.filteredLoans.slice(startIndex, endIndex);
  }

  getPageNumbers(): number[] {
    const pages = [];
    const maxVisiblePages = 5;

    if (this.totalPages <= maxVisiblePages) {
      for (let i = 1; i <= this.totalPages; i++) {
        pages.push(i);
      }
    } else {
      let startPage = Math.max(1, this.currentPage - Math.floor(maxVisiblePages / 2));
      let endPage = startPage + maxVisiblePages - 1;

      if (endPage > this.totalPages) {
        endPage = this.totalPages;
        startPage = Math.max(1, endPage - maxVisiblePages + 1);
      }

      for (let i = startPage; i <= endPage; i++) {
        pages.push(i);
      }
    }

    return pages;
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

  getStatusText(status: number): string {
    switch (status) {
      case 0: return 'Pending';
      case 1: return 'Approved';
      case 2: return 'Rejected';
      default: return 'Unknown';
    }
  }

  deleteLoan(loanId: number): void {
    if (confirm('Are you sure you want to delete this loan application?')) {
      this.isLoading = true;
      this.loanService.deleteLoan(loanId).subscribe({
        next: () => {
          this.successMessage = 'Loan deleted successfully';
          this.loadLoans();
          setTimeout(() => {
            this.successMessage = '';
          }, 1500);
        },
        error: (error) => {
          this.errorMessage = 'Failed to delete loan application';
          this.isLoading = false;
        }
      });
    }
  }

  calculateDueDate(createdDate: Date | string, durationMonths: number): Date {
    const date = new Date(createdDate);
    date.setMonth(date.getMonth() + durationMonths);
    return date;
  }

  isDueDatePassed(loan: any): boolean {
    const dueDate = this.calculateDueDate(loan.createdAt, loan.duration);
    return new Date() > dueDate && loan.status === 1;
  }

  getDaysOverdue(loan: any): number {
    const dueDate = this.calculateDueDate(loan.createdAt, loan.duration);
    const diff = new Date().getTime() - dueDate.getTime();
    return Math.floor(diff / (1000 * 60 * 60 * 24));
  }
}
