import { Component, inject, Input, OnInit } from '@angular/core';
import { SelectDropdownComponent } from '../../../common/select/select-dropdown/select-dropdown.component';
import { SubmitButtonComponent } from '../../../common/button/submit-button/submit-button.component';
import { ReferenceService } from '../../../services/reference/reference.service';
import { Language } from '../../../_models/reference/language/language.model';
import { FormControl, Form, Validators } from '@angular/forms';
import { NgFor, NgIf } from '@angular/common';
import { CourseCreateService } from '../../../services/course/course-create-service.service';

@Component({
  standalone: true,
  selector: 'app-course-supported-languages',
  imports: [SelectDropdownComponent, SubmitButtonComponent, NgFor, NgIf],
  templateUrl: './course-supported-languages.component.html',
  styleUrl: './course-supported-languages.component.css',
})
export class CourseSupportedLanguagesComponent implements OnInit {
  @Input() referenceService!: ReferenceService;
  @Input() createCourseService!: CourseCreateService;
  @Input() courseId!: string;

  @Input() componentTitle: string =
    'Select supported languages for this course';

  //Variables for the select dropdown
  selectLanguageDropdownLabel: string = 'Select a language';
  selectLanguageDropdownPlaceholder: string = 'Select a language';

  //Variables for the add language button
  addLanguageButtonText: string = 'Add selected language';
  addLanguageButtonColor: string = 'green';

  selectedLanguages: Language[] = [];
  supportedLanguages: Language[] = [];
  supportedLanguagesOptions: { name: string; value: string }[] = [];

  supportedLanguageFormControl: FormControl = new FormControl('', [
    Validators.required,
  ]);

  warningText: string = '';
  ngOnInit(): void {
    console.log('Course id: ', this.courseId);
    this.loadSupportedLanguages();
    this.loadCourseSupportedLanguages();
  }

  loadCourseSupportedLanguages() {
    this.createCourseService
      .getLanguagesSupportedByCourse(this.courseId)
      .subscribe((response) => {
        if (response.success === 'true') {
          this.selectedLanguages = response.data;
          console.log('Selected languages: ', this.selectedLanguages);
          this.refreshSupportedLanguagesList();
        }
      });
  }

  refreshSupportedLanguagesList() {
    const selectedLanguagesIds = this.selectedLanguages.map(
      (language) => language.languageId
    );

    this.supportedLanguagesOptions = this.supportedLanguages
      .filter((language) => !selectedLanguagesIds.includes(language.languageId))
      .map((language) => {
        return {
          name: `${language.name} (${language.code})`,
          value: language.languageId,
        };
      });
    this.supportedLanguageFormControl.setValue('');
  }
  loadSupportedLanguages() {
    this.referenceService.getAllLanguages().subscribe((response) => {
      console.log('Response', response.message);
      if (response.success === 'true') {
        this.supportedLanguages = response.data;
        console.log(this.supportedLanguages);
        this.supportedLanguagesOptions = this.supportedLanguages.map(
          (language) => {
            return {
              name: `${language.name} (${language.code})`,
              value: language.languageId,
            };
          }
        );
      }
    });
  }

  addSelectedLanguage() {
    console.log(this.supportedLanguageFormControl.value);
    var selectedLanguage = this.supportedLanguages.find(
      (language) =>
        language.languageId === this.supportedLanguageFormControl.value
    );
    if (!selectedLanguage) {
      return;
    }

    console.log('Course id:', this.courseId);
    console.log('Selected language: ', selectedLanguage.languageId);
    this.createCourseService
      .addSupportedLanguage(this.courseId, selectedLanguage.languageId)
      .subscribe((response) => {
        if (response.success === 'false') {
          this.warningText = response.message;
          return;
        }
      });

    console.log(selectedLanguage);
    this.selectedLanguages.push(selectedLanguage);
    console.log(this.selectedLanguages);
    this.refreshSupportedLanguagesList();
  }

  removeLanguage(languageId: string) {
    this.createCourseService
      .removeSupportedLanguage(this.courseId, languageId)
      .subscribe((response) => {
        console.log(response);
      });
    this.selectedLanguages = this.selectedLanguages.filter(
      (language) => language.languageId !== languageId
    );
    console.log(this.selectedLanguages);
    this.refreshSupportedLanguagesList();
  }
}
