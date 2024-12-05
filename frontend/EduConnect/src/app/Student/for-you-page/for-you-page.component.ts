import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatCardModule } from '@angular/material/card';

import { MatToolbarModule } from '@angular/material/toolbar';


@Component({
  selector: 'app-for-you-page',
  standalone: true,
  imports: [MatCardModule, MatButtonModule, MatDividerModule, MatIconModule,MatToolbarModule],
  templateUrl: './for-you-page.component.html',
  styleUrls: ['./for-you-page.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ForYouPageComponent {
 
    
    images: any;
    prevImage() {
    throw new Error('Method not implemented.');
    }
    nextImage() {
    throw new Error('Method not implemented.');
    }


  
  
}
