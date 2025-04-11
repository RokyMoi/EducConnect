import { CommonModule, NgClass, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { SnackboxService } from '../../../services/shared/snackbox.service';

@Component({
  selector: 'app-snackbox',
  standalone: true,
  imports: [NgIf, NgClass],
  templateUrl: './snackbox.component.html',
  styleUrl: './snackbox.component.css',
})
export class SnackboxComponent {
  snack: { message: string; type: string } | null = null;

  constructor(private snackboxService: SnackboxService) {
    this.snackboxService.snackbox$.subscribe((snack) => {
      this.snack = snack;
    });
  }
}
