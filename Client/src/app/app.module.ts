import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';
import { NgxGalleryModule } from 'ngx-gallery';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { AuthService } from './services/auth.service';
import { HomeComponent } from './components/home/home.component';
import { RegisterComponent } from './components/register/register.component';
import { ErrorInterceptorProvider } from './services/error.interceptor';
import { AlertifyService } from './services/alertify.service';
import { BsDropdownModule, TabsModule, BsDatepickerModule } from 'ngx-bootstrap';
import { MemberListComponent } from './components/members/member-list/member-list.component';
import { ListsComponent } from './components/lists/lists.component';
import { MessagesComponent } from './components/messages/messages.component';
import { appRoutes } from './routes';
import { AuthGuard } from './guards/auth.guard';
import { UserService } from './services/user.service';
import { MemberCardComponent } from './components/members/member-card/member-card.component';
import { MemberDetailComponent } from './components/members/member-detail/member-detail.component';
import { MemberDetailResolver } from './resolvers/member-detail.resolver';
import { MemberListResolver } from './resolvers/member-list.resolver';
import { MemberEditComponent } from './components/members/member-edit/member-edit.component';
import { MemberEditResolver } from './resolvers/member-edit.resolver';
import { UnsavedChangesGuard } from './guards/unsaved-changes.guard';
import { PhotoEditorComponent } from './components/members/photo-editor/photo-editor.component';
import { FileUploadModule } from 'ng2-file-upload';
import { TimeAgoPipe } from 'time-ago-pipe';


export function tokenGetter() {
  return localStorage.getItem("token");
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    ListsComponent,
    MessagesComponent,
    MemberCardComponent,
    MemberDetailComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    TimeAgoPipe
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgxGalleryModule,
    FileUploadModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ['localhost:54314'],
        blacklistedRoutes: ['localhost:54314/api/auth']
      }
    }),
    TabsModule.forRoot(),
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),

    RouterModule.forRoot(appRoutes)
  ],
  providers: [
    AuthService,
    ErrorInterceptorProvider,
    AlertifyService,
    AuthGuard,
    UnsavedChangesGuard,
    UserService,
    MemberDetailResolver,
    MemberListResolver,
    MemberEditResolver
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
