import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { LoginRequest } from './models/login-request.model';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  private readonly _url = `${environment.baseApiUrl}/account/login`;
  constructor(private readonly _http: HttpClient) {}

  public login(request: LoginRequest): Observable<void> {
    return this._http.post<void>(this._url, request);
  }
}
