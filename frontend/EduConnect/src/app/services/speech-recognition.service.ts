import { Injectable, NgZone } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SpeechRecognitionService {
  private recognition: any;
  private isListening = false;
  private transcriptSubject = new BehaviorSubject<string>('');
  private transcript: string = '';

  constructor(private zone: NgZone) {
    const SpeechRecognition =
      (window as any).webkitSpeechRecognition ||
      (window as any).SpeechRecognition;

    if (!SpeechRecognition) {
      console.error('Speech recognition is not supported in this browser.');
      return;
    }

    this.recognition = new SpeechRecognition();
    this.recognition.continuous = true;
    this.recognition.lang = 'en-US';
    this.recognition.interimResults = false;

    this.recognition.onstart = () => {
      console.log('Voice recognition started. Speak now...');
    };

    this.recognition.onresult = (event: any) => {
      const tempTranscript = event.results[event.resultIndex][0].transcript;
      this.zone.run(() => {
        console.log('Transcript:', tempTranscript);
        this.transcript = this.transcript + tempTranscript;
        this.transcriptSubject.next(tempTranscript);
      });
    };

    this.recognition.onerror = (event: any) => {
      console.error('Recognition error:', event.error);
    };

    this.recognition.onend = () => {
      console.log('Voice recognition ended.');
      if (this.isListening) {
        this.recognition.start();
      }
    };
  }

  startListening() {
    this.isListening = true;
    this.transcriptSubject.next(''); // reset transcript
    this.recognition.start();
  }

  stopListening() {
    this.isListening = false;
    this.recognition.stop();
  }

  getTranscriptStream(): Observable<string> {
    return this.transcriptSubject.asObservable();
  }

  getFullTranscript(): string {
    return this.transcriptSubject.value;
  }
}
