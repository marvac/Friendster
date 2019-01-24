import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { Photo } from 'src/app/models/photo';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';
import { AlertifyService } from 'src/app/services/alertify.service';


@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  @Output() getMemberPhotoChange = new EventEmitter<string>();

  uploader: FileUploader;
  hasBaseDropZoneOver: boolean = false;
  currentMainPhoto: Photo;
  private usersUrl = '/api/users/';

  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: `${this.usersUrl}${this.authService.decodedToken.nameid}/photos`,
      authToken: `Bearer ${localStorage.getItem('token')}`,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024 //10MB
    })

    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };

        this.photos.push(photo);
      }
    }
  }

  setMainPhoto(photo: Photo) {
    const userId = this.authService.decodedToken.nameid;
    this.userService.setMainPhoto(userId, photo.id)
      .subscribe(() => {
        this.currentMainPhoto = this.photos.filter(p => p.isMain)[0];
        this.currentMainPhoto.isMain = false;
        photo.isMain = true;
        this.authService.changeProfilePhoto(photo.url);
        this.authService.currentUser.photoUrl = photo.url;
        localStorage.setItem('user', JSON.stringify(this.authService.currentUser))
      }, error => {
        this.alertify.error(error);
      })
  }

  deletePhoto(photoId: number) {
    this.alertify.confirm("Are you sure you want to delete this photo?", () => {
      const userId = this.authService.decodedToken.nameid;
      this.userService.deletePhoto(userId, photoId)
        .subscribe(() => {
          const index = this.photos.findIndex(p => p.id == photoId);
          this.photos.splice(index, 1);
          this.alertify.success("Photo deleted");
        }, error => {
          this.alertify.error(error);
        })
    });
  }
}
