import { Component } from '@angular/core';
import { QuoteList } from './quotes-components/quote-list/quote-list';
import { Router } from '@angular/router';

@Component({
  selector: 'app-quotes',
  imports: [QuoteList],
  templateUrl: './quotes.html',
  styleUrl: './quotes.scss',
})
export class Quotes {
  constructor(private router: Router) {}

  async quoteAdd(): Promise<void> {
    await this.router.navigate(['/quoteForm']);
  }
}
