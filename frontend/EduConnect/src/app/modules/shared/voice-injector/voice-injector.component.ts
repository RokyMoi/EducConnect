import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgClass, NgStyle } from '@angular/common';
import { SpeechRecognitionService } from '../../../services/speech-recognition.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-voice-injector',
  standalone: true,
  imports: [NgStyle, NgClass],
  templateUrl: './voice-injector.component.html',
  styleUrl: './voice-injector.component.css',
})
export class VoiceInjectorComponent implements OnInit, OnDestroy {
  isListening = false;
  transcription: string = '';
  private transcriptSub!: Subscription;
  private focusedElement: HTMLInputElement | HTMLTextAreaElement | null = null;

  constructor(private speechRecognitionService: SpeechRecognitionService) {}
  ngOnInit(): void {
    document.addEventListener('focusin', this.handleFocusIn.bind(this));
  }

  toggleListening() {
    if (this.isListening) {
      this.isListening = false;
      this.speechRecognitionService.stopListening();
      this.transcriptSub.unsubscribe();
    } else {
      this.isListening = true;
      this.transcription = '';
      this.speechRecognitionService.startListening();

      this.transcriptSub = this.speechRecognitionService
        .getTranscriptStream()
        .subscribe((text) => {
          this.transcription = text;
          if (this.focusedElement) {
            this.injectTranscriptToFocusedElement(text);
          }
        });
    }
  }

  ngOnDestroy(): void {
    if (this.transcriptSub) {
      this.transcriptSub.unsubscribe();
    }
    document.removeEventListener('focusin', this.handleFocusIn.bind(this));
  }

  private handleFocusIn(event: FocusEvent) {
    const target = event.target as HTMLElement;
    console.log('Focused element:', target);
    if (
      target instanceof HTMLInputElement ||
      target instanceof HTMLTextAreaElement
    ) {
      this.focusedElement = target;
    } else {
      this.focusedElement = null;
    }

    console.log('Focused element after check:', this.focusedElement);
  }

  private injectTranscriptToFocusedElement(text: string) {
    if (!this.focusedElement) return;

    const start = this.focusedElement.selectionStart || 0;
    const end = this.focusedElement.selectionEnd || 0;
    const currentValue = this.focusedElement.value;

    const newValue =
      currentValue.substring(0, start) + text + currentValue.substring(end);

    this.focusedElement.value = newValue;

    const newCursorPosition = start + text.length;
    this.focusedElement.selectionStart = newCursorPosition;

    this.focusedElement.dispatchEvent(new Event('input'));
  }
}
