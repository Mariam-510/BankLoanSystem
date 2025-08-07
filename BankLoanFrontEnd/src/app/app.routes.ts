import { Routes } from '@angular/router';
import { LoginComponent } from './Components/Auth/login/login.component';
import { RegisterComponent } from './Components/Auth/register/register.component';
import { LoanListComponent } from './Components/Loan/loan-list/loan-list.component';
import { UserProfileComponent } from './Components/user-profile/user-profile.component';
import { AdminDashboardComponent } from './Components/Admin/admin-dashboard/admin-dashboard.component';
import { AdminGuard } from './Guards/admin.guard';
import { ConfirmEmailComponent } from './Components/Auth/confirm-email/confirm-email.component';
import { ForgotPasswordComponent } from './Components/AuthForgetPassword/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './Components/AuthForgetPassword/reset-password/reset-password.component';
import { ClientGuard } from './Guards/client.guard';
import { LoanApplicationComponent } from './Components/Loan/loan-application/loan-application.component';
import { CreateLoanTypeComponent } from './Components/LoanType/create-loan-type/create-loan-type.component';
import { EditLoanTypeComponent } from './Components/LoanType/edit-loan-type/edit-loan-type.component';
import { LoanTypeListComponent } from './Components/LoanType/loan-type-list/loan-type-list.component';
import { LoanDetailsComponent } from './Components/Admin/loan-details/loan-details.component';
import { AuthGuard } from './Guards/auth.guard';
import { LoanViewComponent } from './Components/Loan/loan-view/loan-view.component';
import { EditLoanComponent } from './Components/Loan/edit-loan/edit-loan.component';
import { ResetCodeComponent } from './Components/AuthForgetPassword/reset-code/reset-code.component';

export const routes: Routes = [

  // Auth routes
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'confirm-email', component: ConfirmEmailComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'reset-code', component: ResetCodeComponent },
  { path: 'reset-password', component: ResetPasswordComponent },

  // User routes
  { path: 'loans', component: LoanListComponent, canActivate: [AuthGuard] },
  { path: 'apply', component: LoanApplicationComponent, canActivate: [ClientGuard] },
  { path: 'loans/edit', component: EditLoanComponent, canActivate: [ClientGuard] },
  { path: 'loans/view', component: LoanViewComponent, canActivate: [ClientGuard] },
  { path: 'profile', component: UserProfileComponent, canActivate: [AuthGuard] },

  // Admin routes
  { path: 'admin/dashboard', component: AdminDashboardComponent, canActivate: [AdminGuard] },
  { path: 'admin/loans', component: LoanDetailsComponent, canActivate: [AdminGuard] },

  // Loan Types routes
  { path: 'loan-types', component: LoanTypeListComponent, canActivate: [AdminGuard] },
  { path: 'loan-types/create', component: CreateLoanTypeComponent, canActivate: [AdminGuard] },
  { path: 'loan-types/edit', component: EditLoanTypeComponent, canActivate: [AdminGuard] },

  { path: '', redirectTo: '/loans', pathMatch: 'full' },
  { path: '**', redirectTo: '/loans' }
];
