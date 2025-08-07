import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LoanTypeService } from '../../../Services/ApiServices/loan-type.service';

@Component({
  selector: 'app-edit-loan-type',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './edit-loan-type.component.html',
  styleUrls: ['./edit-loan-type.component.css']
})
export class EditLoanTypeComponent implements OnInit {
  loanTypeForm: FormGroup;
  errorMessage = '';
  successMessage = '';
  typeId: number;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private loanTypeService: LoanTypeService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.loanTypeForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]]
    });
    this.typeId = 0;
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.typeId = Number(params['id']);
      if (this.typeId) {
        this.loadLoanType();
      } else {
        this.errorMessage = 'No loan type ID provided';
        this.router.navigate(['/loan-types']);
      }
    });
  }

  loadLoanType(): void {
    this.isLoading = true;
    this.loanTypeService.getLoanTypeById(this.typeId).subscribe({
      next: (data) => {
        this.loanTypeForm.patchValue({
          name: data.data.name
        });
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load loan type';
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.loanTypeForm.invalid) {
      return;
    }

    this.isLoading = true;
    this.loanTypeService.updateLoanType(this.typeId, this.loanTypeForm.value)
      .subscribe({
        next: () => {
          this.errorMessage = '';
          this.successMessage = 'Loan type updated successfully';
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
