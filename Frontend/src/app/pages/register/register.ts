import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  user = {
    userName: '',
    password: ''
  };

  url = 'http://localhost:5259/api/';
  message = '';

  constructor(private router: Router) {}

  async register(): Promise<void> {
    if (this.user.userName.length < 3 || this.user.password.length == 0){
        this.message = 'Vänligen fyll i användarnamn och lösenord.'
        return;
    }
    const resp = await fetch(this.url + 'Auth/Register', {
      method: 'POST',
      body: JSON.stringify(this.user),
      headers: {
        'Content-Type': 'application/json'
      }
    });

    var data = await resp.json();
    
    this.message = resp.ok
      ? 'Kontot har skapats.'
      : `Registreringen misslyckades. \n${data.message}`;

    if (resp.ok) {
      await this.sleep(2500);
      await this.router.navigate(['/login']);
    }
  }

  sleep(ms: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }
}
