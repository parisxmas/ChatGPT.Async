import { Component } from '@angular/core';

@Component({
  selector: 'app-response',
  templateUrl: './response.component.html',
  styleUrls: ['./response.component.scss'],
})
export class ResponseComponent {
  public message: string | null = null;
  public cursorActive = false;
}
