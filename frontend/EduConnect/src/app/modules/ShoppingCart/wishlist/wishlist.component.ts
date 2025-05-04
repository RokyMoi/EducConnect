
import {Component, inject, OnInit} from '@angular/core';
import  {CommonModule} from '@angular/common';
import { Router } from '@angular/router';
import {MatSnackBar} from '@angular/material/snack-bar';
import {SnackboxService} from '../../../services/shared/snackbox.service';
import {WishlistService} from '../../../services/Shopping-Cart/wishlist.service';

@Component({
  selector: 'app-wishlist',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './wishlist.component.html',
  styleUrl: './wishlist.component.css'
})
export class WishlistComponent implements OnInit{
  kursevi:any;
  constructor(private router: Router,
              private SnackBox: SnackboxService,
              private wishlistService: WishlistService) {

  }

  ngOnInit(): void {
        this.UcitajKurseve();
    }
  MoveToShoppingCart(){
    this.router.navigate(['/cart-dwe']);
  }
  UcitajKurseve(){
    this.wishlistService.CoursePrint().subscribe({
      next: (response) => {
        console.log('Course print response:', response);
        this.kursevi = response;
      },
      error: (err) => {
        console.error('Error fetching course print:', err);
      }
    });
  }

  removeCourse(courseId: any) {
 this.wishlistService.removeCourseFromWishlist(courseId).subscribe({
   next: response=> {
     this.SnackBox.showSnackbox("Uspjesno ste izbrisali kurs sa wishliste");
     this.UcitajKurseve();
   },
   error: err => {
     console.log("Error pri brisanju kursa: ",err);
   }
 })
  }

  openCourse(courseId: any) {
    this.router.navigate(['/student/course/details', courseId]);
  }
}
