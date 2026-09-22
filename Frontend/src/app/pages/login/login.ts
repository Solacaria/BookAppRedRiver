import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  user = {
    userName: '',
    password: ''
  };
  message = "";

  constructor(private router: Router) {}

  url = 'http://localhost:5259/api/';

    async login(): Promise<void>{
      if (this.user.userName.length < 3 || this.user.password.length == 0){
        this.message = 'Vänligen fyll i användarnamn och lösenord.'
        return;
    }
      var resp = await fetch(this.url + 'Auth/Login',{
        method: 'POST',
        body: JSON.stringify(this.user), 
        headers: {
          'Content-Type' : 'application/json'
        },
      });

      var data = await resp.json();

      if (!resp.ok) {
        this.message = data.message;
        return;
      }
      
      localStorage.setItem('token', data.token);

      if (resp.ok) {
      this.message = "Inloggning lyckades!"
      await this.sleep(1500);
      await this.router.navigate(['/books']);
    }
    }

    sleep(ms: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }
}
