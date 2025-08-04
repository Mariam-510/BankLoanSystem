import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';
import { BehaviorSubject, Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { API_CONFIG, appConfig } from '../../app.config';

interface DecodedToken {
  sub: string;
  exp: number;
  email: string;
  firstName: string;
  lastName: string;
  roles: string | string[];
}

export interface User {
  accountId: string;
  email: string;
  firstName: string;
  lastName: string;
  roles: string | string[];
  tokenExpiration: Date;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${API_CONFIG.apiUrl}api/Auth`;

  private currentUserSubject = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();
  private tokenExpirationTimer: any;

  constructor(private router: Router, private http: HttpClient) {
    this.initializeAuthState();
  }

  private initializeAuthState() {
    const token = localStorage.getItem('jwtToken') || sessionStorage.getItem('jwtToken');
    if (token) {
      const rememberMe = localStorage.getItem('jwtToken') !== null;
      this.setAuthState(token, rememberMe);
    }
  }

  setAuthState(token: string, rememberMe: boolean) {
    if (rememberMe) {
      localStorage.setItem('jwtToken', token);
      sessionStorage.removeItem('jwtToken');
    } else {
      sessionStorage.setItem('jwtToken', token);
      localStorage.removeItem('jwtToken');
    }

    const decoded = jwtDecode<DecodedToken>(token);
    const currentUser: User = {
      accountId: decoded.sub,
      email: decoded.email,
      firstName: decoded.firstName,
      lastName: decoded.lastName,
      roles: decoded.roles,
      tokenExpiration: new Date(decoded.exp * 1000)
    };

    this.currentUserSubject.next(currentUser);
    this.setAutoLogout(decoded.exp);
  }

  private setAutoLogout(expirationTime: number) {
    const expiresIn = expirationTime * 1000 - Date.now();
    this.tokenExpirationTimer = setTimeout(() => {
      this.logout(true);
    }, expiresIn);
  }

  register(registerData: any): Observable<any> {
    const formData = new FormData();
    formData.append('FirstName', registerData.firstName);
    formData.append('LastName', registerData.lastName || '');
    formData.append('Email', registerData.email);
    formData.append('Password', registerData.password);
    formData.append('ConfirmPassword', registerData.confirmPassword);
    return this.http.post(`${this.apiUrl}/Register`, formData);
  }

  login(credentials: { email: string, password: string }, rememberMe: boolean): Observable<any> {
    // console.log(credentials.email);
    // console.log(credentials.password);
    return this.http.post(`${this.apiUrl}/login`, credentials).pipe(
      tap((response: any) => {
        // console.log(response);
        this.setAuthState(response.data.jwtToken, rememberMe);
      })
    );
  }


  logout(isExpired = false) {
    localStorage.removeItem('jwtToken');
    sessionStorage.removeItem('jwtToken');
    this.currentUserSubject.next(null);
    if (this.tokenExpirationTimer) {
      clearTimeout(this.tokenExpirationTimer);
    }

    // this.http.delete(this.apiUrl).subscribe();

    if (isExpired) {
      this.router.navigate(['/login'], { state: { message: 'Session expired. Please login again.' } });
    } else {
      this.router.navigate(['/login']);
    }
  }

  getToken(): string | null {
    return localStorage.getItem('jwtToken') || sessionStorage.getItem('jwtToken');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  hasRole(requiredRole: string): boolean {
    const user = this.currentUserSubject.value;
    if (!user?.roles) return false;
    return Array.isArray(user.roles)
      ? user.roles.includes(requiredRole)
      : user.roles === requiredRole;
  }

  updateToken(newToken: string) {
    const rememberMe = localStorage.getItem('jwtToken') !== null;
    localStorage.removeItem('jwtToken');
    sessionStorage.removeItem('jwtToken');

    if (rememberMe) {
      localStorage.setItem('jwtToken', newToken);
    } else {
      sessionStorage.setItem('jwtToken', newToken);
    }

    const decoded = jwtDecode<DecodedToken>(newToken);
    const currentUser: User = {
      accountId: decoded.sub,
      email: decoded.email,
      firstName: decoded.firstName,
      lastName: decoded.lastName,
      roles: decoded.roles,
      tokenExpiration: new Date(decoded.exp * 1000)
    };

    this.currentUserSubject.next(currentUser);
    this.setAutoLogout(decoded.exp);
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  // Additional auth methods
  createAdmin(adminData: any): Observable<any> {
    const formData = new FormData();
    formData.append('FirstName', adminData.firstName);
    formData.append('LastName', adminData.lastName || '');
    formData.append('Email', adminData.email);
    formData.append('Password', adminData.password);
    formData.append('ConfirmPassword', adminData.confirmPassword);
    return this.http.post(`${this.apiUrl}/CreateAdmin`, formData);
  }

  confirmEmail(confirmData: { email: string, code: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/ConfirmEmailCode`, confirmData);
  }

  resendConfirmEmail(email: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/ResendConfirmEmail`, { email });
  }

  forgotPassword(email: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/ForgotPassword`, { email });
  }

  validateResetCode(validateData: { email: string, code: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/ValidateResetCode`, validateData);
  }

  resetPassword(resetData: { email: string, newPassword: string, confirmPassword: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/ResetPassword`, resetData);
  }
}
