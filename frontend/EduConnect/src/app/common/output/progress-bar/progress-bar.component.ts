import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-progress-bar',
  imports: [],
  templateUrl: './progress-bar.component.html',
  styleUrl: './progress-bar.component.css',
  standalone: true,
})
export class ProgressBarComponent {
  @Input() color: string = 'blue';
  @Input() progress: string = '70%';
  @Input() backgroundColor: string = '#e2e2e2';
  @Input() outsideRadius: string = '12px';
  @Input() insideRadius: string = '12px';
  @Input() overallWidth: string = '100%';
  @Input() marginTop: string = '5px';
  @Input() marginBottom: string = '5px';
  constructor() {}

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
  }
}
