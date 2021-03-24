import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { LoginModel } from '../models/login.model';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private authService: AuthService,
    private notifier: MessageService,
    private router: Router,
    ) { }

  ngOnInit() {
  }

  model = new LoginModel();

  Login(){
    if(this.validateEmail(this.model.email)){
      this.authService.Login(this.model).subscribe(
        data => {
          if(data.success){
            this.notifier.add({severity: 'success', summary: 'Успішно', detail: 'Ви успішно авторизувались'});
            localStorage.setItem('token', data.token);
            this.router.navigate(['/home']);
          }
          else if(data.success === false){
            for(var i = 0; i < data.errors.length; i++){
            this.notifier.add({severity: 'error', summary: 'Помилка', detail: 'Пошти не існує'});
           }
          }      
        }
      )
    }
    else{
      this.notifier.add({severity: 'warn', summary: 'Попередження', detail: 'Введіть корректно пошту'});
    }
  }

  validateEmail(email: string) {
    const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

}
