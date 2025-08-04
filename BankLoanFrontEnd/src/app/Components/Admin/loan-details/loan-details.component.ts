// loan-details.component.ts
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LoanService } from '../../../Services/ApiServices/loan.service';
import { API_CONFIG } from '../../../app.config';

@Component({
  selector: 'app-loan-details',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './loan-details.component.html',
  styleUrls: ['./loan-details.component.css']
})
export class LoanDetailsComponent implements OnInit {
  loan: any = null;
  isLoading = false;
  errorMessage = '';
  successMessage = '';
  apiConfig = API_CONFIG;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private loanService: LoanService
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadLoanDetails(+id);
    }
  }

  loadLoanDetails(id: number): void {
    this.isLoading = true;
    this.loanService.getLoanById(id).subscribe({
      next: (data) => {
        this.loan = data.data;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load loan details';
        this.isLoading = false;
      }
    });
  }

  updateLoanStatus(status: number): void {
    if (!this.loan) return;

    this.isLoading = true;
    this.loanService.updateLoanStatus(this.loan.id, status).subscribe({
      next: () => {
        this.successMessage = 'Loan status updated successfully';
        this.loadLoanDetails(this.loan.id);
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to update loan status';
        this.isLoading = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/admin/dashboard']);
  }

  getStatusText(status: number): string {
    switch (status) {
      case 0: return 'Pending';
      case 1: return 'Approved';
      case 2: return 'Rejected';
      default: return 'Unknown';
    }
  }

  // loan-details.component.ts
  getDocumentUrl(path: string): string {
    return `${this.apiConfig.apiUrl + path}`;
  }
}
