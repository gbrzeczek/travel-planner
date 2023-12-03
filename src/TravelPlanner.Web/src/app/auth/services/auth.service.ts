import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly _apiUrl = `${environment.apiUrl}/account`;

  constructor(private readonly _client: HttpClient) {}

  public login(email: string, password: string): Observable<void> {
    const body = {
      email,
      password,
    };

    return this._client.post<void>(`${this._apiUrl}/login`, body);
  }
}
