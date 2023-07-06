import { Component, Input } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { PhotosService } from 'src/app/features/publications/services/photos.service';
import { OffersService } from '../../services/offers.service';
import { IPhoto } from '../../interfaces/photo.interface';

@Component({
  selector: 'app-add-offer',
  templateUrl: './add-offer.component.html',
  styleUrls: ['./add-offer.component.scss'],
})
export class AddOfferComponent {
  createForm: FormGroup = new FormGroup({});
  loading = false;
  @Input() publicationId: string = '';

  constructor(
    private offersService: OffersService,
    private photoService: PhotosService
  ) {
    this.generateForm();
  }

  generateForm() {
    this.createForm = new FormGroup({
      title: new FormControl(
        { value: '', disabled: false },
        {
          validators: [
            Validators.required,
            Validators.minLength(5),
            Validators.maxLength(100),
          ],
        }
      ),
      content: new FormControl(
        { value: '', disabled: false },
        {
          validators: [
            Validators.required,
            Validators.minLength(5),
            Validators.maxLength(200),
          ],
        }
      ),
    });
  }

  onSubmit() {
    const { title, content } = this.createForm.value;

    this.loading = true;
    this.offersService
      .createOffer(this.publicationId, title, content, this.photosToUpload)
      .subscribe({
        next: () => {
          this.loading = false;
          this.createForm.reset();
          this.photosToUpload = [];
        },
        error: (err: any) => {
          this.loading = false;
          this.createForm.reset();
          this.photosToUpload = [];
          console.log(err);
        },
      });
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
    // this.photoService
    //   .removePhoto(photoId, this.publication?.id ?? '')
    //   .subscribe({
    //     next: () => {
    //       this.photosToUpload = this.photosToUpload.filter(
    //         (p) => p.photoId !== photoId
    //       );
    //     },
    //   });
  }
}
