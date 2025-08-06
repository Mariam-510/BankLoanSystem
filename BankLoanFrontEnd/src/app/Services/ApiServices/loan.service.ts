import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { API_CONFIG } from '../../app.config';

@Injectable({
  providedIn: 'root'
})
export class LoanService {
  private apiUrl = `${API_CONFIG.apiUrl}api/Loans`;

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  getAllLoans(): Observable<any> {
    return this.http.get(this.apiUrl);
  }

  createLoan(loanData: any): Observable<any> {
    const formData = new FormData();
    formData.append('Amount', loanData.amount);
    formData.append('Duration', loanData.duration);
    formData.append('LoanTypeId', loanData.loanTypeId);
    if (loanData.nationalId) formData.append('NationalId', loanData.nationalId);
    if (loanData.salarySlip) formData.append('SalarySlip', loanData.salarySlip);

    if (!this.authService.hasRole('Admin')) {
      const user = this.authService.getCurrentUser();
      if (user) {
        formData.append('AppUserId', user.accountId);
      }
    }

    return this.http.post(this.apiUrl, formData);
  }

  getLoanById(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  updateLoan(id: number, loanData: any): Observable<any> {

    return this.http.put(`${this.apiUrl}/${id}`, loanData);
  }

  deleteLoan(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  getUserLoans(): Observable<any> {
    return this.http.get(`${this.apiUrl}/user`);
  }

  getLoansByType(loanTypeId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/type/${loanTypeId}`);
  }

  updateLoanStatus(id: number, status: number): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${id}/status`, { loanStatus: status });
  }
}
