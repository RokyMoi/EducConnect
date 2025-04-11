import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-course-lesson-dialog',
  imports: [],
  templateUrl: './course-lesson-dialog.component.html',
  styleUrl: './course-lesson-dialog.component.css',
})
export class CourseLessonDialogComponent {
  @Input() dialogTitle: string = 'Dialog title';
  @Output() closeModal: EventEmitter<void> = new EventEmitter<void>();

  
  emitCloseDialogEvent() {
    this.closeModal.emit();
  }
}
