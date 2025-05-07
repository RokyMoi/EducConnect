import { Component, NgZone } from '@angular/core';
import { NgStyle } from '@angular/common';
import { SpeechRecognitionService } from '../../../services/speech-recognition.service';

@Component({
  selector: 'app-voice-injector',
  standalone: true,
  imports: [NgStyle],
  templateUrl: './voice-injector.component.html',
  styleUrl: './voice-injector.component.css',
})
export class VoiceInjectorComponent {
  recognition: any;
  isListening = false;
  transcription: string = '';

  constructor(private speechRecognitionService: SpeechRecognitionService) {}

  toggleListening() {
    if (this.isListening) {
      this.isListening = false;
      this.speechRecognitionService.stopListening();
      this.transcription = this.speechRecognitionService.getTranscript();
      console.log('Transcription from component:', this.transcription);
    } else {
      this.isListening = true;
      this.transcription = '';
      this.speechRecognitionService.startListening();
    }
  }
}
