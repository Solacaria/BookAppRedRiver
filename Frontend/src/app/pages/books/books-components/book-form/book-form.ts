import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-book-form',
  imports: [FormsModule],
  templateUrl: './book-form.html',
  styleUrl: './book-form.scss',
})
export class BookForm {
  message = "";
  isEditMode = false;
  headline = "Lägg till bok";

  //Bokmodell för form i HTML
  book = {
    id: null,
    title: "",
    author: "",
    publishedDate: ""
  }
  constructor(private router: Router) {
    //Hämtar och hanterar datan på den klickade boken vid "Update"
    const navigation = this.router.currentNavigation();
    const bookFromList = navigation?.extras.state?.['book'];

    if (bookFromList) {
      this.book = {
        id: bookFromList.id,
        title: bookFromList.title,
        author: bookFromList.author,
        publishedDate: bookFromList.publishedDate,
      }
      this.isEditMode = true;
      this.headline = "Redigera bok";
    }
  }
  url = 'http://localhost:5259/api/';

  async bookSave(){
    //Simpel validering
    if (this.book.author.length <= 4 || this.book.title.length <= 4 || !this.book.publishedDate.trim()){
      this.message = "Titel och Författare behöver båda vara minst 4 tecken långt, och publiceringsår måste anges.";
      return;
    };

    //Ifall ny bok skapas, så blir det inge problem med ID i backend
    let newBook = null;
    if (!this.isEditMode){
      newBook = {
        title: this.book.title,
        author: this.book.author,
        publishedDate: this.book.publishedDate,
      }
    };


    //Säkerställer att samma formulär kan användas för både Create och Update
    const token = localStorage.getItem("token");
    const method = this.isEditMode ? 'PUT' : "POST";
    const endpoint = this.isEditMode ? 'Book/Edit' : 'Book/Create';
    const bookToSend = this.isEditMode ? this.book : newBook;

    
    var resp = await fetch(this.url + `${endpoint}`, {
      method: method,
      headers: {
        "Content-Type": "application/json",
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify(bookToSend),
    });

    var data = await resp.json();

    if (!resp.ok){
      this.message = `${data.response}`;
      return;
    }

    this.message = this.isEditMode 
      ? 'Boken redigerades. Du blir nu omdirigerad.' 
      : 'Boken skapades. Du blir nu omdirigerad.';
   
    await this.sleep(2500);
    this.isEditMode = false;
    this.headline = "Lägg till bok";
    await this.router.navigate(["books"]);
  }

  cancel(){
    //Tar dig tillbaka helt enkelt :)
    this.router.navigate(["books"]);
  }

  sleep(ms: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }
}
