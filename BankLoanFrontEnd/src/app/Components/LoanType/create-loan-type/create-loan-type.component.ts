// loan-type-create.component.ts
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LoanTypeService } from '../../../Services/ApiServices/loan-type.service';

@Component({
  selector: 'app-create-loan-type',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './create-loan-type.component.html',
  styleUrls: ['./create-loan-type.component.css']
})
export class CreateLoanTypeComponent {
  loanTypeForm: FormGroup;
  errorMessage = '';
  successMessage = '';
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private loanTypeService: LoanTypeService,
    private router: Router
  ) {
    this.loanTypeForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]]
    });
  }

  onSubmit(): void {
    if (this.loanTypeForm.invalid) {
      return;
    }

    this.isLoading = true;
    this.loanTypeService.createLoanType(this.loanTypeForm.value)
      .subscribe({
        next: () => {
          this.errorMessage = '';
          this.successMessage = 'Loan type created successfully';
          this.isLoading = false;
          setTimeout(() => {
            this.router.navigate(['/loan-types']);
          }, 1500);
        },
        error: (error) => {
          this.isLoading = false;

          if (error.error && error.error.errors && error.error.errors.length > 0) {
            this.errorMessage = error.error.errors[0]; // Take the first error
          } else if (error.error?.message) {
            this.errorMessage = error.error.message;
          } else {
            // Fallback to a generic error message
            this.errorMessage = 'Failed to create loan type. Please try again.';
          }
        }
      });
  }
}
