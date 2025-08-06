import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LoanService } from '../../../Services/ApiServices/loan.service';
import { LoanTypeService } from '../../../Services/ApiServices/loan-type.service';
import { AuthService } from '../../../Services/ApiServices/auth.service';
import { FileSizePipe } from '../../../Pipes/file-size.pipe';

@Component({
  selector: 'app-loan-application',
  imports: [ReactiveFormsModule, CommonModule, RouterModule, FormsModule, FileSizePipe],
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
    private fb: FormBuilder) {
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

  // Add these to your component
  nationalIdPreview: string | null = null;
  salarySlipPreview: string | null = null;

  onNationalIdChange(event: any) {
    const file = event.target.files[0];
    if (!file) return;

    // Validate file type
    const allowedTypes = ['image/jpeg', 'image/png', 'application/pdf'];
    if (!allowedTypes.includes(file.type)) {
      this.errorMessage = 'Only JPG, PNG, or PDF files are allowed';
      event.target.value = ''; // Clear the file input
      return;
    }

    // Validate file size (5MB max)
    const maxSize = 5 * 1024 * 1024; // 5MB
    if (file.size > maxSize) {
      this.errorMessage = 'File size exceeds 5MB limit';
      event.target.value = ''; // Clear the file input
      return;
    }

    this.errorMessage = ''; // Clear any previous errors
    this.nationalIdFile = file;

    if (file.type.includes('image')) {
      const reader = new FileReader();
      reader.onload = () => this.nationalIdPreview = reader.result as string;
      reader.readAsDataURL(file);
    } else {
      this.nationalIdPreview = 'pdf';
    }
  }

  onSalarySlipChange(event: any) {
    const file = event.target.files[0];
    if (!file) return;

    // Validate file type
    const allowedTypes = ['image/jpeg', 'image/png', 'application/pdf'];
    if (!allowedTypes.includes(file.type)) {
      this.errorMessage = 'Only JPG, PNG, or PDF files are allowed';
      event.target.value = ''; // Clear the file input
      return;
    }

    // Validate file size (5MB max)
    const maxSize = 5 * 1024 * 1024; // 5MB
    if (file.size > maxSize) {
      this.errorMessage = 'File size exceeds 5MB limit';
      event.target.value = ''; // Clear the file input
      return;
    }

    this.errorMessage = ''; // Clear any previous errors
    this.salarySlipFile = file;

    if (file.type.includes('image')) {
      const reader = new FileReader();
      reader.onload = () => this.salarySlipPreview = reader.result as string;
      reader.readAsDataURL(file);
    } else {
      this.salarySlipPreview = 'pdf';
    }
  }

  removeNationalId() {
    this.nationalIdFile = null;
    this.nationalIdPreview = null;
    // Clear the file input value
    const fileInput = document.getElementById('nationalId') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  }

  removeSalarySlip() {
    this.salarySlipFile = null;
    this.salarySlipPreview = null;
    // Clear the file input value
    const fileInput = document.getElementById('salarySlip') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
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
        this.errorMessage = '';
        this.successMessage = 'Loan application submitted successfully!';
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
