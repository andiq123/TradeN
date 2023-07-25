import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { IPhoto } from 'src/app/features/publications/interfaces/photo.interface';
import { IPublication } from 'src/app/features/publications/interfaces/publication.interface';
import { PhotosService } from 'src/app/features/publications/services/photos.service';
import { PublicationsService } from 'src/app/features/publications/services/publications.service';

@Component({
  selector: 'app-add-publication',
  templateUrl: './add-publication.component.html',
  styleUrls: ['./add-publication.component.scss'],
})
export class AddPublicationComponent implements OnInit {
  createForm: FormGroup = new FormGroup({});
  @Input() publication: IPublication | null = null;
  loading = false;
  isEditMode = false;
  @Input() addButtonStyles = false;

  constructor(
    private publicationsService: PublicationsService,
    private photoService: PhotosService
  ) {}

  ngOnInit(): void {
    this.generateForm();
    this.isEditMode = !!this.publication;
    if (this.isEditMode) {
      this.photosToUpload = this.publication?.photos ?? [];
    }
  }

  onSubmit() {
    const { title, content, desiredItem } = this.createForm.value;
    this.loading = true;
    if (!this.isEditMode) {
      this.publicationsService
        .createPublication(title, content, desiredItem, this.photosToUpload)
        .subscribe({
          next: () => {
            this.loading = false;
            this.createForm.reset();
            this.photosToUpload = [];
          },
          error: (err: any) => {
            this.loading = false;
            this.createForm.reset();
            console.log(err);
          },
        });
    } else {
      this.publicationsService
        .updatePublication(
          this.publication?.id,
          title,
          content,
          this.photosToUpload
        )
        .subscribe({
          next: () => {
            this.loading = false;
          },
          error: (err: any) => {
            this.loading = false;
            console.log(err);
          },
        });
    }
  }

  photosToUpload: IPhoto[] = [];
  onFileChange(event: any) {
    const file = event.target.files[0];
    //call photo service and log upload progress

    this.photoService.uploadPhoto(file).subscribe({
      next: (photo: IPhoto) => {
        this.photosToUpload = [...this.photosToUpload, photo];
        if (this.photosToUpload.length === 1) {
          this.selectMain(photo.id);
        }
      },
    });
  }

  selectMain(id: string) {
    this.photosToUpload = this.photosToUpload.map((p) => {
      if (p.id === id) {
        p.isMain = true;
        return p;
      }
      p.isMain = false;
      return p;
    });
  }

  removePhoto(photoId: string) {
    this.photoService
      .removePhoto(photoId, this.publication?.id ?? '')
      .subscribe({
        next: () => {
          this.photosToUpload = this.photosToUpload.filter(
            (p) => p.photoId !== photoId
          );
        },
      });
  }

  generateForm() {
    this.createForm = new FormGroup({
      title: new FormControl(
        { value: this.publication?.title ?? '', disabled: false },
        {
          validators: [
            Validators.required,
            Validators.minLength(5),
            Validators.maxLength(100),
          ],
        }
      ),
      content: new FormControl(
        { value: this.publication?.content ?? '', disabled: false },
        {
          validators: [
            Validators.required,
            Validators.minLength(5),
            Validators.maxLength(1500),
          ],
        }
      ),
      desiredItem: new FormControl(
        { value: this.publication?.desiredItem ?? '', disabled: false },
        {
          validators: [
            Validators.required,
            Validators.minLength(5),
            Validators.maxLength(100),
          ],
        }
      ),
    });
  }
}
