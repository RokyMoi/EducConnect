import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { AccountService } from '../../../services/account.service';
import { PhotoComponent } from '../../Photo/photo-comp/photo-comp.component';
import { Message } from '../../../models/messenger/message';
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
  HandleFormStatusForMain(Message: boolean) {
    this.settingsControll = Message;
  }
  type = '';
  settingsControll: boolean = false;
  //Api Links For Editing
  FirstNameLink = 'http://localhost:5177/api/Settings/Student/ChangeFirstName';
  LastNameLink = 'http://localhost:5177/api/Settings/Student/ChangeLastName';
  DescriptionLink =
    'http://localhost:5177/api/Settings/Student/ChangeDescription';
  EmailLink = 'http://localhost:5177/api/Settings/Student/ChangeEmail';
  PasswordChangeLink =
    'http://localhost:5177/api/Settings/Student/changePassword';

  //////////////////////////
  statusController: boolean = false;
  HandleFormStatus(Message: boolean) {
    this.statusController = Message;
    if (!this.statusController) {
      this.LoadUserPhoto();
    }
  }
  http = inject(HttpClient);
  photoUrl = '';
  accountService = inject(AccountService);
  ngOnInit(): void {
    this.LoadUserPhoto();
  }

  LoadUserPhoto() {
    var token = this.accountService.getAccessToken();
    const headers = {
      Authorization: `Bearer ${token}`,
    };
    this.http
      .get<{ data: { url: string } }>(
        'http://localhost:5177/Photo/GetCurrentUserProfilePicture',
        { headers }
      )
      .subscribe({
        next: (response) => {
          this.photoUrl = response.data.url;
        },
      });
  }
  OpenPhotoEditor() {
    this.statusController = true;
  }
  OpenEditor(tip: string) {
    this.settingsControll = true;
    this.type = tip;
  }
}
