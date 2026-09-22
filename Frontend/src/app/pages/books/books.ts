import { Component } from '@angular/core';
import { BookList } from './books-components/book-list/book-list';
import { Router } from '@angular/router';

@Component({
  selector: 'app-books',
  imports: [BookList],
  templateUrl: './books.html',
  styleUrl: './books.scss',
})
export class Books {
  constructor(private router: Router) {}



  async bookAdd(): Promise<void> {
    await this.router.navigate(['/bookForm']);
  }

  bookEdit(): void {
  }

  bookDelete(): void {
  }
}
