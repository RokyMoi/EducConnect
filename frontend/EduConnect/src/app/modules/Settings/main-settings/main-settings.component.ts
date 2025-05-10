import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { AccountService } from '../../../services/account.service';
import { PhotoComponent } from '../../Photo/photo-comp/photo-comp.component';
import { NgIf } from '@angular/common';
import { DynamicFormComponent } from '../../SetingsStudent/dynamic-form/dynamic-form.component';

@Component({
  selector: 'app-main-settings',
  standalone: true,
  imports: [PhotoComponent, NgIf, DynamicFormComponent],
  templateUrl: './main-settings.component.html',
  styleUrl: './main-settings.component.css',
})
export class MainSettingsComponent implements OnInit {
  student: any;

  // Form control variables
  type = '';
  settingsControll = false;
  statusController = false;

  // API Links For Editing
  FirstNameLink = 'http://localhost:5177/api/Settings/Student/ChangeFirstName';
  LastNameLink = 'http://localhost:5177/api/Settings/Student/ChangeLastName';
  DescriptionLink = 'http://localhost:5177/api/Settings/Student/ChangeDescription';
  EmailLink = 'http://localhost:5177/api/Settings/Student/ChangeEmail';
  PasswordChangeLink = 'http://localhost:5177/api/Settings/Student/changePassword';
  PhoneLink = 'http://localhost:5177/api/Settings/Student/ChangePhone';

  // Service injections
  http = inject(HttpClient);
  accountService = inject(AccountService);

  // Profile photo URL
  photoUrl = '';

  ngOnInit(): void {
    this.LoadUserPhoto();
    this.GetStudentInformations();
  }

  /**
   * Handle form status changes from dynamic forms
   * This is called when any dynamic form closes
   */
  HandleFormStatusForMain(formIsOpen: boolean): void {
    this.settingsControll = formIsOpen;

    if (!formIsOpen) {
      // Form was just closed - refresh the student data
      console.log('Form closed, refreshing student data...');
      // Add small delay to ensure backend processing completes
      setTimeout(() => {
        this.GetStudentInformations();
      }, 300);
    }
  }

  /**
   * Handle photo component status changes
   */
  HandleFormStatus(photoComponentIsOpen: boolean): void {
    this.statusController = photoComponentIsOpen;

    if (!photoComponentIsOpen) {
      // Photo editor was closed, refresh the photo
      console.log('Photo editor closed, refreshing photo...');
      this.LoadUserPhoto();
    }
  }

  /**
   * Fetch student information from the API
   */
  GetStudentInformations(): void {
    const token = this.accountService.getAccessToken();
    const headers = {
      'Authorization': `Bearer ${token}`,
    };
    const LinkProfile = 'http://localhost:5177/api/Student/getCurrentStudentWithPhoto';

    console.log('Fetching student information...');
    this.http.get(LinkProfile, { headers }).subscribe({
      next: (response) => {
        this.student = response;
        console.log("Student data refreshed:", this.student);
      },
      error: (err) => {
        console.error("Error fetching student data:", err);
      }
    });
  }

  /**
   * Load the user's profile photo
   */
  LoadUserPhoto(): void {
    const token = this.accountService.getAccessToken();
    const headers = {
      Authorization: `Bearer ${token}`,
    };

    console.log('Loading user photo...');
    this.http
      .get<{ data: { url: string } }>(
        'http://localhost:5177/Photo/GetCurrentUserProfilePicture',
        { headers }
      )
      .subscribe({
        next: (response) => {
          this.photoUrl = response.data.url;
          console.log('Photo URL updated:', this.photoUrl);
        },
        error: (err) => {
          console.error('Error loading user photo:', err);
        }
      });
  }

  /**
   * Open the photo editor
   */
  OpenPhotoEditor(): void {
    this.statusController = true;
  }

  /**
   * Open the specified editor type
   */
  OpenEditor(tip: string): void {
    this.settingsControll = true;
    this.type = tip;
  }
}
