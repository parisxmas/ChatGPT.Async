import { Component } from '@angular/core';
import * as signalR from "@microsoft/signalr"
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'client';

  private hubConnection!: signalR.HubConnection;
  input! :string;

  ngOnInit() {
    this.startConnection();
    this.addTransferChartDataListener();
    
  }
  
  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
                            .withUrl('https://localhost:7093/chat')
                            .build();
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  send() {
    this.hubConnection.send("SendMessageToCaller", this.input);
  }
  
  public addTransferChartDataListener = () => {
    this.hubConnection.on('ReceiveMessage', (data) => {
     
      console.log(data);
    });
  }
}
