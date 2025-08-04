// loan-type-create.component.ts
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LoanTypeService } from '../../../Services/ApiServices/loan-type.service';


@Component({
  selector: 'app-create-loan-type',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './create-loan-type.component.html',
  styleUrl: './create-loan-type.component.css'
})
export class CreateLoanTypeComponent {
  loanTypeForm: FormGroup;
  errorMessage = '';
  successMessage = '';

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

    this.loanTypeService.createLoanType(this.loanTypeForm.value)
      .subscribe({
        next: () => {
          this.successMessage = 'Loan type created successfully';
          setTimeout(() => {
            this.router.navigate(['/loan-types']);
          }, 2000);
        },
        error: (error) => {
          this.errorMessage = error.error?.message || 'Failed to create loan type';
        }
      });
  }
}
