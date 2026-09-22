import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-quote-form',
  imports: [FormsModule],
  templateUrl: './quote-form.html',
  styleUrl: './quote-form.scss',
})
export class QuoteForm {
  message = '';
  isEditMode = false;
  headline = 'Lägg till citat';

  //Citatmodel för form i HTML
  quote = {
    id: null,
    text: '',
    author: '',
    year: 'Unknown'
  };

  private readonly url = 'http://localhost:5259/api/';

  constructor(private router: Router) {
    //Hämtar och hanterar datan på det klickade citatet vid "Update"
    const navigation = this.router.currentNavigation();
    const quoteFromList = navigation?.extras.state?.['quote'];

    if (quoteFromList) {
      this.quote = {
        id: quoteFromList.id,
        text: quoteFromList.text,
        author: quoteFromList.author,
        year: quoteFromList.year
      };
      this.isEditMode = true;
      this.headline = 'Redigera citat';
    }
  }

  async quoteSave(): Promise<void> {
    //simpel validering
    if (this.quote.text.trim().length === 0 || this.quote.author.trim().length === 0) {
      this.message = 'Citat och författare måste anges.';
      return;
    }

    
    let newQuote = null;
    if (!this.isEditMode){
        newQuote = {
            text: this.quote.text,
            author: this.quote.author,
            year: this.quote.year
        }
    };

    const token = localStorage.getItem('token');
    const method = this.isEditMode ? 'PUT' : "POST";
    const endpoint = this.isEditMode ? 'Quote/Edit' : 'Quote/Create';
    const quoteTosend = this.isEditMode ? this.quote : newQuote;

    console.log(quoteTosend, endpoint, method);
    const resp = await fetch(this.url + `${endpoint}`, {
      method: method,
      headers: {
        'Authorization': `Bearer ${token}`,
        "Content-Type": "application/json",
      },
      body: JSON.stringify(quoteTosend)
    });

    const data = await resp.json();

    if (!resp.ok) {
      this.message = data.message ?? 'Citatet kunde inte sparas.';
      return;
    }

    this.message = this.isEditMode
      ? 'Citatet redigerades. Du blir nu omdirigerad.'
      : 'Citatet skapades. Du blir nu omdirigerad.';

    await this.sleep(2500);
    this.isEditMode = false;
    this.headline = "Lägg till citat";
    await this.router.navigate(['/quotes']);
  }

  cancel(): void {
    this.router.navigate(['/quotes']);
  }

  sleep(ms: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }
}
