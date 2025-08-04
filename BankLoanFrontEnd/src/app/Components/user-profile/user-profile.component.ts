import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../Services/ApiServices/auth.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  user: any;
  isAdmin = false;

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.loadUserProfile();
  }

  loadUserProfile(): void {
    this.user = this.authService.getCurrentUser();
    console.log(this.user)
    this.isAdmin = this.authService.hasRole('Admin');
  }

  logout(): void {
    this.authService.logout();
  }
}
