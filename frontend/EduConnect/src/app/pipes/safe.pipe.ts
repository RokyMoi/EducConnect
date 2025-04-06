import { Pipe, PipeTransform, Sanitizer } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Pipe({
  name: 'safe',
  standalone: true,
})
export class SafePipe implements PipeTransform {
  constructor(private sanitizer: DomSanitizer) {}
  transform(value: string): SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl(value);
  }
}
