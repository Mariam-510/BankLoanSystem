import { Component, OnInit } from '@angular/core';
import { LoanService } from '../../../Services/ApiServices/loan.service';
import { AuthService } from '../../../Services/ApiServices/auth.service';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-loan-list',
  imports: [CommonModule, RouterModule],
  templateUrl: './loan-list.component.html',
  styleUrls: ['./loan-list.component.css']
})
export class LoanListComponent implements OnInit {
  loans: any[] = [];
  userLoans: any[] = [];
  isLoading = false;
  errorMessage = '';

  constructor(
    private loanService: LoanService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.loadLoans();
  }

  loadLoans(): void {
    this.isLoading = true;

    this.loanService.getUserLoans().subscribe({
      next: (data) => {
        console.log(data);
        this.userLoans = data.data;
        this.isLoading = false;
      },
      error: (error) => {
        console.log(error);
        // this.errorMessage = 'Failed to load your loans';
        this.isLoading = false;
      }
    });
  }

  // Add this method to fix the error
  getStatusText(status: number): string {
    switch (status) {
      case 0: return 'Pending';
      case 1: return 'Approved';
      case 2: return 'Rejected';
      default: return 'Unknown';
    }
  }

  updateLoanStatus(loanId: number, status: number): void {
    this.loanService.updateLoanStatus(loanId, status).subscribe({
      next: () => {
        this.loadLoans();
      },
      error: (error) => {
        this.errorMessage = 'Failed to update loan status';
      }
    });
  }
}
