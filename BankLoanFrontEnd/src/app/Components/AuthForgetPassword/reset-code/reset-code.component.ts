import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../Services/ApiServices/auth.service';

@Component({
  selector: 'app-reset-code',
  imports: [ReactiveFormsModule, CommonModule, RouterModule, FormsModule],
  templateUrl: './reset-code.component.html',
  styleUrl: './reset-code.component.css'
})
export class ResetCodeComponent implements OnInit {
  resetCodeForm: FormGroup;
  errorMessage = '';
  successMessage = '';
  isLoading = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder
  ) {
    this.resetCodeForm = this.fb.group({
      code: ['', [Validators.required]]
    });
  }

  email!: string;
  ngOnInit(): void {
    this.email = this.route.snapshot.queryParams['email'];

    if (!this.email) {
      this.router.navigate(['/forgot-password']);
      return;
    }
  }


  onSubmit(): void {
    if (this.resetCodeForm.invalid || !this.email) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const validateData = {
      email: this.email,
      code: this.resetCodeForm.value.code
    };

    this.authService.validateResetCode(validateData).subscribe({
      next: () => {
        this.successMessage = 'Reset Code is correct! You can now reset password.';
        setTimeout(() => {
          this.router.navigate(['/reset-password'], {
            queryParams: { email: this.email }
          });
        }, 2000);
      },
      error: (error) => {
        this.errorMessage = error.error?.message || 'Failed to validate reset code';
        this.isLoading = false;
      }
    });
  }

  resendCode(): void {
    if (!this.email) {
      this.errorMessage = 'Email is required';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.forgotPassword(this.email).subscribe({
      next: () => {
        this.successMessage = 'Password reset instructions have been sent to your email';
      },
      error: (error) => {
        this.errorMessage = error.error?.message || 'Failed to send reset instructions';
      },
      complete: () => {
        this.isLoading = false;
      }
    });

  }
}
