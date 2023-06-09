import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { RequestComponent } from './request/request.component';
import { ResponseComponent } from './response/response.component';
import { ErrorComponent } from './error/error.component';
import { SanitizeHtmlPipe } from './domSanitizer.pipe';
@NgModule({
  declarations: [
    AppComponent,
    RequestComponent,
    ResponseComponent,
    ErrorComponent,
    SanitizeHtmlPipe,
  ],
  imports: [BrowserModule, FormsModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
