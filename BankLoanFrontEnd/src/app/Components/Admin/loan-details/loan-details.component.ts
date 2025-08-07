// loan-details.component.ts
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LoanService } from '../../../Services/ApiServices/loan.service';
import { CommonModule } from '@angular/common';
import { API_CONFIG } from '../../../app.config';
import { SafeUrlPipe } from '../../../Pipes/safe-url.pipe';

declare var bootstrap: any;

@Component({
  selector: 'app-loan-details',
  standalone: true,
  imports: [CommonModule, SafeUrlPipe],
  templateUrl: './loan-details.component.html',
  styleUrls: ['./loan-details.component.css']
})
export class LoanDetailsComponent implements OnInit {
  loan: any = null;
  isLoading = false;
  errorMessage = '';
  successMessage = '';
  previewUrl: string | null = null;
  apiConfig = API_CONFIG;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private loanService: LoanService
  ) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const id = params['id'];
      if (id) {
        this.loadLoanDetails(+id);
      } else {
        this.errorMessage = 'No loan ID provided';
        this.router.navigate(['admin/dashboard']);
      }
    });
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

  getStatusText(status: number): string {
    switch (status) {
      case 0: return 'Pending';
      case 1: return 'Approved';
      case 2: return 'Rejected';
      default: return 'Unknown';
    }
  }

  getDocumentUrl(path: string): string {
    return `${this.apiConfig.apiUrl + path}`;
  }

  previewDocument(documentPath: string) {
    this.previewUrl = this.getDocumentUrl(documentPath);
    const modal = new bootstrap.Modal(document.getElementById('documentPreviewModal'));
    modal.show();
  }

  async downloadFile(fileUrl: string) {
    if (!fileUrl) return;

    try {
      const response = await fetch(fileUrl);
      const blob = await response.blob();
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = this.getFilenameFromUrl(fileUrl);
      a.click();
      window.URL.revokeObjectURL(url);
    } catch (error) {
      console.error('Download failed:', error);
      this.errorMessage = 'Failed to download file. Please try again.';
    }
  }

  private getFilenameFromUrl(url: string): string {
    try {
      const cleanUrl = url.split('?')[0].split('#')[0];
      const lastSegment = cleanUrl.split('/').pop() || '';
      return lastSegment.includes('.') ? lastSegment : 'document';
    } catch {
      return 'document';
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
