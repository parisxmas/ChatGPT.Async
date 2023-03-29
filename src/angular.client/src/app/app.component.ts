import {
  ChangeDetectorRef,
  Component,
  ComponentRef,
  ElementRef,
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

  @ViewChild('history') private scrollContainer!: ElementRef;

  request!: ComponentRef<RequestComponent>;
  response!: ComponentRef<ResponseComponent>;

  constructor(private changeDetector: ChangeDetectorRef) {}

  ngOnInit() {
    this.startConnection();
    this.addChatGptListener();
  }

  ngAfterContentChecked(): void {
    this.changeDetector.detectChanges();
  }

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7093/chat')
      .withAutomaticReconnect()
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

  formatMessage(message: string) {
    return message.replaceAll('\n', '<br>');
  }

  public addChatGptListener = () => {
    this.hubConnection.on('ReceiveMessage', (data) => {
      const parsedMessage = JSON.parse(data) as MessagePayload;

      switch (parsedMessage.M) {
        case 'SetCaretActive':
          this.response.instance!.cursorActive = true;
          break;
        case 'SetCaretInactive':
          this.response.instance!.cursorActive = false;
          break;
        case null:
          break;
        default:
          this.response.instance!.message! += this.formatMessage(
            parsedMessage.M
          );
      }

      //      console.log(data);
    });

    this.hubConnection.on('InitialMessage', (data) => {
      const parsedMessage = JSON.parse(data) as InitialMessages;

      if (parsedMessage.S === 'User') {
        this.request = this.vcr.createComponent(RequestComponent);
        this.request.instance.message = this.formatMessage(parsedMessage.M!);
      } else {
        this.response = this.vcr.createComponent(ResponseComponent);
        this.response.instance.message = this.formatMessage(parsedMessage.M!);
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
