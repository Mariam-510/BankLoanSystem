import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { LoanService } from '../../../Services/ApiServices/loan.service';
import { LoanTypeService } from '../../../Services/ApiServices/loan-type.service';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { API_CONFIG } from '../../../app.config';
import { FileSizePipe } from '../../../Pipes/file-size.pipe';


@Component({
  selector: 'app-edit-loan',
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule, FileSizePipe],
  templateUrl: './edit-loan.component.html',
  styleUrl: './edit-loan.component.css'
})
export class EditLoanComponent implements OnInit {
  loanForm: FormGroup;
  loanId: number = 0;
  isLoading = false;
  errorMessage = '';
  loanTypes: any[] = [];
  nationalIdFile: File | null = null;
  salarySlipFile: File | null = null;
  loan: any = null; // Add this property to store the loan data
  successMessage = '';

  apiConfig = API_CONFIG;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private loanService: LoanService,
    private loanTypeService: LoanTypeService
  ) {
    this.loanForm = this.fb.group({
      amount: ['', [Validators.required, Validators.min(1)]],
      duration: ['', [Validators.required, Validators.min(1)]],
      loanTypeId: ['', Validators.required],
      nationalId: [null],
      salarySlip: [null]
    });
  }

  ngOnInit(): void {
    this.loanId = Number(this.route.snapshot.paramMap.get('id'));
    // this.loadLoanTypes();
    this.loadLoanDetails();
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

  loadLoanDetails(): void {
    this.isLoading = true;
    this.loanService.getLoanById(this.loanId).subscribe({
      next: (response) => {
        this.loan = response.data; // Store the loan data
        console.log(this.loan);
        this.loanForm.patchValue({
          amount: response.data.amount,
          duration: response.data.duration,
          loanTypeId: response.data.loanTypeId
        });
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load loan details';
        this.isLoading = false;
      }
    });
  }

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
    if (this.loanForm.invalid) {
      this.errorMessage = 'Please fill all required fields';
      return;
    }

    this.isLoading = true;
    const formData = new FormData();
    formData.append('Amount', this.loanForm.value.amount);
    formData.append('Duration', this.loanForm.value.duration);
    // formData.append('LoanTypeId', this.loanForm.value.loanTypeId);

    // Make these optional for updates
    if (this.nationalIdFile) formData.append('NationalId', this.nationalIdFile);
    if (this.salarySlipFile) formData.append('SalarySlip', this.salarySlipFile);

    this.loanService.updateLoan(this.loanId, formData).subscribe({
      next: () => {
        this.errorMessage = '';
        this.successMessage = 'Loan updated successfully!';
        this.isLoading = false;
        setTimeout(() => {
          this.router.navigate(['/loans']);
          this.successMessage = '';
        }, 2000);
      },
      error: (error) => {
        console.log(error);
        this.errorMessage = error.error?.message || 'Failed to update loan';
        this.isLoading = false;
      }
    });
  }


  getDocumentUrl(path: string): string {
    return `${this.apiConfig.apiUrl + path}`;
  }

  hasDocument(path: string | null): boolean {
    return !!path && path.trim() !== '';
  }

}
