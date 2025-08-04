import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_CONFIG } from '../../app.config';

@Injectable({
  providedIn: 'root'
})
export class LoanTypeService {
  private apiUrl = `${API_CONFIG.apiUrl}api/LoanTypes`;


  constructor(private http: HttpClient) { }

  getAllLoanTypes(): Observable<any> {
    return this.http.get(this.apiUrl);
  }

  createLoanType(loanTypeData: { name: string }): Observable<any> {
    return this.http.post(this.apiUrl, loanTypeData);
  }

  getLoanTypeById(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  updateLoanType(id: number, loanTypeData: { name: string }): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, loanTypeData);
  }

  deleteLoanType(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
