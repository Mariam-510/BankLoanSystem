import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../Services/ApiServices/auth.service';

@Component({
  selector: 'app-confirm-email',
  imports: [ReactiveFormsModule, CommonModule, RouterModule, FormsModule],
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.css']
})
export class ConfirmEmailComponent implements OnInit {
  confirmForm: FormGroup;
  errorMessage = '';
  successMessage = '';
  isLoading = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder
  ) {
    this.confirmForm = this.fb.group({
      code: ['', [Validators.required]]
    });
  }

  email!: string;
  ngOnInit(): void {
    this.email = this.route.snapshot.queryParams['email'];

    if (!this.email) {
      this.router.navigate(['/register']);
      return;
    }
  }


  onSubmit(): void {
    if (this.confirmForm.invalid || !this.email) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const confirmData = {
      email: this.email,
      code: this.confirmForm.value.code
    };

    this.authService.confirmEmail(confirmData).subscribe({
      next: () => {
        this.successMessage = 'Email confirmed successfully! You can now login.';
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },
      error: (error) => {
        this.errorMessage = error.error?.message || 'Failed to confirm email';
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

    this.authService.resendConfirmEmail(this.email).subscribe({
      next: () => {
        this.successMessage = 'Confirmation code has been resent to your email';
      },
      error: (error) => {
        this.errorMessage = error.error?.message || 'Failed to resend confirmation code';
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }
}
