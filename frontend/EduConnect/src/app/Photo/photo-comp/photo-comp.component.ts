import { Component, inject } from '@angular/core';
import { HttpClient, HttpEventType, HttpHeaders, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../../services/account.service';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-photo-comp', 
  standalone:true,
  imports:[NgIf],
  templateUrl: './photo-comp.component.html',
  styleUrls: ['./photo-comp.component.css']
})
export class PhotoComponent { 
  selectedFile: File | null = null;
  uploadProgress: number = 0;
  imagePreview: string | ArrayBuffer | null = null;
  accountService = inject(AccountService);
  constructor(private http: HttpClient) {}

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result;
      };
      reader.readAsDataURL(file);
    }
  }

  onSubmit(): void {
    if (this.selectedFile) {
      const formData = new FormData();
      formData.append('file', this.selectedFile, this.selectedFile.name);
      const token = this.accountService.getAccessToken();
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      });

      const uploadReq = new HttpRequest('POST', 'http://localhost:5177/Photo/addPersonProfilePicture', formData, {
        headers: headers,
        reportProgress: true,
      });

      this.http.request(uploadReq).subscribe(event => {
        switch (event.type) {
          case HttpEventType.UploadProgress:
            if (event.total) {
              this.uploadProgress = Math.round((100 * event.loaded) / event.total);
            }
            break;
          case HttpEventType.Response:
            console.log('Upload complete', event.body);
            break;
        }
      });
    }
  }
}