import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoanService } from '../../../Services/ApiServices/loan.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {
  loans: any[] = [];
  filteredLoans: any[] = [];
  paginatedLoans: any[] = [];
  isLoading = false;
  errorMessage = '';

  // Filter properties
  statusFilter = 'all';
  searchQuery = '';

  // Pagination properties
  currentPage = 1;
  itemsPerPage = 4;
  totalPages = 1;

  // Sorting properties
  sortField = 'createdAt';
  sortDirection: 'asc' | 'desc' = 'desc';

  // Status counts
  pendingCount: number = 0;
  approvedCount: number = 0;
  rejectedCount: number = 0;

  Math = Math;

  constructor(private loanService: LoanService) { }

  ngOnInit(): void {
    this.loadLoans();
  }

  loadLoans(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.loanService.getAllLoans().subscribe({
      next: (data) => {
        this.loans = data.data;
        this.calculateStatusCounts();
        this.sortLoans();
        this.filteredLoans = [...this.loans];
        this.applyFilters();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Failed to load loans', error);
        this.errorMessage = 'Failed to load loan applications. Please try again later.';
        this.isLoading = false;
      }
    });
  }

  calculateStatusCounts(): void {
    this.pendingCount = this.loans.filter(loan => loan.status === 0).length;
    this.approvedCount = this.loans.filter(loan => loan.status === 1).length;
    this.rejectedCount = this.loans.filter(loan => loan.status === 2).length;
  }

  sortLoans(): void {
    this.loans.sort((a, b) => {
      const valueA = this.sortField === 'createdAt' ? new Date(a[this.sortField]).getTime() : a[this.sortField];
      const valueB = this.sortField === 'createdAt' ? new Date(b[this.sortField]).getTime() : b[this.sortField];

      if (valueA < valueB) {
        return this.sortDirection === 'asc' ? -1 : 1;
      }
      if (valueA > valueB) {
        return this.sortDirection === 'asc' ? 1 : -1;
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
    this.filteredLoans = this.loans.filter(loan => {
      const statusMatch =
        this.statusFilter === 'all' ||
        loan.status.toString() === this.statusFilter;

      const searchMatch =
        !this.searchQuery ||
        loan.appUserFirstName.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        (loan.appUserLastName && loan.appUserLastName.toLowerCase().includes(this.searchQuery.toLowerCase())) ||
        (loan.loanTypeName && loan.loanTypeName.toLowerCase().includes(this.searchQuery.toLowerCase())) ||
        loan.id.toString().includes(this.searchQuery);

      return statusMatch && searchMatch;
    });

    this.currentPage = 1;
    this.updatePagination();
  }

  // This is the missing method that was causing the error
  onFilterChange(): void {
    this.applyFilters();
  }

  calculateDueDate(createdDate: Date | string, durationMonths: number): Date {
    const date = new Date(createdDate);
    date.setMonth(date.getMonth() + durationMonths);
    return date;
  }

  isDueDatePassed(loan: any): boolean {
    if (loan.status !== 1) return false; // Only approved loans can be overdue
    const dueDate = this.calculateDueDate(loan.createdAt, loan.duration);
    return new Date() > dueDate;
  }

  getDaysOverdue(loan: any): number {
    const dueDate = this.calculateDueDate(loan.createdAt, loan.duration);
    const diff = new Date().getTime() - dueDate.getTime();
    return Math.floor(diff / (1000 * 60 * 60 * 24));
  }

  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredLoans.length / this.itemsPerPage);
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedLoans = this.filteredLoans.slice(startIndex, endIndex);
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

  getStatusText(status: number): string {
    switch (status) {
      case 0: return 'Pending';
      case 1: return 'Approved';
      case 2: return 'Rejected';
      default: return 'Unknown';
    }
  }
}
