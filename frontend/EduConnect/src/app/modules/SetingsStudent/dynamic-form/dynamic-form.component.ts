import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  inject
} from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { CommonModule, NgIf } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AccountService } from '../../../services/account.service';
import { Observable, finalize } from 'rxjs';

@Component({
  selector: 'app-dynamic-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NgIf],
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.css'],
})
export class DynamicFormComponent implements OnInit {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);

  dynamicForm!: FormGroup;
  isSubmitting = false;
  errorMessage = '';

  @Input() valueNameChild = '';
  @Input() ApiLink = '';
  @Input() fieldLabel = '';
  @Output() FormStatus = new EventEmitter<boolean>();

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.createForm();
  }

  /**
   * Create form with appropriate validators based on field type
   */
  private createForm(): void {
    let validators = [Validators.required];

    // Add specific validators based on field type
    if (this.fieldLabel === 'Password') {
      validators.push(Validators.minLength(8));
    } else if (this.fieldLabel === 'Phone') {
      // Basic phone validation - adjust based on your requirements
      validators.push(Validators.pattern(/^[0-9+\-\s]*$/));
    }

    this.dynamicForm = this.fb.group({
      userInput: new FormControl(this.valueNameChild, validators)
    });
  }

  get userInput() {
    return this.dynamicForm.get('userInput');
  }

  /**
   * Close the form
   */
  ChangeFormStatus() {
    this.FormStatus.emit(false);
  }

  /**
   * Handle form submission
   */
  onSubmit() {
    if (this.dynamicForm.invalid) {
      this.dynamicForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    const inputVal = this.userInput!.value as string;

    this.uploadResult(inputVal).pipe(
      finalize(() => this.isSubmitting = false)
    ).subscribe({
      next: (response) => {
        console.log(`Successfully updated ${this.fieldLabel}:`, response);
        // Signal to parent that form has been submitted and closed
        this.FormStatus.emit(false);
      },
      error: (err) => {
        console.error(`Failed to update ${this.fieldLabel}:`, err);
        this.errorMessage = err.error?.message || 'Update failed. Please try again.';
      }
    });
  }

  /**
   * Make the API call to update the value
   */
  private uploadResult(input: string): Observable<any> {
    const token = this.accountService.getAccessToken();
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });

    const url = `${this.ApiLink}?input=${encodeURIComponent(input)}`;
    return this.http.post(url, {}, { headers });
  }
}
