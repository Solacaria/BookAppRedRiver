import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';



interface Book {
  id: number;
  title: string;
  author: string;
  publishedDate: string;
};

@Component({
  selector: 'app-book-list',
  imports: [],
  templateUrl: './book-list.html',
  styleUrl: './book-list.scss',
})
export class BookList implements OnInit {
  books: Book[] = [];
  message = "";

  constructor(private router: Router) {};
  private readonly url = "http://localhost:5259/api/";

  ngOnInit(): void {
    this.loadBooks();
  };

  async loadBooks(): Promise<void> {
    const token = localStorage.getItem("token");

    const resp = await fetch(this.url + "Book/getAll", {
      method: "GET",
      headers: {
        "Authorization": `Bearer ${token}`
      }
    });

    const data = await resp.json();
    if (!resp.ok) {
      this.message = `${data.message}`;
      return;
    }
    this.books = data as Book[];

  };

  async editBook(book: Book): Promise<void>{
    await this.router.navigate(['/bookForm'],{
      state: {
        book
      }});
  }

  async deleteBook(book: Book): Promise<void>{
    const token = localStorage.getItem("token");

    const resp = await fetch (this.url + "Book/Delete", {
      method: "DELETE",
      headers: {
        "Authorization": `Bearer ${token}`,
        "Content-Type": "application/json",
      },
      body: JSON.stringify(book)
    });

    var data = await resp.json();

    if (!resp.ok){
      this.message = data.message;
      return;
    }

    this.message = "Boken är borttagen";
    await this.loadBooks();

  }

  sleep(ms: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }

  async addFive(){
    if (this.books.length >= 10){
      this.message = "Ta bort lite böker innan du slumpar in nya :)";
      return;
    }
    const firstNames = ["Harry", "Luke", "Darth", "Frodo", "Gandalf",
      "Bilbo", "Geralt", "Aragorn", "Legolas", "Leia", "Obi-Wan",
      "Anakin", "Jon", "Daenerys", "Hermione"
    ];

    const lastNames = ["Potter", "Malfoy", "Skywalker", "Baggins", "Gandalfsson", "of Rivia", 
      "Stark", "Targaryen", "Kenobi", "Organa", "Granger", "Lannister", "Solo"
    ];

    const titles = ["Return of the Jedi", "The Fellowship of the Ring",
      "The Empire Strikes Back", "Harry Potter and the Goblet of Fire",
      "A Game of Thrones", "The Two Towers", "Revenge of the Sith",
      "The Dark Side of the Moon", "The Deathly Hallows", "Attack of the Clones",
      "The Battle of Hogwarts", "The Rise of the Sith",  "The Last Dragon", "The Phantom Menace"
    ];

    for (let i = 0; i < 5; i++){
      const firstName = firstNames[Math.floor(Math.random() * firstNames.length)];
      const lastName = lastNames[Math.floor(Math.random() * lastNames.length)];
      const title = titles[Math.floor(Math.random() * titles.length)];
      const year = Math.floor(Math.random() * (2025 - 1900 + 1)) + 1900;

      const fullName = `${firstName} ${lastName}`;
  

      var book = {
        title: title,
        author: fullName,
        publishedDate: year.toString()
      }

      await this.addBook(book);
    }
    this.message = "5st böcker har lagt till.";
    await this.sleep(3500);
    this.message = "";
    await this.router.navigate(["books"]);
  }

  async addBook(book: any): Promise<void> {

    const token = localStorage.getItem("token");

    var resp = await fetch(this.url + "book/create", {
      method: 'POST',
      headers: {
        "Content-Type": "application/json",
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify(book),
    });
  }
 
}
