import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { LoanService } from '../../../Services/ApiServices/loan.service';
import { CommonModule } from '@angular/common';
import { API_CONFIG } from '../../../app.config';
import { SafeUrlPipe } from '../../../Pipes/safe-url.pipe';

declare var bootstrap: any;

@Component({
  selector: 'app-loan-view',
  standalone: true,
  imports: [CommonModule, RouterModule, SafeUrlPipe],
  templateUrl: './loan-view.component.html',
  styleUrls: ['./loan-view.component.css']
})
export class LoanViewComponent implements OnInit {
  loan: any = null;
  isLoading = false;
  errorMessage = '';
  apiConfig = API_CONFIG;

  constructor(
    private route: ActivatedRoute,
    private loanService: LoanService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const id = params['id'];
      if (id) {
        this.loadLoanDetails(+id);
      } else {
        this.errorMessage = 'No loan ID provided';
        this.router.navigate(['/loans']);
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

  getStatusText(status: number): string {
    switch (status) {
      case 0: return 'Pending';
      case 1: return 'Approved';
      case 2: return 'Rejected';
      default: return 'Unknown';
    }
  }

  getStatusClass(status: number): string {
    switch (status) {
      case 0: return 'bg-warning';
      case 1: return 'bg-success';
      case 2: return 'bg-danger';
      default: return 'bg-secondary';
    }
  }

  getDocumentUrl(path: string): string {
    return `${this.apiConfig.apiUrl + path}`;
  }

  // Add to your component
  previewUrl: string | null = null;

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

      // Extract filename from URL
      // const filename = this.getFilenameFromUrl(fileUrl);

      const a = document.createElement('a');
      a.href = url;
      a.download = fileUrl;
      a.click();

      window.URL.revokeObjectURL(url);
    } catch (error) {
      console.error('Download failed:', error);
      this.errorMessage = 'Failed to download file. Please try again.';
    }
  }

  private getFilenameFromUrl(url: string): string {
    try {
      // Handle URLs with query parameters or fragments
      const cleanUrl = url.split('?')[0].split('#')[0];
      const lastSegment = cleanUrl.split('/').pop() || '';

      // Check if the last segment has an extension
      if (lastSegment.includes('.')) {
        return lastSegment;
      }

      // If no extension found, try to determine from content type
      return lastSegment || 'document';
    } catch {
      return 'document';
    }
  }

  // Add these methods to your component
  calculateDueDate(createdDate: Date | string, durationMonths: number): Date {
    const date = new Date(createdDate);
    date.setMonth(date.getMonth() + durationMonths);
    return date;
  }

  isDueDatePassed(loan: any): boolean {
    const dueDate = this.calculateDueDate(loan.createdAt, loan.duration);
    return new Date() > dueDate && loan.status === 1; // Only for approved loans
  }

  getDaysOverdue(loan: any): number {
    const dueDate = this.calculateDueDate(loan.createdAt, loan.duration);
    const diff = new Date().getTime() - dueDate.getTime();
    return Math.floor(diff / (1000 * 60 * 60 * 24));
  }

}
