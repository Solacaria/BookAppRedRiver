import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

interface Quote {
  id: number;
  text: string;
  author: string;
  year: string;
}

@Component({
  selector: 'app-quote-list',
  imports: [],
  templateUrl: './quote-list.html',
  styleUrl: './quote-list.scss',
})
export class QuoteList implements OnInit {
  quotes: Quote[] = [];
  message = '';

  private readonly url = 'http://localhost:5259/api/';

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.loadQuotes();
  }

  async loadQuotes(): Promise<void> {
    const token = localStorage.getItem('token');
    const resp = await fetch(this.url + 'Quote/getAll', {
      method: 'GET',
      headers: {
        Authorization: `Bearer ${token}`
      }
    });

    const data = await resp.json();

    if (!resp.ok) {
      this.message = data.message ?? 'Citat kunde inte hämtas.';
      return;
    }

    this.quotes = data as Quote[];
  }

  async editQuote(quote: Quote): Promise<void> {
    await this.router.navigate(['/quoteForm'], {
      state: {
        quote
      }
    });
  }

  async deleteQuote(quote: Quote): Promise<void> {
    const token = localStorage.getItem('token');

    const resp = await fetch(this.url + `Quote/delete`, {
      method: 'DELETE',
      headers: {
        'Authorization': `Bearer ${token}`,
        "Content-Type": "application/json",
      },
      body: JSON.stringify(quote)
    });

    const data = await resp.json();

    if (!resp.ok) {
      this.message = data.message ?? 'Citatet kunde inte tas bort.';
      return;
    }

    this.message = 'Citatet är borttaget.';
    await this.loadQuotes();
  }
}
