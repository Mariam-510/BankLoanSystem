// loan-type-edit.component.ts
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LoanTypeService } from '../../../Services/ApiServices/loan-type.service';

@Component({
  selector: 'app-edit-loan-type',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './edit-loan-type.component.html',
  styleUrl: './edit-loan-type.component.css'
})
export class EditLoanTypeComponent implements OnInit {
  loanTypeForm: FormGroup;
  errorMessage = '';
  successMessage = '';
  typeId: number;

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
    this.typeId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadLoanType();
  }

  loadLoanType(): void {
    this.loanTypeService.getLoanTypeById(this.typeId).subscribe({
      next: (data) => {
        this.loanTypeForm.patchValue({
          name: data.data.name
        });
      },
      error: (error) => {
        this.errorMessage = 'Failed to load loan type';
      }
    });
  }

  onSubmit(): void {
    if (this.loanTypeForm.invalid) {
      return;
    }

    this.loanTypeService.updateLoanType(this.typeId, this.loanTypeForm.value)
      .subscribe({
        next: () => {
          this.successMessage = 'Loan type updated successfully';
          setTimeout(() => {
            this.router.navigate(['/loan-types']);
          }, 2000);
        },
        error: (error) => {
          this.errorMessage = error.error?.message || 'Failed to update loan type';
        }
      });
  }
}
