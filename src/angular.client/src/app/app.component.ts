import {
  Component,
  ComponentRef,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { RequestComponent } from './request/request.component';
import { ResponseComponent } from './response/response.component';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'client';

  private hubConnection!: signalR.HubConnection;
  input!: string;
  @ViewChild('viewContainerRef', { read: ViewContainerRef })
  vcr!: ViewContainerRef;

  request!: ComponentRef<RequestComponent>;
  response!: ComponentRef<ResponseComponent>;

  ngOnInit() {
    this.startConnection();
    this.addChatGptListener();
  }

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:44318/chat')
      .build();
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch((err) => console.log('Error while starting connection: ' + err));
  };

  send() {
    this.hubConnection.send('SendMessageToCaller', this.input);
    this.request = this.vcr.createComponent(RequestComponent);
    this.request.instance.message = this.input;
    this.input = '';

    this.response = this.vcr.createComponent(ResponseComponent);
    this.response.instance.message = '';
  }

  public addChatGptListener = () => {
    this.hubConnection.on('ReceiveMessage', (data) => {
      const parsedMessage = JSON.parse(data) as MessagePayload;
      if (parsedMessage.M !== null) {
        this.response.instance!.message! += parsedMessage.M;
      } else {
        this.response.instance!.cursorActive =
          !this.response.instance!.cursorActive;
      }

      console.log(data);
    });


    this.hubConnection.on('InitialMessage', (data) => {
      const parsedMessage = JSON.parse(data) as InitialMessages;

        if(parsedMessage.S === 'User')
        {
          this.request = this.vcr.createComponent(RequestComponent);
          this.request.instance.message = parsedMessage.M;
        }
        else{
          this.response = this.vcr.createComponent(ResponseComponent);
          this.response.instance.message = parsedMessage.M;
        }
    });

    
  };

  keydown(event: any) {
    if (event.key === 'Enter') {
      this.send();
    }
  }
}

export interface MessagePayload {
  M: string | null;
  E: boolean;
}


export interface InitialMessages {
  M: string | null;
  S: string | null;
}
