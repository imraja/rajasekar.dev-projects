import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  serverMessages: string[] = [];
  ngOnInit(): void {
    let connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5213/latest-messages")
      .build();

    connection.on("Receive-Messages", data => {
      this.serverMessages.push(data);
      console.log(data);
    });

     connection
     .start()
     .then(() => console.log('Connection started'))
     .catch(err => console.log('Error while starting connection: ' + err))
    //   .then(() => connection.invoke("send", "Hello"));

  }
  title = 'SingnalrClient';
}
