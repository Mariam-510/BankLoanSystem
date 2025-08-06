import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { AuthService } from './Services/ApiServices/auth.service';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from "./Components/header/header.component";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule, RouterModule, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Loan System';
  isAuthenticated = false;
  isAdmin = false;

  constructor(private authService: AuthService) {
    this.authService.currentUser$.subscribe(user => {
      this.isAuthenticated = !!user;
      this.isAdmin = this.authService.hasRole('Admin');
    });
  }

  logout(): void {
    this.authService.logout();
  }
}
