import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LoanService } from '../../../Services/ApiServices/loan.service';
import { LoanTypeService } from '../../../Services/ApiServices/loan-type.service';
import { AuthService } from '../../../Services/ApiServices/auth.service';

@Component({
  selector: 'app-loan-application',
  imports: [ReactiveFormsModule, CommonModule, RouterModule, FormsModule],
  templateUrl: './loan-application.component.html',
  styleUrls: ['./loan-application.component.css']
})
export class LoanApplicationComponent {
  loanForm: FormGroup;
  loanTypes: any[] = [];
  errorMessage: string = '';
  successMessage: string = '';
  isLoading = false;
  nationalIdFile: File | null = null;
  salarySlipFile: File | null = null;

  constructor(
    private loanService: LoanService,
    private loanTypeService: LoanTypeService,
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.loanForm = this.fb.group({
      amount: ['', [Validators.required, Validators.min(1)]],
      duration: ['', [Validators.required, Validators.min(1)]],
      loanTypeId: ['', [Validators.required]]
    });

    this.loadLoanTypes();
  }

  loadLoanTypes(): void {
    this.loanTypeService.getAllLoanTypes().subscribe({
      next: (data) => {
        console.log(data);
        this.loanTypes = data.data;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load loan types';
      }
    });
  }

  onNationalIdChange(event: any): void {
    this.nationalIdFile = event.target.files[0];
  }

  onSalarySlipChange(event: any): void {
    this.salarySlipFile = event.target.files[0];
  }

  onSubmit(): void {
    if (this.loanForm.invalid || !this.nationalIdFile || !this.salarySlipFile) {
      this.errorMessage = 'Please fill all required fields and upload both documents';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const formData = {
      amount: this.loanForm.value.amount,
      duration: this.loanForm.value.duration,
      loanTypeId: this.loanForm.value.loanTypeId,
      nationalId: this.nationalIdFile,
      salarySlip: this.salarySlipFile
    };

    this.loanService.createLoan(formData).subscribe({
      next: () => {
        this.successMessage = 'Loan application submitted successfully!';
        this.errorMessage = '';
        this.loanForm.reset();
        this.nationalIdFile = null;
        this.salarySlipFile = null;
        this.isLoading = false;
        setTimeout(() => {
          this.router.navigate(['/loans']);
        }, 2000);
      },
      error: (error) => {
        this.errorMessage = error.error?.message || 'Failed to submit loan application';
        this.isLoading = false;
      }
    });
  }
}
