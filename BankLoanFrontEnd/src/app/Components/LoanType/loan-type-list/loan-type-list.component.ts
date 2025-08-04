// loan-type-list.component.ts
import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LoanTypeService } from '../../../Services/ApiServices/loan-type.service';

@Component({
  selector: 'app-loan-type-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './loan-type-list.component.html',
  styleUrls: ['./loan-type-list.component.css']
})
export class LoanTypeListComponent implements OnInit {
  loanTypes: any[] = [];
  errorMessage = '';
  successMessage = '';

  constructor(
    private loanTypeService: LoanTypeService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadLoanTypes();
  }

  loadLoanTypes(): void {
    this.loanTypeService.getAllLoanTypes().subscribe({
      next: (data) => {
        this.loanTypes = data.data;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load loan types';
      }
    });
  }

  editLoanType(id: number): void {
    this.router.navigate(['/loan-types/edit', id]);
  }

  deleteLoanType(id: number): void {
    if (confirm('Are you sure you want to delete this loan type?')) {
      this.loanTypeService.deleteLoanType(id).subscribe({
        next: () => {
          this.successMessage = 'Loan type deleted successfully';
          this.loadLoanTypes();
        },
        error: (error) => {
          this.errorMessage = error.error?.message || 'Failed to delete loan type';
        }
      });
    }
  }
}
