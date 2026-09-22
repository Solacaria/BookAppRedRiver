import { Routes } from '@angular/router';
import { Books } from './pages/books/books';
import { Login } from './pages/login/login';
import { Quotes } from './pages/quotes/quotes';
import { Register } from './pages/register/register';
import { BookForm } from './pages/books/books-components/book-form/book-form';
import { QuoteForm } from './pages/quotes/quotes-components/quote-form/quote-form';



export const routes: Routes = [
    { path: 'books',component: Books },
    { path: 'login',component: Login },
    { path: 'quotes',component: Quotes},
    { path: 'register', component: Register },
    { path: "bookForm", component: BookForm},
    { path: "quoteForm", component: QuoteForm},
    { path: '', redirectTo: '/login', pathMatch: 'full' }
];
