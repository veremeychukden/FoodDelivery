import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { LoginResponseModel } from '../models/login-response.model';
import { LoginModel } from '../models/login.model';
import { RegisterResponseModel } from '../models/register-response.model';
import { RegisterModel } from '../models/register.model';
import jwt_decode from 'jwt-decode';
import { UserModel } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  baseUrl: string = '/api/user';


  Register(model: RegisterModel): Observable<RegisterResponseModel>{
    return this.http.post<RegisterResponseModel>(this.baseUrl + '/register', model);
  }

  Login(model: LoginModel): Observable<LoginResponseModel>{
    return this.http.post<LoginResponseModel>(this.baseUrl + '/login', model);
  }

  isLoggedIn(){
    if(localStorage.getItem('token') ==  null){
      return false;
    }
    return true;
  }

  isOwner(){
    if(this.isLoggedIn()){
      const token = localStorage.getItem('token');
      var role = (jwt_decode(token) as UserModel).role;
      if(role == 'Owner'){
        return true;
      }
      return false;
    }
  }

  isManager(){
    if(this.isLoggedIn()){
      const token = localStorage.getItem('token');
      var role = (jwt_decode(token) as UserModel).role;
      if(role == 'Manager'){
        return true;
      }
      return false;
    }
  }

  isWorker(){
    if(this.isLoggedIn()){
      const token = localStorage.getItem('token');
      var role = (jwt_decode(token) as UserModel).role;
      if(role == 'Worker'){
        return true;
      }
      return false;
    }
  }
  
  isUser(){
    if(this.isLoggedIn()){
      const token = localStorage.getItem('token');
      var role = (jwt_decode(token) as UserModel).role;
      if(role == 'User'){
        return true;
      }
      return false;
    }
  }

}
