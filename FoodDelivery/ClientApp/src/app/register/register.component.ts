import { Component, OnInit, Type } from '@angular/core';
import { LoginModel } from '../models/login.model';
import { RegisterModel } from '../models/register.model';
import { AuthService } from '../services/auth.service';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(private authService: AuthService, private notifier: NotifierService) { }

  ngOnInit() {
    this.isError = false;
  }

  model = new RegisterModel();
  isError: boolean;

  onSubmit(){
    if (this.model.username === null) {
      this.notifier.notify('error', 'Поле "Ім\'я та прізвище" пусте!');
      this.isError = true;
    }
    if (this.model.firstName === null) {
      this.notifier.notify('error', 'Поле "Вік" пусте!');
      this.isError = true;
    }
    if (this.model.lastName === null) {
      this.notifier.notify('error', 'Поле "Телефон" пусте!');
      this.isError = true;
    }
    if(this.model.email === null){
      this.notifier.notify('error', 'Поле "Email" пусте!');
      this.isError = true;
    }
    if(this.model.password === null) {
      this.notifier.notify('error', 'Поле "Пароль" пусте!');
      this.isError = true;
    }
    if (this.isError == false) {
      this.authService.Register(this.model).subscribe(
        data => {
          console.log(data);
            this.notifier.notify('success', 'You registered!');        
        }
      )
    }
  }




}
