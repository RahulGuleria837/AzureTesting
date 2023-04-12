import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-update',
  templateUrl: './update.component.html',
  styleUrls: ['./update.component.scss']
})
export class UpdateComponent {
  selectedFile!: File;

  onFileSelected(event:any) {
    this.selectedFile = <File>event.target.files[0];
  }
  constructor(private http: HttpClient) {}
  
  onUpload() {
    const formData = new FormData();
    formData.append('file', this.selectedFile);
    console.log('data reading')
  
    this.http.post('http://localhost:7285/api/BlobContainer', formData).subscribe(
      (response) => {
        alert('File uploaded successfully')
        console.log('File uploaded successfully');
      },
      error => {
        
        console.error('Error uploading file', error);
      }
    );
  

    }}
