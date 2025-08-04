// admin-dashboard.component.ts
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
  isLoading = false;
  statusFilter: string = 'all'; // Default to show all
  searchQuery: string = '';

  // Status counts
  pendingCount: number = 0;
  approvedCount: number = 0;
  rejectedCount: number = 0;

  constructor(private loanService: LoanService) { }

  ngOnInit(): void {
    this.loadLoans();
  }

  loadLoans(): void {
    this.isLoading = true;
    this.loanService.getAllLoans().subscribe({
      next: (data) => {
        this.loans = data.data;
        this.calculateStatusCounts();
        this.applyFilters();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Failed to load loans', error);
        this.isLoading = false;
      }
    });
  }

  calculateStatusCounts(): void {
    this.pendingCount = this.loans.filter(loan => loan.status === 0).length;
    this.approvedCount = this.loans.filter(loan => loan.status === 1).length;
    this.rejectedCount = this.loans.filter(loan => loan.status === 2).length;
  }

  applyFilters(): void {
    this.filteredLoans = this.loans.filter(loan => {
      // Apply status filter
      const statusMatch =
        this.statusFilter === 'all' ||
        loan.status.toString() === this.statusFilter;

      // Apply search filter
      const searchMatch =
        !this.searchQuery ||
        loan.appUserFirstName.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        loan.appUserLastName?.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        loan.loanTypeName.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        loan.id.toString().includes(this.searchQuery);

      return statusMatch && searchMatch;
    });
  }

  onFilterChange(): void {
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
