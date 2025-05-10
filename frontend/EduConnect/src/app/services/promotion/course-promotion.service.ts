// src/app/services/promotion.service.ts

import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  CoursePromotion,
  CoursePromotionDetail,
  CreatePromotionDto,
  UpdatePromotionDto,
  UpdateStatusDto,
  PromotionStatus
} from '../../models/promotionCourse/promotion.model';
import { AccountService } from '../account.service';

@Injectable({
  providedIn: 'root'
})
export class PromotionService {
  private apiUrl = `http://localhost:5177/api/Promotions`;

  constructor(
    private http: HttpClient,
    private accountService: AccountService
  ) { }

  private getAuthHeaders(): HttpHeaders {
    const token = this.accountService.getAccessToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
      // Removed Content-Type header to allow proper FormData boundaries
    });
  }

  private getJsonAuthHeaders(): HttpHeaders {
    const token = this.accountService.getAccessToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  getAllPromotions(): Observable<CoursePromotion[]> {
    return this.http.get<CoursePromotion[]>(this.apiUrl, {
      headers: this.getJsonAuthHeaders()
    });
  }
  getPost(){
   return this.http.get("http://localhost:5177/AllPromotions",{headers: this.getJsonAuthHeaders()});
  }

  getPromotionById(id: string): Observable<CoursePromotionDetail> {
    return this.http.get<CoursePromotionDetail>(`${this.apiUrl}/${id}`, {
      headers: this.getJsonAuthHeaders()
    });
  }

  createPromotion(promotionData: CreatePromotionDto, files: File[]): Observable<any> {
    const formData = new FormData();

    // Add basic promotion data - match casing with backend DTO properties
    formData.append('CourseId', promotionData.courseId);
    formData.append('Title', promotionData.title);
    formData.append('Description', promotionData.description);
    formData.append('StartDate', promotionData.startDate.toString());
    formData.append('EndDate', promotionData.endDate.toString());

    // Add images if available - match casing with backend DTO property
    if (files && files.length > 0) {
      for (let i = 0; i < files.length; i++) {
        formData.append('Images', files[i], files[i].name);
      }
    }

    return this.http.post<any>(this.apiUrl, formData, {
      headers: this.getAuthHeaders()
    });
  }

  updatePromotion(promotionData: UpdatePromotionDto, newFiles?: File[]): Observable<any> {
    const formData = new FormData();

    // Add basic promotion data - match casing with backend DTO properties
    formData.append('PromotionId', promotionData.promotionId);
    formData.append('Title', promotionData.title);
    formData.append('Description', promotionData.description);

    if (promotionData.status !== undefined) {
      formData.append('Status', promotionData.status.toString());
    }

    if (promotionData.startDate) {
      formData.append('StartDate', promotionData.startDate.toString());
    }

    if (promotionData.endDate) {
      formData.append('EndDate', promotionData.endDate.toString());
    }

    // Add main image ID if specified
    if (promotionData.mainImageId) {
      formData.append('MainImageId', promotionData.mainImageId);
    }

    // Add image IDs to remove if specified
    if (promotionData.removeImageIds && promotionData.removeImageIds.length > 0) {
      promotionData.removeImageIds.forEach((id, index) => {
        formData.append(`RemoveImageIds[${index}]`, id);
      });
    }

    // Add new images if available
    if (newFiles && newFiles.length > 0) {
      for (let i = 0; i < newFiles.length; i++) {
        formData.append('NewImages', newFiles[i], newFiles[i].name);
      }
    }

    return this.http.put<any>(`${this.apiUrl}/${promotionData.promotionId}`, formData, {
      headers: this.getAuthHeaders()
    });
  }

  deletePromotion(id: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`, {
      headers: this.getJsonAuthHeaders()
    });
  }

  setMainImage(promotionId: string, imageId: string): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/SetMainImage/${promotionId}/${imageId}`, {}, {
      headers: this.getJsonAuthHeaders()
    });
  }

  updateStatus(id: string, status: PromotionStatus): Observable<any> {
    const statusDto: UpdateStatusDto = { status };
    return this.http.put<any>(`${this.apiUrl}/UpdateStatus/${id}`, statusDto, {
      headers: this.getJsonAuthHeaders()
    });
  }

  getImageUrl(imageId: string): string {
    return `${this.apiUrl}/Image/${imageId}`;
  }
}
