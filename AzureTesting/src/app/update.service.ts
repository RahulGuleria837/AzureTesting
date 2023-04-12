import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Update } from './update';
selectedFile: File;

@Injectable({
  providedIn: 'root'
})
export class UpdateService {
  private apiUrl = 'http://localhost:7285/api/BlobContainer';

  constructor(private httpClient:HttpClient) {}

  uploadFile(file: File) {
    const formData = new FormData();
    formData.append('file', file);

    return this.httpClient.post(this.apiUrl, formData);
  }
 
}
